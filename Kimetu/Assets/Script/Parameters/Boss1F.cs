using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Enemy/1FBoss")]
public class Boss1F : EnemyStatusScriptableObject {
	[SerializeField]
	private int _hurioroshiPower;
	[SerializeField]
	private int _nagiharaiPower;
	public override int GetPower(EnemyAttackType type) {
		if (type == EnemyAttackType.SwingDown) {
			return _hurioroshiPower;
		} else {
			return _nagiharaiPower;
		}
	}
}
