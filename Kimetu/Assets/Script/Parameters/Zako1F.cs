using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Enemy/1FZako")]
public class Zako1F : EnemyStatusScriptableObject {
	[SerializeField]
	private int _hurioroshiPower;
	public int hurioroshiPower { get { return _hurioroshiPower; } }

	public override int GetPower(EnemyAttackType type) {
		return hurioroshiPower;
	}
}
