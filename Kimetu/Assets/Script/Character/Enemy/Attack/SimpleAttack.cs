using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SimpleAttack : EnemyAttack, IAttackEventHandler {
	[SerializeField, Tooltip("攻撃方法")]
	private EnemyAttackType attackType;
	private System.IDisposable observer;

	protected override void Start() {
		base.Start();
		System.Type type = EnemyAttackTypeDictionary.typeDictionary[attackType];
		var attackHook = GetComponentInParent(type) as IEventHook;

		if (attackHook == null) {
			Debug.Log("type: " + type.Name);
		}

		this.observer = attackHook.trigger.Subscribe((e) => {
			if (e) { AttackStart(); }
			else { AttackEnd(); }
		});
	}

	public override IEnumerator Attack() {
		cancelFlag = false;
		//攻撃範囲の描画
		DrawStartAttackArea();
		enemyAnimation.StartAttackAnimation(attackType);
		yield return new WaitForSeconds(1.0f);
		//攻撃アニメーション開始まで待機
		Debug.Log("攻撃アニメーション開始");
		yield return WaitStartAttackAnimation();
		//攻撃アニメーション終了まで待機
		yield return WaitEndAttackAnimation();
		//攻撃範囲描画終了
		DrawEndAttackArea();
	}
}
