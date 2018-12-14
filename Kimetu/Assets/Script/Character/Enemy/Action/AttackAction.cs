using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : ActionBase {
	[SerializeField]
	private EnemyAttack enemyAttack;
	private GameObject player;

	protected override void Start() {
		base.Start();
		player = GetPlayer();
	}

	public override IEnumerator Action() {
		yield return enemyAttack.Attack();
	}

	public bool CanAttack() {
		return enemyAttack.CanAttack(player);
	}

	public override void Cancel() {
		enemyAttack.Cancel();
	}
}
