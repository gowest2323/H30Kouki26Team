using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Enemy/3FZako")]

public class Zako3F : EnemyStatusScriptableObject {
	[SerializeField]
	private int _hurioroshiPower;
	public override int GetPower(EnemyAttackType type) {
		return _hurioroshiPower;
	}
}
