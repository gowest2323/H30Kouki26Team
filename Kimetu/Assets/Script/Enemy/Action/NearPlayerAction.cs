using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NearPlayerAction : MonoBehaviour, IEnemyActionable
{
    [SerializeField]
    private EnemyAttackableArea attackableArea;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject playerObj;
    public bool isNearPlayer { private set; get; }

    public IEnumerator Action(UnityAction callBack)
    {
        Debug.Log("接近開始");

        while (!attackableArea.IsPlayerInArea(playerObj, EnemyAttackableArea.Area.Attackable))
        {
            Debug.Log("接近中");
            agent.SetDestination(playerObj.transform.position);
            yield return null;
        }
        Debug.Log("接近成功");
        isNearPlayer = true;
        callBack.Invoke();
    }
}
