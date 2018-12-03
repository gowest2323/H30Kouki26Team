using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class MoveAction : MonoBehaviour, IEnemyActionable
{
    /// <summary>
    /// 移動する座標
    /// </summary>
    protected Vector3 movePosition { get; set; }
    /// <summary>
    /// 回転する角度
    /// この角度まで回転する
    /// </summary>
    protected Quaternion moveRotation { get; set; }
    /// <summary>
    /// 移動中にプレイヤーを見つけたか
    /// </summary>
    public bool isDetectPlayer { private set; get; }

    protected EnemyAnimation enemyAnimation;
    protected NavMeshAgent agent;
    [SerializeField, Header("目的地に近づいたと判断する距離")]
    protected float remainingDistance = 0.7f;
    [SerializeField, Header("目的角に回転したと判断する大きさ")]
    protected float remainingRotate = 1.0f;
    [SerializeField,Header("目的角に回転するのにかける時間")]
    protected float rotateSecond = 1.0f;
    [SerializeField, Header("敵の視界")]
    private EnemySearchableAreaBase searchArea;
    private GameObject player;
    private Transform topTransform; //Enemyの一番上のTransform

    protected virtual void Start()
    {
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
        agent = GetComponentInParent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(TagName.Player.String());
        topTransform = GetTopTransform();
        Assert.IsNotNull(enemyAnimation, "EnemyAnimationが存在しません。");
        Assert.IsNotNull(agent, "NavMeshAgentが存在しません。");
        Assert.IsNotNull(player, "Playerが存在しません。");
    }

    /// <summary>
    /// 行動する
    /// </summary>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public virtual IEnumerator Action(UnityAction callBack)
    {
        enemyAnimation.StartRunAnimation();
        agent.SetDestination(movePosition);
        agent.isStopped = false;
        isDetectPlayer = false;
        //目的地に戻るまで
        while (!IsMoveEndCondition())
        {
            //移動中にプレイヤーを見つけたら終了する
            if (IsDetectPlayer())
            {
                isDetectPlayer = true;
                Finish(callBack);
                yield break;
            }
            yield return new WaitForSeconds(Slow.Instance.DeltaTime());
        }
        enemyAnimation.StopRunAnimation();
        agent.isStopped = true;
        //角度が違っていたら回転する
        float time = 0.0f;
        Quaternion beforeRotation = topTransform.rotation;
        while (!IsRotateEndCondition())
        {
            //回転中にプレイヤーを見つけたら終了する
            if (IsDetectPlayer())
            {
                isDetectPlayer = true;
                Finish(callBack);
                yield break;
            }
            time += Slow.Instance.DeltaTime();
            topTransform.rotation = Quaternion.Slerp(beforeRotation, moveRotation, (time / rotateSecond));
            yield return new WaitForSeconds(Slow.Instance.DeltaTime());
        }
        Finish(callBack);
    }

    /// <summary>
    /// 終了時処理
    /// </summary>
    /// <param name="callBack">終了時に呼ぶコールバック</param>
    private void Finish(UnityAction callBack)
    {
        enemyAnimation.StopRunAnimation();
        agent.isStopped = true;
        callBack.Invoke();
    }

    /// <summary>
    /// プレイヤーを見つけたか
    /// </summary>
    /// <returns></returns>
    private bool IsDetectPlayer()
    {
        return searchArea.IsPlayerInArea(player, true);
    }

    /// <summary>
    /// 移動終了条件を満たしたか
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsMoveEndCondition()
    {
        return (agent.remainingDistance < remainingDistance);
    }
    
    /// <summary>
    /// 回転終了条件を満たしたか
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsRotateEndCondition()
    {
        return Quaternion.Angle(moveRotation, topTransform.rotation) < remainingRotate;
    }

    /// <summary>
    /// Enemyの一番上のTransformの取得
    /// </summary>
    /// <returns></returns>
    private Transform GetTopTransform()
    {
        //1番上のEnemyにはRigidBodyがついているためそのTransformを利用
        return GetComponentInParent<Rigidbody>().transform;
    }
}
