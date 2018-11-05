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
    private void Start()
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
        float time = 0.0f;
        Quaternion before = enemyTransform.rotation;
        Quaternion after = Quaternion.Euler(-90.0f, before.y, before.z);
        while (time < layTime)
        {
            float t = (time / layTime);
            enemyTransform.rotation = Quaternion.Lerp(before, after, t);
            time += Slow.Instance.DeltaTime();
            yield return null;
        }
        enemyTransform.rotation = after;
    }
}
