using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SearchAction : MonoBehaviour, IEnemyActionable
{
    [SerializeField]
    private EnemyAttackableArea attackableArea;
    private GameObject player;
    public bool canSearched { private set; get; }
    private EnemyAnimation enemyAnimation;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagName.Player.String());
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
    }

    /// <summary>
    /// 索敵
    /// </summary>
    /// <returns>索敵範囲内にプレイヤーがいたらtrue</returns>
    public IEnumerator Action(UnityAction callBack)
    {
        Debug.Log("索敵");
        yield return null;
        canSearched = attackableArea.IsPlayerInArea(player, true);
        callBack.Invoke();
    }
}
