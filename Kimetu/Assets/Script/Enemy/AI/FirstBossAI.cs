using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAI : EnemyAI, IEnemyInfoProvider
{
    private bool isAction; //行動中か？
    [SerializeField]
    private IdleAction idle;
    [SerializeField, Tooltip("振り下ろし")]
    private AttackAction swingAttack;
    [SerializeField, Tooltip("薙ぎ払い")]
    private AttackAction nagiharaiAttack;
    [SerializeField]
    private NearPlayerAction nearPlayer;
    [SerializeField]
    private DamageAction damage;
    [SerializeField]
    private DeathAction death;
    [SerializeField]
    private GameObject player;
    private EnemyStatus status;
    private EnemyAnimation enemyAnimation;

    public string informationText { private set; get; }

    private void Start()
    {
        status = GetComponent<EnemyStatus>();
        currentActionCoroutine = Think();
        canUseHeal = false;
    }

    public override void Countered()
    {
        //行動を停止し、ダメージアクションに移行
        StopCoroutine(currentActionCoroutine);
        currentActionCoroutine = StartCoroutine(damage.Action(ActionCallBack, DamagePattern.Countered));
        currentState = EnemyState.Damage;
        return;
    }

    public override void OnHit(DamageSource damageSource)
    {
        //すでに死亡しているなら何もしない
        //これがないと、死亡したエネミーに攻撃が当たったとき、
        //エネミーのローテーションがおかしくなる(PassOutが実行されるため??)
        if (status.IsDead()) { return; }
        ApplyDamage(damageSource);
        //現在の行動を停止
        CoroutineManager.Instance.StopCoroutine(currentActionCoroutine);
        //死亡したら倒れるモーション
        if (status.IsDead())
        {
            Death();
        }
        //まだ生きていたらダメージモーション
        else
        {
            currentState = EnemyState.Damage;
            //ダメージ状態に移行
            currentActionCoroutine = CoroutineManager.Instance.StartCoroutineEx(damage.Action(ActionCallBack));
            //enemyAnimation.StartDamageAnimation();
        }
    }

    /// <summary>
    /// 行動決定
    /// </summary>
    /// <returns></returns>
    protected override Coroutine Think()
    {
        Debug.Log("前の状態:" + currentState);
        isAction = true;
        if (status.IsDead())
        {
            return CoroutineManager.Instance.StartCoroutineEx(death.Action(DeadEnd));
        }
        switch (currentState)
        {
            //待機の次はプレイヤーに接近
            case EnemyState.Idle:
                currentState = EnemyState.MoveNear;
                var c = nearPlayer.Action(ActionCallBack);
                return CoroutineManager.Instance.StartCoroutineEx(nearPlayer.Action(ActionCallBack));
            //攻撃の次は待機
            case EnemyState.Attack:
                currentState = EnemyState.Idle;
                return CoroutineManager.Instance.StartCoroutineEx(idle.Action(ActionCallBack));
            //ダメージを受けた後は待機
            case EnemyState.Damage:
                currentState = EnemyState.Idle;
                return CoroutineManager.Instance.StartCoroutineEx(idle.Action(ActionCallBack));
            //接近した後は十分近づいていたら攻撃、もしくは待機
            case EnemyState.MoveNear:
                if (nearPlayer.isNearPlayer)
                {
                    currentState = EnemyState.Attack;
                    //振り下ろしが当たる範囲なら振り下ろす
                    if (swingAttack.CanAttack(player))
                    {
                        return StartCoroutine(swingAttack.Action(ActionCallBack));
                    }
                    else
                    {
                        return StartCoroutine(nagiharaiAttack.Action(ActionCallBack));
                    }
                }
                else
                {
                    currentState = EnemyState.Idle;
                    return CoroutineManager.Instance.StartCoroutineEx(idle.Action(ActionCallBack));
                }
            default:
                return CoroutineManager.Instance.StartCoroutineEx(idle.Action(ActionCallBack));
        }
    }

    private void Update()
    {
        Action();
        this.informationText = currentState.ToString();
        if (currentState == EnemyState.MoveNear)
        {
            informationText = nearPlayer.informationText;
        }
    }

    /// <summary>
    /// 行動する
    /// </summary>
    private void Action()
    {
        //行動中なら何もしない
        if (isAction) return;
        //行動していなければ行動を決定し実行
        currentActionCoroutine = Think();
    }

    /// <summary>
    /// 行動終了時に呼ばれる
    /// </summary>
    public void ActionCallBack()
    {
        isAction = false;
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Death()
    {
        CoroutineManager.Instance.StopCoroutine(currentActionCoroutine);
        currentActionCoroutine = Think();
    }

    /// <summary>
    /// 死亡後に呼ばれる
    /// </summary>
    public void DeadEnd()
    {
        canUseHeal = deathByRepl;
        if (deathByRepl)
        {
            ShowBeam();
        }
        else
        {
            Extinction();
        }
    }

    public override void UsedHeal()
    {
        base.UsedHeal();
        Destroy(this.gameObject);
    }
}
