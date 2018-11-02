﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NearPlayerAction : MonoBehaviour, IEnemyActionable
{
    [SerializeField]
    private EnemySearchableAreaBase attackableArea;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject playerObj;
    public bool isNearPlayer { private set; get; }
    private EnemyAnimation enemyAnimation;

    private void Start()
    {
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
    }

    public IEnumerator Action(UnityAction callBack)
    {
        isNearPlayer = false;
        agent.isStopped = false;
        enemyAnimation.StartRunAnimation();
        Debug.Log("接近開始");

        while (!attackableArea.IsPlayerInArea(playerObj, true))
        {
            Debug.Log("接近中");
            agent.SetDestination(playerObj.transform.position);
            yield return null;
        }
        Debug.Log("接近成功");
        isNearPlayer = true;
        agent.isStopped = true;
        enemyAnimation.StopRunAnimation();
        callBack.Invoke();
    }
}
