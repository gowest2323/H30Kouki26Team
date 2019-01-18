using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Enemy/2FZako")]
public class Zako2F : EnemyStatusScriptableObject {
	[SerializeField]
	private int _kiriagePower;
	[SerializeField]
	private int _nagiharaiPower;

	public override int GetPower(EnemyAttackType type) {
		if (type == EnemyAttackType.Kiriage) {
			return _kiriagePower;
		} else {
			return _nagiharaiPower;
		}
	}
}
