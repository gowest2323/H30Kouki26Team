using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Enemy/3FZako")]

public class Zako3F : EnemyStatusScriptableObject {
	[SerializeField]
	private int _hurioroshiPower;
	[SerializeField]
	private int _kioniHurioroshiPower;
	public override int GetPower(EnemyAttackType type) {
		if (type == EnemyAttackType.KioniHurioroshi) {
			return _kioniHurioroshiPower;
		} else {
			return _hurioroshiPower;
		}
	}
}
