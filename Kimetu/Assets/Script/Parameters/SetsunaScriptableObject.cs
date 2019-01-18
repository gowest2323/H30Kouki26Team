using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Enemy/Setsuna")]
public class SetsunaScriptableObject : EnemyStatusScriptableObject {
	[SerializeField]
	private int _comboPower;
	[SerializeField]
	private int _nagiharaiPower;
	[SerializeField]
	private int _thunderPower;

	public override int GetPower(EnemyAttackType type) {
		if (type == EnemyAttackType.SetunaCombo) {
			return _comboPower;
		} else if (type == EnemyAttackType.SetsunaNagiharai) {
			return _nagiharaiPower;
		} else {
			return _thunderPower;
		}
	}
}
