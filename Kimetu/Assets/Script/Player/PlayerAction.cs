using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction {
	private PlayerAnimation playerAnimation;
	private Transform transform;

	public PlayerAction(PlayerAnimation playerAnimation, Transform transform) {
		this.playerAnimation = playerAnimation;
		this.transform = transform;
	}

	public void Move(Vector3 dir) {
		//こうしないとコントローラのスティックがニュートラルに戻った時、
		//勝手に前を向いてしまう
		if(dir == Vector3.zero) {
			playerAnimation.StopRunAnimation();
			return;
		}
		playerAnimation.StartRunAnimation();
		var pos = transform.position;
		//transform.position += dir * 10 * Slow.Instance.playerDeltaTime;
		transform.position += dir * 10 * Time.deltaTime;
		transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
	}
}
