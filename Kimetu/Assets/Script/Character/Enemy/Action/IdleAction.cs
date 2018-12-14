using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : ActionBase {
	/// <summary>
	/// 待機する秒数
	/// </summary>
	public float waitSecond { get; set; }

	private void Awake() {
		Debug.Log("idle start");
		waitSecond = 1.0f;
	}

	protected override void Start() {
		base.Start();
		waitSecond = 1.0f;
	}

	public override IEnumerator Action() {
		cancelFlag = false;
		enemyAnimation.StopRunAnimation();
		float time = 0.0f;

		while (time < waitSecond) {
			if (cancelFlag) break;

			float slowDelta = Slow.Instance.DeltaTime();
			time += slowDelta;
			yield return new WaitForSeconds(slowDelta);
		}
	}
}
