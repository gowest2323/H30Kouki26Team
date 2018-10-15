using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerAction : MonoBehaviour, IDamageable
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

	void Start() {
        this.status = GetComponent<PlayerStatus>();
		this.playerAnimation = new PlayerAnimation(GetComponent<Animator>());
		this.isGuard = false;
		this.counterOccuredTime = -1;
		this.state = PlayerState.Idle;
        this.isAvoid = false;
        this.avoidMoveTime = 0.5f;
    }

    public void Move(Vector3 dir)
    {
        //カウンター中は移動できない。
        if (state == PlayerState.Counter) return;
        //こうしないとコントローラのスティックがニュートラルに戻った時、
        //勝手に前を向いてしまう
        if (dir == Vector3.zero)
        {
            playerAnimation.StopRunAnimation();
            return;
        }
        playerAnimation.StartRunAnimation();
        var pos = transform.position;
        //transform.position += dir * 10 * Slow.Instance.playerDeltaTime;
        transform.position += dir * 10 * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
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

    private IEnumerator StartAttack()
    {
        weapon.AttackStart();
        yield return new WaitForSeconds(1);
        weapon.AttackEnd();
    }

    /// <summary>
    /// 攻撃を受けた時の処理
    /// </summary>
    /// <param name="damageSource"></param>
    public void OnHit(DamageSource damageSource)
    {
        if (state == PlayerState.Defence)
        {

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

    private void Damage(DamageSource damage)
    {
        status.Damage(damage.damage);
        //死亡したら倒れるモーション
        if (status.IsDead())
        {
            Destroy(this.gameObject);
            //playerAnimation.Start...();
        }
        //まだ生きていたらダメージモーション
        else
        {
            //playerAnimation.StartDamageAnimation();
        }
    }

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
            for(float i = 0; i <= avoidMoveTime; i += Time.deltaTime)
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
        if(Vector3.Dot(transform.forward, dir) >= 0.3f)
        {
            //前進回避アニメーション
            playerAnimation.StartForwardAvoidAnimation();
        }
        //横
        else if(Vector3.Dot(transform.forward, dir) < 0.3f &&
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
}
