using System;
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
    private GameObject playerObj;
    public bool isNearPlayer { private set; get; }
    private EnemyAnimation enemyAnimation;

    private void Awake()
    {
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
        playerObj = GameObject.FindGameObjectWithTag(TagName.Player.String());
    }

    private void Start()
    {
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
    }

    public IEnumerator Action(UnityAction callBack)
    {
        isNearPlayer = false;
        agent.isStopped = false;
        enemyAnimation.StartRunAnimation();

        while (!attackableArea.IsPlayerInArea(playerObj, true))
        {
            agent.SetDestination(playerObj.transform.position);
            yield return null;
        }
        isNearPlayer = true;
        agent.isStopped = true;
        enemyAnimation.StopRunAnimation();
        callBack.Invoke();
    }
}
