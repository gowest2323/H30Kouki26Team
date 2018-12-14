using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ComboAttack : EnemyAttack, IAttackEventHandler {
	private Animator anim;
	private Transform topTransform;
	[SerializeField]
	private float rotateTime;
	private System.IDisposable observer;

	protected override void Start() {
		base.Start();
		topTransform = GetTopTransform();
		anim = enemyAnimation.anim;
		System.Type type = EnemyAttackTypeDictionary.typeDictionary[EnemyAttackType.Combo];
		var attackHook = GetComponentInParent(type) as IEventHook;

		if (attackHook == null) {
			Debug.Log("type: " + type.Name);
		}

		this.observer = attackHook.trigger.Subscribe((e) => {
			if (e) { AttackStart(); }
			else { AttackEnd(); }
		});
	}

	private void OnDestroy() {
		//イベントの購読を終了
		observer.Dispose();
	}

	public override IEnumerator Attack() {
		cancelFlag = false;
		//攻撃範囲描画
		DrawStartAttackArea();
		yield return Rotate();
        yield return new WaitForSeconds(GetForcedWaitTime());
		enemyAnimation.StartAttackAnimation(EnemyAttackType.Combo);
		yield return WaitStartAttackAnimation();
		//攻撃アニメーション終了まで待機
		yield return WaitEndAttackAnimation();
		DrawEndAttackArea();
	}

	private IEnumerator Rotate() {
		float time = 0.0f;

		Quaternion before = topTransform.rotation;

		Vector3 temp = before.eulerAngles;
		temp.y += 180.0f;
		Quaternion after = Quaternion.Euler(temp);

		while (time < rotateTime) {
			float slowDelta = Slow.Instance.DeltaTime();
			time += slowDelta;
			float t = time / rotateTime;

			Quaternion rotate = Quaternion.Lerp(before, after, t);
			topTransform.rotation = rotate;
			yield return new WaitForSeconds(slowDelta);
		}

		topTransform.rotation = after;
	}
}
