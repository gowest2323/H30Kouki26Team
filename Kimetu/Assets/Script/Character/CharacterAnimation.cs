﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class CharacterAnimation : MonoBehaviour {
	protected Animator animator;

	/// <summary>
	/// アニメーションの速度を 0-1 の範囲で設定します。
	/// </summary>
	/// <value></value>
	public float speed {
		set {
			animator.speed = value;
		}

		get {
			return animator.speed;
		}
	}

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	protected virtual void Awa() {
		animator = GetComponent<Animator>();
		speed = 1.0f;
	}

	/// <summary>
	/// アニメーションが終了したか？
	/// </summary>
	/// <param name="epsilon">誤差</param>
	/// <param name="layerNo">判定するアニメーションのレイヤー番号</param>
	/// <returns></returns>
	public bool IsEndAnimation(float epsilon, int layerNo = 0) {
		AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(layerNo);
		return animatorInfo.normalizedTime > 1.0f - epsilon;
	}

	public bool IsPlayingAnimation(string characterName, string animationName) {
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		return info.fullPathHash == Animator.StringToHash("Base Layer." + characterName + "@" + animationName);
	}

	/// <summary>
	/// アニメーションを待機します。
	/// </summary>
	/// <param name="characterName"></param>
	/// <param name="animationName">Animatorないのステートの名前</param>
	/// <returns></returns>
	public IEnumerator WaitAnimation(string characterName, string animationName) {
		AnimatorStateInfo info;
		yield return new WaitWhile(() => {
			//現在のアニメーション情報
			info = animator.GetCurrentAnimatorStateInfo(0);
			//今再生中のアニメーション名がattackAnimationNameでない間待機
			return info.fullPathHash != Animator.StringToHash("Base Layer." + characterName + "@" + animationName);
		});
		info = animator.GetCurrentAnimatorStateInfo(0);
		//アニメーションが終了するまで待機
		yield return new WaitWhile(() => {
			info = animator.GetCurrentAnimatorStateInfo(0);
			//アニメーション名が変わっている場合があるのでそれを調べる
			return !IsEndAnimation(0.02f) && info.fullPathHash == Animator.StringToHash("Base Layer." + characterName + "@" + animationName);
		});
	}
}
