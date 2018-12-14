using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamagePattern {
	Normal,
	Countered,
}
public class DamageAction : ActionBase {
	public DamagePattern damagePattern { get; set; }
	[SerializeField]
	private string damageStateName = "hit";
	[SerializeField]
	private string replStateName = "repl";

	protected override void Start() {
		base.Start();
		damagePattern = DamagePattern.Normal;
	}

	public override IEnumerator Action() {
		cancelFlag = false;

		if (damagePattern == DamagePattern.Normal) {
			if (Slow.Instance.isSlowNow) {
				enemyAnimation.StartDamageAnimation();
			}

			yield return WaitStartAttackAnimation(damageStateName);
			yield return WaitEndAttackAnimation();
		}

		//はじかれたときの処理
		else {
			enemyAnimation.StartReplAnimation();
			yield return WaitStartAttackAnimation(replStateName);
			yield return WaitEndAttackAnimation();
		}
	}

	/// <summary>
	/// アニメーションの再生を待機する
	/// </summary>
	/// <param name="stateName"></param>
	/// <returns></returns>
	protected IEnumerator WaitStartAttackAnimation(string stateName) {
		while (!enemyAnimation.IsPlayingAnimation("oni", stateName)) {
			if (cancelFlag) break;

			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}
	}

	/// <summary>
	/// アニメーションの終了を待機する
	/// </summary>
	/// <returns></returns>
	protected IEnumerator WaitEndAttackAnimation() {
		while (!enemyAnimation.IsEndAnimation(0.1f)) {
			if (cancelFlag) break;

			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}
	}
}
