using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AttackAction : MonoBehaviour, IEnemyActionable {
	[SerializeField]
	private EnemyAttack attack;

	public IEnumerator Action(UnityAction callBack) {
		yield return StartCoroutine(attack.Attack());
		callBack.Invoke();
	}

	public bool CanAttack(GameObject target) {
		return attack.CanAttack(target);
	}
}
