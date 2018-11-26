using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DamagePattern
{
    Normal,
    Countered,
}

public class DamageAction : MonoBehaviour, IEnemyActionable
{
    private EnemyAnimation enemyAnimation;

    private void Start()
    {
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
    }

    public IEnumerator Action(UnityAction callBack, DamagePattern damage)
    {
        Debug.Log("damage start");
        if (damage == DamagePattern.Normal)
        {
            if(Slow.Instance.isSlowNow) {
                EffectManager.Instance.EnemyDamageEffectCreate(gameObject);
                enemyAnimation.StartDamageAnimation();
                yield return new WaitWhile(() => !enemyAnimation.IsEndAnimation(0.02f));
            }
            EffectManager.Instance.EnemyDamageEffectCreate(gameObject);
        }
        else
        {
            
            enemyAnimation.StartReplAnimation();
            yield return new WaitWhile(() => !enemyAnimation.IsEndAnimation(0.02f));
        }
        yield return null;
        callBack.Invoke();
        Debug.Log("damage end");
    }

    public IEnumerator Action(UnityAction callBack)
    {
        yield return StartCoroutine(Action(callBack, DamagePattern.Normal));
    }
}
