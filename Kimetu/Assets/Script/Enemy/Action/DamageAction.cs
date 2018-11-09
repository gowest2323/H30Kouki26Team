using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageAction : MonoBehaviour, IEnemyActionable
{
    private EnemyAnimation enemyAnimation;

    private void Start()
    {
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
    }

    public IEnumerator Action(UnityAction callBack)
    {
        yield return new WaitForSeconds(3.0f);
        callBack.Invoke();
    }
}
