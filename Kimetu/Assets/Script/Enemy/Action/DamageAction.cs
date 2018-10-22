using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageAction : MonoBehaviour, IEnemyActionable
{
    public IEnumerator Action(UnityAction callBack)
    {
        Debug.Log("ダメージを受けた");
        yield return new WaitForSeconds(3.0f);
        Debug.Log("ダメージ状態回復");
        callBack.Invoke();
    }
}
