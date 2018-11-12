using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneAttackPatternAI : EnemyAI, IEnemyInfoProvider
{
    private bool isAction; //行動中か？
    [SerializeField]
    private IdleAction idle;
    [SerializeField]
    private AttackAction attack;
    [SerializeField]
    private SearchAction search;
    [SerializeField]
    private NearPlayerAction nearPlayer;
    [SerializeField]
    private DamageAction damage;
    [SerializeField]
    private DeathAction death;
    private EnemyStatus status;

    public string informationText { get { return currentState.ToString(); } }

    private void Start()
    {
        status = GetComponent<EnemyStatus>();
        currentActionCoroutine = Think();
        canUseHeal = false;
    }

    public override void Countered()
    {
        Debug.Log("カウンター未実装");
        return;
    }

    public override void OnHit(DamageSource damageSource)
    {
        //すでに死亡しているなら何もしない
        //これがないと、死亡したエネミーに攻撃が当たったとき、
        //エネミーのローテーションがおかしくなる(PassOutが実行されるため??)
        if (status.IsDead()) { return; }
        status.Damage(damageSource.damage);
        StopCoroutine(currentActionCoroutine);
        currentActionCoroutine = StartCoroutine(damage.Action(ActionCallBack));
        //死亡したら倒れるモーション
        if (status.IsDead())
        {
            Death();
        }
        //まだ生きていたらダメージモーション
        else
        {
            currentState = EnemyState.Damage;
            //enemyAnimation.StartDamageAnimation();
        }
    }

    protected override Coroutine Think()
    {
        isAction = true;
        if (reserveState != EnemyState.None)
        {
            EnemyState next = reserveState;
            reserveState = EnemyState.None;

            if (next == EnemyState.MoveNear)
            {
                currentState = EnemyState.MoveNear;
                return StartCoroutine(nearPlayer.Action(ActionCallBack));
            }
        }
        switch (currentState)
        {
            case EnemyState.Idle:
                currentState = EnemyState.Search;
                return StartCoroutine(search.Action(ActionCallBack));
            case EnemyState.Attack:
                currentState = EnemyState.Idle;
                return StartCoroutine(idle.Action(ActionCallBack, 0.25f));
            case EnemyState.Search:
                if (search.canSearched)
                {
                    currentState = EnemyState.MoveNear;
                    return StartCoroutine(nearPlayer.Action(ActionCallBack));
                }
                else
                {
                    currentState = EnemyState.Idle;
                    return StartCoroutine(idle.Action(ActionCallBack));
                }
            case EnemyState.Damage:
                reserveState = EnemyState.MoveNear;
                currentState = EnemyState.Idle;
                return StartCoroutine(idle.Action(ActionCallBack, 0.5f));
            case EnemyState.MoveNear:
                if (nearPlayer.isNearPlayer)
                {
                    currentState = EnemyState.Attack;
                    return StartCoroutine(attack.Action(ActionCallBack));
                }
                else
                {
                    currentState = EnemyState.Idle;
                    return StartCoroutine(idle.Action(ActionCallBack));
                }
            default:
                return StartCoroutine(idle.Action(ActionCallBack));
        }
    }

    private void Update()
    {
        Action();
    }

    private void Action()
    {
        if (isAction) return;
        currentActionCoroutine = Think();
    }

    public void ActionCallBack()
    {
        isAction = false;
    }

    private void Death()
    {
        StopCoroutine(currentActionCoroutine);
        currentActionCoroutine = StartCoroutine(death.Action(DeadEnd));
        currentState = EnemyState.Death;
        isAction = true;
    }

    public void DeadEnd()
    {
        canUseHeal = true;
    }
}
