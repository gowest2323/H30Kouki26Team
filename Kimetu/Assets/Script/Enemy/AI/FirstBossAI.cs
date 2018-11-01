using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAI : EnemyAI, IDamageable
{
    private bool isAction; //行動中か？
    [SerializeField]
    private IdleAction idle;
    [SerializeField]
    private AttackAction swingAttack;
    [SerializeField]
    private AttackAction horizontalAttack;
    [SerializeField]
    private SearchAction search;
    [SerializeField]
    private NearPlayerAction nearPlayer;
    [SerializeField]
    private DamageAction damage;
    [SerializeField]
    private GameObject player;
    private EnemyStatus status;

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
        //現在の行動を停止
        StopCoroutine(currentActionCoroutine);
        //死亡したら倒れるモーション
        if (status.IsDead())
        {
            Debug.Log("死亡");
        }
        //まだ生きていたらダメージモーション
        else
        {
            currentState = EnemyState.Damage;
            //ダメージ状態に移行
            currentActionCoroutine = StartCoroutine(damage.Action(ActionCallBack));
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
        switch (currentState)
        {
            //待機の次はプレイヤーに接近
            case EnemyState.Idle:
                currentState = EnemyState.MoveNear;
                return StartCoroutine(nearPlayer.Action(ActionCallBack));
            //攻撃の次は待機
            case EnemyState.Attack:
                currentState = EnemyState.Idle;
                return StartCoroutine(idle.Action(ActionCallBack));
            //ダメージを受けた後は待機
            case EnemyState.Damage:
                currentState = EnemyState.Idle;
                return StartCoroutine(idle.Action(ActionCallBack));
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
                        return StartCoroutine(horizontalAttack.Action(ActionCallBack));
                    }
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
}
