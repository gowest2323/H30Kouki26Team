using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IdleAction : MonoBehaviour, IEnemyActionable
{
    [SerializeField]
    private float waitTime;
    private EnemyAnimation enemyAnimation;

    private void Start()
    {
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
    }

    /// <summary>
    /// 待機行動
    /// </summary>
    /// <returns></returns>
    public IEnumerator Action(UnityAction callBack)
    {
        Debug.Log("待機開始");
        yield return new WaitForSeconds(waitTime);
        callBack.Invoke();
        Debug.Log("待機終了");
    }
}
