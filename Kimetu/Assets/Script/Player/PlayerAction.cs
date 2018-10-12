using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerAction : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Weapon weapon;
    private PlayerAnimation playerAnimation;
    private PlayerState state;
    private PlayerStatus status;
    private bool isGuard;
    private float counterTime;
    private float counterOccuredTime;

    void Start()
    {
        this.playerAnimation = new PlayerAnimation(GetComponent<Animator>());
        this.isGuard = false;
        this.counterTime = -1;
        this.counterOccuredTime = -1;
        this.state = PlayerState.Idle;
        status = GetComponent<PlayerStatus>();
    }


    public void Move(Vector3 dir)
    {
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
        this.state = PlayerState.Attack;
        StartCoroutine(StartAttack());
        playerAnimation.StartAttackAnimation();
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
        else
        {
            status.Damage(damageSource.damage);
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
    }
}
