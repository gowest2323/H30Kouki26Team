using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Enemy/2FBoss")]
public class Boss2F : EnemyStatusScriptableObject {

	[SerializeField]
	private int _kiriagePower;
	[SerializeField]
	private int _comboPower;
	[SerializeField]
	private int _nagiharaiPower;

	public override int GetPower(EnemyAttackType type) {
		if (type == EnemyAttackType.Kiriage) {
			return _kiriagePower;
		} else if (type == EnemyAttackType.Combo) {
			return _comboPower;
		} else {
			return _nagiharaiPower;
		}
	}

}
