using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class DeathAction : MonoBehaviour, IEnemyActionable
{
    [SerializeField, Header("倒れるまでの時間")]
    private float layTime;
    private EnemyAnimation enemyAnimation;
    [SerializeField]
    private Transform enemyTransform;
    [SerializeField]
    private NavMeshAgent enemyNavMesh;

    private void Awake()
    {
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
    }

    public IEnumerator Action(UnityAction callBack)
    {
        //NavMeshを停止しないと起き上がるので停止させる
        enemyNavMesh.isStopped = true;
        enemyNavMesh.enabled = false;
        enemyAnimation.StopRunAnimation();
        yield return StartCoroutine(Lay(layTime));
        callBack.Invoke();
    }

    /// <summary>
    /// ゆっくり倒れる
    /// </summary>
    /// <param name="layTime"></param>
    /// <returns></returns>
    private IEnumerator Lay(float layTime)
    {
        enemyAnimation.StartDeathAnimation();
        yield return new WaitWhile(() => !enemyAnimation.IsEndAnimation(Mathf.Epsilon));
    }
}
