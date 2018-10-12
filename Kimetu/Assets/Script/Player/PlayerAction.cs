using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerAction : MonoBehaviour, IDamageable
{
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

    void Start()
    {
        this.playerAnimation = new PlayerAnimation(GetComponent<Animator>());
        this.isGuard = false;
        this.state = PlayerState.Idle;
        status = GetComponent<PlayerStatus>();
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
    /// 回避行動を実行します。
    /// 数秒後に自動で回避状態は解除されます。
    /// </summary>
    public void Avoid()
    {
        this.state = PlayerState.Avoid;
        //TODO:コルーチンなどを開始する
        //TODO:回避行動中は他のアクションを実行できないように
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
}
