using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction {
	private PlayerAnimation playerAnimation;
	private Transform transform;
	private PlayerState state;
	private bool isGuard;
	private float counterTime;
	private float counterOccuredTime;

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

	/// <summary>
	/// 攻撃を開始します。
	/// </summary>
	public void Attack() {
		playerAnimation.StartAttackAnimation();
	}

	/// <summary>
	/// ガードを開始します。
	/// </summary>
	public void GuardStart() {
		this.isGuard = true;
		this.state = PlayerState.Defence;
		//TODO:ここでガードモーションに入る
	}

	/// <summary>
	/// ガードを終了します。
	/// </summary>
	public void GuardEnd() {
		this.isGuard = false;
		this.state = PlayerState.Idle;
	}

	/// <summary>
	/// 回避行動を実行します。
	/// 数秒後に自動で回避状態は解除されます。
	/// </summary>
	public void Avoid() {
		this.state = PlayerState.Avoid;
		//TODO:コルーチンなどを開始する
		//TODO:回避行動中は他のアクションを実行できないように
	}

}
