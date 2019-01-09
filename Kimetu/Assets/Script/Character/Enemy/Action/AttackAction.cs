using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : ActionBase {
	[SerializeField]
	private EnemyAttack enemyAttack;
	private GameObject player;
	private bool isSetuna;

	protected override void Start() {
		base.Start();
		player = GetPlayer();
		this.isSetuna = GetComponentInParent<LastBossAI>();
	}

	public override IEnumerator Action() {
		if(!isSetuna && !SceneChanger.Instance().isChanging) {
			AudioManager.Instance.PlayEnemySE(AudioName.oni_oaa_preAttack_03.String());
			yield return new WaitForSeconds(1f);
		}
		yield return enemyAttack.Attack();
	}

	public bool CanAttack() {
		return enemyAttack.CanAttack(player);
	}

	public override void Cancel() {
		enemyAttack.Cancel();
	}
}
