using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerAction : MonoBehaviour, IDamageable, ICharacterAnimationProvider, ILongPressInformation
{
    //回避中か
    private bool isAvoid;
    //攻撃中か
    private bool isAttack;
    [SerializeField, Header("アニメーション時間")]
    private float avoidMoveTime = 0.2f;//回避秒数(回避アニメーション移動部分の時間)
    [SerializeField]
    private float avoidMoveDistance = 2.0f;//回避距離
    [SerializeField]
    private float knockbackMoveTime;//ノックバック秒数(ノックバックアニメーション移動部分の時間)
    [SerializeField]
    private float knockbackMoveDistance = 2.0f;
    [SerializeField]
    private float rayDistance = 10.0f;
    [SerializeField]
    private float limitRayDistance = 2.0f;

    [SerializeField, Header("持っている武器")]
    private Weapon weapon;
    [SerializeField, Header("カウンター判定の存在する時間(秒)")]
    private float counterTime;
    private PlayerAnimation playerAnimation; //アニメーション管理
    private PlayerState mState; //プレイヤーの状態
    public PlayerState state
    {
        private set
        {
            mState = value;
            //ディフェンス以外になったらフラグ変更
            if (value != PlayerState.Defence)
            {
                this.isGuard = false;
            }
        }
        get { return mState; }
    }
    private PlayerStatus status; //プレイヤーのステータス
    private bool isGuard; //guardしているか
    private float counterOccuredTime; //カウンターが発生した時間保存用
    private Coroutine counterCoroutine; //カウンター用コルーチン
    private bool canPierceAndHeal; //吸生できるか
    private List<EnemyAI> nearCanPierceEnemyList; //十分近づいている吸生可能な敵リスト
    [SerializeField, Header("吸生テキスト")]
    private Text pierceText;
    [SerializeField, Header("吸生での回復量")]
    private int pierceHealHP;
    [SerializeField]
    private StageManager stageManager;//ステージマネージャー
    [SerializeField, Header("1スタミナが減少する時間（秒）")]
    private float decreaseStaminaPerSecond;
    [SerializeField, Header("ダッシュによって減るスタミナの量")]
    private float decreaseDashStamina = 1;
    private Dash dash; //ダッシュ管理
    [SerializeField, Header("攻撃時に減るスタミナ量")]
    private int decreaseAttackStamina;
    [SerializeField, Header("回避時に減るスタミナ量")]
    private int decreaseAvoidStamina = 10;
    [SerializeField]
    private CameraController playerCamera;
    [SerializeField]
    private PlayableDirector movie;

    [SerializeField]
    private SceneName nextSceneName;
    [SerializeField]
    private float walkSpeed = 10f;
    [SerializeField]
    private float dashSpeed = 15f;
    [SerializeField]
    private PlayerAttackSequence attackSequence;

    public CharacterAnimation characterAnimation { get { return playerAnimation; } }

    //ILongPressInformation
    public string longPressMessage { get { return "吸生";}}
    public bool canLongPress { get { return this.CanPierce(); }}

    void Start()
    {
        this.status = GetComponent<PlayerStatus>();
        //this.playerAnimation = new PlayerAnimation(GetComponent<Animator>());
        this.playerAnimation = GetComponent<PlayerAnimation>();
        this.isGuard = false;
        this.counterOccuredTime = -1;
        this.state = PlayerState.Idle;
        this.isAvoid = false;
        this.isAttack = false;
        status = GetComponent<PlayerStatus>();
        canPierceAndHeal = false;
        nearCanPierceEnemyList = new List<EnemyAI>();
        dash = new Dash(decreaseStaminaPerSecond, DecreaseDashStamina);
        Assert.IsTrue(decreaseAttackStamina > 0);
        Assert.IsTrue(decreaseStaminaPerSecond > 0);
        //攻撃が終わるたびにステートを戻す
        if(this.attackSequence == null) {
            this.attackSequence = GetComponent<PlayerAttackSequence>();
        }
        attackSequence.OnAttackPhaseFinished += () => state = PlayerState.Idle;
    }

    /// <summary>
    /// ダッシュ時のスタミナ減少
    /// </summary>
    public void DecreaseDashStamina()
    {
        status.DecreaseStamina(decreaseDashStamina);
    }

    private void MoveStop()
    {
        dash.Reset();
        playerAnimation.StopRunAnimation();
        playerAnimation.StopWalkAnimation();
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="changeRotation"></param>
    public void Move(Vector3 dir, bool changeRotation)
    {
        //移動できない状態なら移動しない
        if (!CanMove()) return;

        //簡易的にガード中の移動量を半減
        float speed = walkSpeed;
        if (isGuard) speed /= 5;

        //こうしないとコントローラのスティックがニュートラルに戻った時、
        //勝手に前を向いてしまう
        if (dir == Vector3.zero)
        {
            MoveStop();
            if (isGuard) SetGuardMoveDirection(dir);
            return;
        }
        playerAnimation.StartWalkAnimation();
        var pos = transform.position;
        transform.position += playerCamera.hRotation * dir * speed * Slow.Instance.PlayerDeltaTime();
        if (!isGuard)
        {
            if(changeRotation || isAvoid) { transform.rotation = Quaternion.LookRotation(dir, Vector3.up) * playerCamera.hRotation; }
        }
        else
        {
            SetGuardMoveDirection(dir);
        }

        if (!AudioManager.Instance.IsPlayingPlayerSE())
        {
            AudioManager.Instance.PlayPlayerSE(AudioName.SE_WALK.String());
        }
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

        if (!isGuard) playerAnimation.StartRunAnimation();
        var pos = transform.position;
        dash.Update(Slow.Instance.PlayerDeltaTime());
        float t = Mathf.Clamp(dash.dashTimeCounter, 1.0f, 10.0f);
        //transform.position += dir * 10 * Slow.Instance.playerDeltaTime;
        transform.position += playerCamera.hRotation * dir * 10 * t * Slow.Instance.PlayerDeltaTime();
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up) * playerCamera.hRotation;
        if (!AudioManager.Instance.IsPlayingPlayerSE())
        {
            AudioManager.Instance.PlayPlayerSE(AudioName.SE_DASH.String());
        }
    }

    /// <summary>
    /// 攻撃を開始します。
    /// </summary>
    public void Attack()
    {
        //吸生中なら攻撃しない
        if (state == PlayerState.Pierce) return;
        //スタミナが０なら攻撃できない
        if (status.GetStamina() == 0 || status.GetStamina() < 5) return;

        //防御していなければ通常の攻撃
        if (!isGuard)
        {
            if(attackSequence.Attack() == AttackResult.OK) {
                state = PlayerState.Attack;
                status.DecreaseStamina(decreaseAttackStamina);
            }
            //StartCoroutine(StartAttack());
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
    public void GuardStart(Vector3 dir)
    {
        //スタミナが０ならガードできない
        if (status.GetStamina() == 0) return;

        this.isGuard = true;
        this.state = PlayerState.Defence;
        //TODO:ここでガードモーションに入る

        SetGuardMoveDirection(dir);
    }

    /// <summary>
    /// ガード移動方向
    /// </summary>
    public void SetGuardMoveDirection(Vector3 dir)
    {
        if (dir.x < -0.1f)
        {
            playerAnimation.StartGuardWalkAnimation();
            playerAnimation.SetGuardSpeed(1);   //左歩き
            return;
        }
        if (dir.x > 0.1f)
        {
            playerAnimation.StartGuardWalkAnimation();
            playerAnimation.SetGuardSpeed(-1);  //右歩き
            return;
        }
        
        if (Mathf.Abs(dir.z) > 0)//前後移動
        {
            playerAnimation.StartGuardWalkAnimation();
            playerAnimation.SetGuardSpeed(1);   //左歩き
            return;
        }

        if (Mathf.Abs(dir.x) <= 0.1f)
        {
            playerAnimation.StopGuardWalkAnimation();
            playerAnimation.StartGuardAnimation();
        }

    }

    /// <summary>
    /// ガードを終了します。
    /// </summary>
    public void GuardEnd()
    {
        //ガード中やノックバック中でなければガード終了処理をしない
        if (state != PlayerState.Defence &&
            state != PlayerState.KnockBack) return;
        this.isGuard = false;
        playerAnimation.StopGuardWalkAnimation();
        playerAnimation.StopGuardAnimation();
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
        EnemyAI nearEnemy = MostNearEnemy();
        var isBoss = nearEnemy.GetComponent<BossMarker>() != null;
        //敵のほうを向くまで待機
        yield return StartCoroutine(RotateToTarget(nearEnemy.transform, 5.0f));
        status.Heal(pierceHealHP);
        //吸生終了まで待機
        playerAnimation.StartKyuuseiAnimation();
        yield return new WaitForSeconds(2.0f);
        playerAnimation.StopKyuuseiAnimation();
        FarEnemy(nearEnemy);
        nearEnemy.UsedHeal();
        state = PlayerState.Idle;
        if (isBoss)
        {
            SceneChanger.Instance().Change(nextSceneName, new FadeData(1, 1, Color.black));
        }
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
        if (state == PlayerState.KnockBack) return false;
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
    private EnemyAI MostNearEnemy()
    {
        EnemyAI enemy = nearCanPierceEnemyList[0];
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
        playerAnimation.StartCounterAnimation();
        yield return new WaitForSeconds(counterTime);
        playerAnimation.StopGuardAnimation();
        Debug.Log("counter end");
        state = PlayerState.Idle;
    }

    /// <summary>
    /// 攻撃を受けた時の処理
    /// </summary>
    /// <param name="damageSource"></param>
    public void OnHit(DamageSource damageSource)
    {
        if (state == PlayerState.Defence)//防御中
        {
            status.DecreaseStamina(damageSource.damage);
            //ノックバックされる
            BeKnockedBack(damageSource);
        }
        else if (state == PlayerState.Avoid)
        {

        }
        else if (state == PlayerState.Counter)
        {
            DoCounter(damageSource);
        }
        else
        {
            Debug.Log("player damaged");
            Damage(damageSource);
        }
    }

    private void DoCounter(DamageSource damageSource)
    {
        //カウンター発生から経過した時間
        float counterDeltaTime = Time.time - counterOccuredTime;
        //カウンター発生時間内ならカウンター発生
        if (counterDeltaTime < counterTime)
        {
            Debug.Log("counter succeed");
            damageSource.attackCharacter.Countered();
            GuardEnd();
            //スロー中でない時のみ
            if (!Slow.Instance.isSlowNow)
                Slow.Instance.SlowStart(CollectAllCharacterAnimation());
        }
        //失敗したら自分にダメージ
        else
        {
            GuardEnd();
            Debug.Log("counter failed");
            Damage(damageSource);
        }
    }

    private List<CharacterAnimation> CollectAllCharacterAnimation()
    {
        var ret = new List<CharacterAnimation>();
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (!obj.activeInHierarchy) continue;
            var animation = obj.GetComponent<CharacterAnimation>();
            if (animation == null) continue;
            ret.Add(animation);
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
            //StartPosition(stageManager.RestartPosition());
            //status.Reset();

            //プレイヤーが戦闘不能になった時メニューを出す
            var fadeData = new FadeData(1f, 1f, Color.black);
            SceneChanger.Instance().Change(SceneName.PlayerDead, fadeData);
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
        if (this.state == PlayerState.Avoid)
        {
            return;
        }
        //スタミナ
        if (status.GetStamina() < decreaseAvoidStamina)
        {
            return;
        }

        //ガード中回避したらガード解除
        if (isGuard) GuardEnd();
        state = PlayerState.Avoid;

        //回避コルーチンを開始する
        StartCoroutine(AvoidCoroutine(dir));

        status.DecreaseStamina(decreaseAvoidStamina);

        //回避行動中は他のアクションを実行できないように
        //PlayerControllerでisAvoidがtrueの時他のメソッドのUPDATEを停止
    }

    private IEnumerator AvoidCoroutine(Vector3 dir)
    {
        if (isAvoid) yield break;

        isAvoid = true;

        float timeForChangeState = 0.3f;


        //開始位置
        var startPos = transform.position;
        //何の方向もない時
        if (dir == Vector3.zero)
        {
            //後ろ回避アニメーション
            playerAnimation.StartBackAvoidAnimation();
            //SE
            AudioManager.Instance.PlayPlayerSE(AudioName.SE_DODGE.String());

            yield return DirectionAvoid(-transform.forward);

            yield return new WaitForEndOfFrame();

            //TODO:ここに体制立ち直る隙間時間
            yield return new WaitForSeconds(timeForChangeState);

            isAvoid = false;
            state = PlayerState.Idle;
            yield break;
        }


        //ガード以外方向入力がある時、四方向に前進回避アニメーション
        if (!isGuard)
        {
            //前進回避アニメーション
            playerAnimation.StartForwardAvoidAnimation();
            yield return DirectionAvoid(playerCamera.hRotation * dir.normalized);

            yield return new WaitForEndOfFrame();
            //TODO:ここに体制立ち直る隙間時間
            yield return new WaitForSeconds(timeForChangeState);

            isAvoid = false;
            state = PlayerState.Idle;
            yield break;
        }


        //ガード中方向入力がある時(四方向個別にアニメーションあり)、rotation維持
        //Dot()->同じ方向1、垂直0、正反対-1
        //前
        if (Vector3.Dot(transform.forward, dir) >= 0.4f)
        {
            //前進回避アニメーション
            playerAnimation.StartForwardAvoidAnimation();
            yield return DirectionAvoid(playerCamera.hRotation * dir.normalized);
        }
        //横
        else if (Vector3.Dot(transform.forward, dir) < 0.4f &&
                Vector3.Dot(transform.forward, dir) > -0.4f)
        {
            //右回避アニメーション
            if (dir.x > 0f)
            {
                playerAnimation.StartRightAvoidAnimation();
                yield return DirectionAvoid(playerCamera.hRotation * dir.normalized);
            }
            //左回避アニメーション
            if (dir.x < 0f)
            {
                playerAnimation.StartLeftAvoidAnimation();
                yield return DirectionAvoid(playerCamera.hRotation * dir.normalized);
            }
        }
        //後ろ
        else//Vector3.Dot(transform.forward, dir) <= -0.4f
        {
            //後ろ回避アニメーション
            playerAnimation.StartBackAvoidAnimation();
            yield return DirectionAvoid(playerCamera.hRotation * dir.normalized);
        }

        //SE
        AudioManager.Instance.PlayPlayerSE(AudioName.SE_DODGE.String());

        yield return new WaitForEndOfFrame();
        //TODO:ここに体制立ち直る隙間時間
        yield return new WaitForSeconds(timeForChangeState);

        isAvoid = false;
        state = PlayerState.Idle;
        yield break;
    }

    private IEnumerator DirectionAvoid(Vector3 dir)
    {
        var col = GetComponent<Collider>();

        LayerMask mask = LayerMask.GetMask("Stage");
        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), dir.normalized);
        RaycastHit hit;    
        var offset = 0f;
        var start = transform.position;
        while (offset < avoidMoveTime)
        {
            float dis;
            var t = Time.time;
            yield return new WaitForEndOfFrame();
            col.enabled = false;
            var diff = (Time.time - t);
            offset += diff;
            var percent = offset / avoidMoveTime;
            if (Physics.Raycast(ray, out hit, rayDistance, mask))
            {
                Debug.Log("stageに当たった");
                dis = hit.distance;
                Debug.Log(dis);

                if (dis <= limitRayDistance)
                {
                    col.enabled = true;
                    yield break;
                }
            }
            col.enabled = true;

            transform.position = start + (dir * (avoidMoveDistance * percent));

        }
    }

    public bool IsAvoid()
    {
        return isAvoid;
    }

    /// <summary>
    /// 敵に近づいた
    /// </summary>
    public void NearEnemy(EnemyAI enemy)
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
    public void FarEnemy(EnemyAI enemy)
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

    /// <summary>
    /// ノックバックされる
    /// </summary>
    public void BeKnockedBack(DamageSource damageSource)
    {
        if (state == PlayerState.KnockBack) return;

        state = PlayerState.KnockBack;

        StartCoroutine(PlayerKockBack(damageSource));
    }

    private IEnumerator PlayerKockBack(DamageSource damageSource)
    {
        //ノックバックアニメーション
        playerAnimation.StartKnockBackAnimation();

        //敵が攻撃した方向取得
        //NOTE:EnemyActionクラスは削除されました
        EnemyAI enemy = (EnemyAI)damageSource.attackCharacter;
        Vector3 enemyPosXZ = Vector3.Scale(enemy.transform.position, new Vector3(1, 0, 1));
        Vector3 myPosXZ = Vector3.Scale(transform.position, new Vector3(1, 0, 1));
        Vector3 enemyAttackDir = (myPosXZ - enemyPosXZ).normalized;
        //敵に向いて
        transform.LookAt(enemyPosXZ + new Vector3(0, transform.position.y, 0));

        var offset = 0f;
        //開始位置
        var startPos = transform.position;
        Ray ray = new Ray(transform.position, -transform.forward);
        RaycastHit hit;
        float dis;
        var col = GetComponent<Collider>();
        //ノックバック後の位置
        var endPos = startPos + (-transform.forward * knockbackMoveDistance);
        while (offset < knockbackMoveTime)
        {
            var t = Time.time;
            yield return new WaitForEndOfFrame();
            col.enabled = false;
            var diff = (Time.time - t);
            offset += diff;
            var percent = offset / knockbackMoveTime;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, rayDistance, LayerMask.GetMask("Stage")))
            {
                dis = hit.distance;
                Debug.Log("ノックバック");
                if (dis < limitRayDistance)
                {
                    col.enabled = true;
                    //ガードボタンまだ押しているなら
                    if (isGuard)
                    {
                        //防御中に切り替える
                        state = PlayerState.Defence;
                    }
                    else//押してなかったら
                    {
                        //防御解除->通常状態
                        GuardEnd();
                    }
                    yield break;
                }
            }
            col.enabled = true;
            transform.position = Vector3.Lerp(startPos, endPos, percent);
        }
        yield return new WaitForEndOfFrame();

        //ガードボタンまだ押しているなら
        if (isGuard)
        {
            //防御中に切り替える
            state = PlayerState.Defence;
        }
        else//押してなかったら
        {
            //防御解除->通常状態
            GuardEnd();
        }
        yield break;
    }

    public void StartPosition(Vector3 position)
    {
        this.gameObject.transform.position = position;
    }
}
