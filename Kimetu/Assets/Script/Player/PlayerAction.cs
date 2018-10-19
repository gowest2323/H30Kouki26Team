using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerAction : MonoBehaviour, IDamageable, ICharacterAnimationProvider
{
    //回避中か
    private bool isAvoid;
    //回避秒数(回避アニメーション移動部分の時間)
    private float avoidMoveTime;

    [SerializeField, Header("持っている武器")]
    private Weapon weapon;
    [SerializeField, Header("カウンター判定の存在する時間(秒)")]
    private float counterTime;
    private PlayerAnimation playerAnimation; //アニメーション管理
    private PlayerState state; //プレイヤーの状態
    private PlayerStatus status; //プレイヤーのステータス
    private bool isGuard; //guardしているか
    private float counterOccuredTime; //カウンターが発生した時間保存用
    private Coroutine counterCoroutine; //カウンター用コルーチン
    private bool canPierceAndHeal; //吸生できるか
    private List<EnemyAction> nearCanPierceEnemyList; //十分近づいている吸生可能な敵リスト
    [SerializeField, Header("吸生テキスト")]
    private Text pierceText;
    [SerializeField, Header("吸生での回復量")]
    private int pierceHealHP;
    [SerializeField]
    private StageManager stageManager;//ステージマネージャー
    [SerializeField, Header("1スタミナが減少する時間（秒）")]
    private float decreaseStaminaPerSecond;
    private Dash dash; //ダッシュ管理
    [SerializeField, Header("攻撃時に減るスタミナ量")]
    private int decreaseAttackStamina;
    [SerializeField]
    private CameraController playerCamera; 
 


	public CharacterAnimation characterAnimation { get { return playerAnimation; }}

	void Start() {
        this.status = GetComponent<PlayerStatus>();
		this.playerAnimation = new PlayerAnimation(GetComponent<Animator>());
		this.isGuard = false;
		this.counterOccuredTime = -1;
		this.state = PlayerState.Idle;
        this.isAvoid = false;
        this.avoidMoveTime = 0.5f;
        status = GetComponent<PlayerStatus>();
        canPierceAndHeal = false;
        nearCanPierceEnemyList = new List<EnemyAction>();
        dash = new Dash(decreaseStaminaPerSecond, DecreaseDashStamina);
    }

    /// <summary>
    /// ダッシュ時のスタミナ減少
    /// </summary>
    public void DecreaseDashStamina()
    {
        status.DecreaseStamina(1);
    }

    private void MoveStop()
    {
        dash.Reset();
        playerAnimation.StopRunAnimation();
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="dir"></param>
    public void Move(Vector3 dir)
    {
        //移動できない状態なら移動しない
        if (!CanMove()) return;
        //こうしないとコントローラのスティックがニュートラルに戻った時、
        //勝手に前を向いてしまう
        if (dir == Vector3.zero)
        {
            MoveStop();
            return;
        }
        playerAnimation.StartRunAnimation();
        var pos = transform.position;
        transform.position += playerCamera.hRotation * dir * 10 * Slow.Instance.PlayerDeltaTime();
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up) * playerCamera.hRotation;
    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    /// <param name="dir">方向</param>
    /// <param name="dashTime">ダッシュしている時間</param>
    public void Dash(Vector3 dir)
    {
        //移動できない状態なら移動しない
        if (!CanMove()) return;
        //こうしないとコントローラのスティックがニュートラルに戻った時、
        //勝手に前を向いてしまう
        if (dir == Vector3.zero)
        {
            MoveStop();
            return;
        }

        playerAnimation.StartRunAnimation();
        var pos = transform.position;
        dash.Update(Time.deltaTime);
        float t = Mathf.Clamp(dash.dashTimeCounter, 1.0f, 10.0f);
        //transform.position += dir * 10 * Slow.Instance.playerDeltaTime;
        transform.position +=playerCamera.hRotation * dir * 10 * t * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up) * playerCamera.hRotation;
    }

    /// <summary>
    /// 攻撃を開始します。
    /// </summary>
    public void Attack()
    {
        //防御していなければ通常の攻撃
        if (!isGuard)
        {
            state = PlayerState.Attack;
            StartCoroutine(StartAttack());
            playerAnimation.StartAttackAnimation();
        }
        else
        {
            //既にカウンター中ならそれを終了する
            if (state == PlayerState.Counter)
            {
                StopCoroutine(counterCoroutine);
            }
            counterCoroutine = StartCoroutine(CounterStart());
        }
    }

    /// <summary>
    /// ガードを開始します。
    /// </summary>
    public void GuardStart()
    {
        this.isGuard = true;
        this.state = PlayerState.Defence;
        //TODO:ここでガードモーションに入る
    }

    /// <summary>
    /// ガードを終了します。
    /// </summary>
    public void GuardEnd()
    {
        //ガード中でなければガード終了処理をしない
        if (state != PlayerState.Defence) return;
        this.isGuard = false;
        this.state = PlayerState.Idle;
    }

    /// <summary>
    /// 吸生
    /// </summary>
    public void PierceAndHeal()
    {
        if (!CanPierce()) return;
        StartCoroutine(PierceAndHeakCoroutine());
    }

    /// <summary>
    /// 吸生
    /// </summary>
    /// <returns></returns>
    private IEnumerator PierceAndHeakCoroutine()
    {
        state = PlayerState.Pierce;
        EnemyAction nearEnemy = MostNearEnemy();
        //敵のほうを向くまで待機
        yield return StartCoroutine(RotateToTarget(nearEnemy.transform, 5.0f));
        status.Heal(pierceHealHP);
        //吸生終了まで待機
        yield return new WaitForSeconds(2.0f);
        FarEnemy(nearEnemy);
        nearEnemy.UsedHeal();
        state = PlayerState.Idle;
    }

    /// <summary>
    /// 移動できるか
    /// </summary>
    /// <returns></returns>
    private bool CanMove()
    {
        if (state == PlayerState.Avoid) return false;
        if (state == PlayerState.Counter) return false;
        if (state == PlayerState.Pierce) return false;
        return true;
    }

    /// <summary>
    /// 吸生できるか
    /// </summary>
    /// <returns></returns>
    private bool CanPierce()
    {
        if (!canPierceAndHeal) return false;
        if (state == PlayerState.Avoid) return false;
        if (state == PlayerState.Counter) return false;
        if (state == PlayerState.Defence) return false;
        if (state == PlayerState.Pierce) return false;
        if (state == PlayerState.Attack) return false;
        //吸生可能な敵がいなければできない
        if (nearCanPierceEnemyList.Count <= 0) return false;
        return true;
    }

    /// <summary>
    /// 最も近い吸生できる敵を取得
    /// </summary>
    /// <returns></returns>
    private EnemyAction MostNearEnemy()
    {
        EnemyAction enemy = nearCanPierceEnemyList[0];
        //一つしかないことがほとんどだと思うのでチェック
        if (nearCanPierceEnemyList.Count == 1) return enemy;
        //現在の最近敵の距離を持っておく
        float distance = Vector3.Distance(enemy.transform.position, transform.position);
        for (int i = 1; i < nearCanPierceEnemyList.Count; i++)
        {
            if (nearCanPierceEnemyList[i].canUseHeal) continue;
            float distance2 = Vector3.Distance(nearCanPierceEnemyList[i].transform.position, transform.position);
            if (distance2 < distance)
            {
                distance = distance2;
                enemy = nearCanPierceEnemyList[i];
            }
        }
        return enemy;
    }

    /// <summary>
    /// カウンター開始
    /// </summary>
    /// <returns></returns>
    private IEnumerator CounterStart()
    {
        Debug.Log("counter start");
        state = PlayerState.Counter;
        counterOccuredTime = Time.time;
        yield return new WaitForSeconds(counterTime);
        Debug.Log("counter end");
        state = PlayerState.Idle;
    }

    /// <summary>
    /// 攻撃開始
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartAttack()
    {
        weapon.AttackStart();
        status.DecreaseStamina(decreaseAttackStamina);
        yield return new WaitForSeconds(1);
        weapon.AttackEnd();
        state = PlayerState.Attack;
    }

    /// <summary>
    /// 攻撃を受けた時の処理
    /// </summary>
    /// <param name="damageSource"></param>
    public void OnHit(DamageSource damageSource)
    {
        if (state == PlayerState.Defence)
        {
            status.DecreaseStamina(damageSource.damage);
        }
        else if (state == PlayerState.Avoid)
        {

        }
        else if (state == PlayerState.Counter)
        {
            //カウンター発生から経過した時間
            float counterDeltaTime = Time.time - counterOccuredTime;
            //カウンター発生時間内ならカウンター発生
            if (counterDeltaTime < counterTime)
            {
                Debug.Log("counter succeed");
                damageSource.attackCharacter.Countered();
				Slow.Instance.SlowStart(CollectAllCharacterAnimation());
            }
            //失敗したら自分にダメージ
            else
            {
                Debug.Log("counter failed");
                Damage(damageSource);
            }
        }
        else
        {
            Debug.Log("player damaged");
            Damage(damageSource);
        }
    }
	private List<CharacterAnimation> CollectAllCharacterAnimation() {
		var ret = new List<CharacterAnimation>();
		foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
			if(!obj.activeInHierarchy) continue;
			var provider = obj.GetComponent<ICharacterAnimationProvider>();
			if(provider == null) continue;
			ret.Add(provider.characterAnimation);
		}
		return ret;
	}

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage"></param>
    private void Damage(DamageSource damage)
    {
        status.Damage(damage.damage);
        //死亡したら倒れるモーション
        if (status.IsDead())
        {            
            //Destroy(this.gameObject);
            this.gameObject.transform.position = stageManager.RestartPosition();
            //playerAnimation.Start...();
        }
        //まだ生きていたらダメージモーション
        else
        {
            //playerAnimation.StartDamageAnimation();
        }
    }

    /// <summary>
    /// カウンターされる
    /// </summary>
    public void Countered()
    {
        Debug.LogError("PlayerActionのCounteredが呼ばれました。");
    }

    /// <summary>
    /// 回避行動を実行します。
    /// 数秒後に自動で回避状態は解除されます。
    /// </summary>
    public void Avoid(Vector3 dir)
    {
        this.state = PlayerState.Avoid;

        //回避コルーチンを開始する
        StartCoroutine(AvoidCoroutine(dir));

        //回避行動中は他のアクションを実行できないように
        //PlayerControllerでisAvoidがtrueの時他のメソッドのUPDATEを停止
    }

    private IEnumerator AvoidCoroutine(Vector3 dir)
    {
        if (isAvoid) yield break;

        isAvoid = true;

        //何の方向もない時
        if (dir == Vector3.zero)
        {
            //後ろ回避アニメーション
            playerAnimation.StartBackAvoidAnimation();
            //後ろに移動
            for (float i = 0; i <= avoidMoveTime; i += Time.deltaTime)
            {
                transform.position += -transform.forward * 5 * Time.deltaTime;
                yield return null;
            }

            //TODO:ここに体制立ち直る隙間時間？

            isAvoid = false;
            yield break;
        }

        //方向入力がある時(四方向個別にアニメーションあり)、rotation維持
        //Dot()->同じ方向1、垂直0、正反対-1
        //前
        if (Vector3.Dot(transform.forward, dir) >= 0.3f)
        {
            //前進回避アニメーション
            playerAnimation.StartForwardAvoidAnimation();
        }
        //横
        else if (Vector3.Dot(transform.forward, dir) < 0.3f &&
                Vector3.Dot(transform.forward, dir) > -0.3f)
        {
            //右回避アニメーション
            if (dir.x > 0) playerAnimation.StartRightAvoidAnimation();
            //左回避アニメーション
            if (dir.x < 0) playerAnimation.StartLeftAvoidAnimation();
        }
        //後ろ
        else//Vector3.Dot(transform.forward, dir) <= -0.3f
        {
            //後ろ回避アニメーション
            playerAnimation.StartBackAvoidAnimation();
        }

        //向いている方向(正規化)に移動
        for (float i = 0; i <= avoidMoveTime; i += Time.deltaTime)
        {
            transform.position += dir.normalized * 5 * Time.deltaTime;
            yield return null;
        }

        //TODO:ここに体制立ち直る隙間時間？

        isAvoid = false;
        yield break;
    }

    public bool IsAvoid()
    {
        return isAvoid;
    }

    /// <summary>
    /// 敵に近づいた
    /// </summary>
    public void NearEnemy(EnemyAction enemy)
    {
        //敵が死亡していたらリストに追加し、吸生テキスト表示
        if (enemy.canUseHeal)
        {
            if (nearCanPierceEnemyList.Contains(enemy)) return;
            nearCanPierceEnemyList.Add(enemy);
            canPierceAndHeal = true;
            pierceText.enabled = true;
        }
    }

    /// <summary>
    /// 敵から離れた
    /// </summary>
    public void FarEnemy(EnemyAction enemy)
    {
        //死亡していたらリストから削除する
        if (enemy.canUseHeal)
        {
            nearCanPierceEnemyList.Remove(enemy);
            //もう吸生可能な敵が近くにいなければテキストを非表示に
            if (nearCanPierceEnemyList.Count <= 0)
            {
                canPierceAndHeal = false;
                pierceText.enabled = false;
            }
        }
    }

    /// <summary>
    /// ターゲットに向くように回転する
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <param name="maxDegreesDelta">1フレームに回転する最大角度</param>
    /// <returns></returns>
    private IEnumerator RotateToTarget(Transform target, float maxDegreesDelta)
    {
        Vector3 myPositionIgnoreY = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPositionIgnoreY = new Vector3(target.position.x, 0, target.position.z);
        Quaternion toTarget = Quaternion.LookRotation(targetPositionIgnoreY - myPositionIgnoreY);
        this.transform.rotation = toTarget;
        yield return null;
    }
}
