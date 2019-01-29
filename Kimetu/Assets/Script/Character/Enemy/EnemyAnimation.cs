using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyAnimation : CharacterAnimation {
	public Animator anim { get { return animator; } }

	[SerializeField]
	private AudioSource audioSource;

	private bool pauseVoice;

	public enum Voice {
		SetunaRen2Attack01,
		SetunaRen2Attack02,
		SetunaRen3Attack01,
		SetunaRen3Attack02,
		SetunaRen3Attack03
	}


	public void StartRunAnimation() {
		animator.SetBool("Run", true);
	}

	public void StartDeathAnimation() {
		animator.SetBool("Dead", true);
	}

	public void StopRunAnimation() {
		animator.SetBool("Run", false);
	}

	/// <summary>
	/// 攻撃アニメーションの開始
	/// </summary>
	/// <param name="attackType">攻撃の種類</param>
	public void StartAttackAnimation(EnemyAttackType attackType) {
		Assert.IsTrue(EnemyAttackTypeDictionary.dictionary.ContainsKey(attackType), attackType + "が定義されていません");
		//パラメータ名を取得
		string parameterName = EnemyAttackTypeDictionary.dictionary[attackType];
		Assert.IsTrue(Array.Exists(animator.parameters, param => param.name == parameterName), parameterName + "が存在しません");
		animator.SetTrigger(parameterName);
	}

	/// <summary>
	/// ダメージを受けた時のアニメーション再生
	/// </summary>
	public void StartDamageAnimation() {
		animator.SetTrigger("Damage");
	}

	/// <summary>
	/// はじかれたときのアニメーション再生
	/// </summary>
	public void StartReplAnimation() {
		animator.SetTrigger("Repl");
	}

	public void PlayEnemyVoice(Voice v) {
		switch (v) {
			case Voice.SetunaRen2Attack01:
				PlayVoice(AudioManager.Instance.GetSE(AudioName.setuna_ha_niren_01.String()));
				break;

			case Voice.SetunaRen2Attack02:
				PlayVoice(AudioManager.Instance.GetSE(AudioName.setuna_ha_niren_01.String()));
				break;

			case Voice.SetunaRen3Attack01:
				PlayVoice(AudioManager.Instance.GetSE(AudioName.setuna_hu_sanren_02.String()));
				break;

			case Voice.SetunaRen3Attack02:
				PlayVoice(AudioManager.Instance.GetSE(AudioName.setuna_hu_sanren_02.String()));
				break;

			case Voice.SetunaRen3Attack03:
				PlayVoice(AudioManager.Instance.GetSE(AudioName.setuna_hu_sanren_02.String()));
				break;
		}
	}

	private void PlayVoice(AudioClip clip) {
		if (pauseVoice) {
			audioSource.UnPause();
			pauseVoice = false;
		} else {
			audioSource.clip = clip;
			audioSource.Play();
		}
	}

	public void PauseEnemyVoice() {
		audioSource.Pause();
		this.pauseVoice = true;
	}
}
