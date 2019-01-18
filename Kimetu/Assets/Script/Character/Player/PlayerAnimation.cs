using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : CharacterAnimation {
	[SerializeField]
	private GameObject healEffectPrefab;

	// Use this for initialization

	public void StartRunAnimation() {
		//anim.SetFloat("Speed", speed);
		animator.SetBool("Run", true);
	}

	public void StopRunAnimation() {
		animator.SetBool("Run", false);
	}

	public void StartWalkAnimation() {
		animator.SetBool("Walk", true);
	}

	public void StopWalkAnimation() {
		animator.SetBool("Walk", false);
	}

	public void StartCounterAnimation() {
		animator.SetTrigger("Counter");
	}

	public void StartDamageAnimation() {
		animator.SetTrigger("Damage");
	}

	public void StartDeadAnimation() {
		animator.SetBool("Dead", true);
	}

	public void StopDeadAnimation() {
		animator.SetBool("Dead", false);
	}

	public void StartAttackAnimation(int phase) {
		switch (phase) {
			case 0:
				animator.SetTrigger("Attack");
				break;

			case 1:
				animator.SetTrigger("Attack2");
				break;

			case 2:
				animator.SetTrigger("Attack3");
				break;

			case 3:
				animator.SetTrigger("Attack4");
				break;

			default:
				throw new System.ArgumentException();
		}
	}
	public void CancelAttackAnimation() {
		CancelAttackAnimation(0);
		CancelAttackAnimation(1);
		CancelAttackAnimation(2);
		CancelAttackAnimation(3);
	}
	public void CancelAttackAnimation(int phase) {
		//FIXME:配列か何かにいれる、or列挙をキーとしたDictionaryとかにいれる
		switch (phase) {
			case 0:
				animator.ResetTrigger("Attack");
				break;

			case 1:
				animator.ResetTrigger("Attack2");
				break;

			case 2:
				animator.ResetTrigger("Attack3");
				break;

			case 3:
				animator.ResetTrigger("Attack4");
				break;

			default:
				throw new System.ArgumentException("phase " + phase);
		}
	}
	public void StartGuardAnimation() {
		//StopWalkAnimation();
		animator.SetBool("Guard", true);
	}

	public void StopGuardAnimation() {
		animator.SetBool("Guard", false);
		animator.SetBool("GuardWalk", false);
	}
	public void StartGuardWalkAnimation() {
		if(!animator.GetBool("Guard")) {
			StartGuardAnimation();
			StopWalkAnimation();
		}
		animator.SetBool("GuardWalk", true);
	}
	public void StopGuardWalkAnimation() {
		animator.SetBool("GuardWalk", false);
	}

	public void SetGuardSpeed(int speed) {
		animator.SetFloat("GuardSpeed", speed);
	}

	public void StartForwardAvoidAnimation() {
		animator.SetTrigger("ForwardAvoid");
	}

	public void StartRightAvoidAnimation() {
		animator.SetTrigger("RightAvoid");
	}

	public void StartLeftAvoidAnimation() {
		animator.SetTrigger("LeftAvoid");
	}

	public void StartBackAvoidAnimation() {
		animator.SetTrigger("BackAvoid");
	}

	public void StartKnockBackAnimation() {
		animator.SetTrigger("KnockBack");
	}

	public void StartKyuuseiAnimation() {
		animator.SetBool("Kyuusei", true);
	}

	public void StopKyuuseiAnimation() {
		animator.SetBool("Kyuusei", false);
	}

	public void StartKirinukeAnimation() {
		animator.SetBool("Kirinuke", true);
	}

	public void StopKirinukeAnimation() {
		animator.SetBool("Kirinuke", false);
	}

	public void StartRengekiAnimation() {
		animator.SetTrigger("Rengeki");
	}

	public void PlayKyuseiSE() {

		AudioManager.Instance.PlayEnemySE(AudioName.oni_aa_kyusei_06.String());
		var effect = GameObject.Instantiate(healEffectPrefab);
		AudioManager.Instance.PlayPlayerSE(AudioName.kyusei.String());
		effect.transform.position = transform.position;
		GameObject.Destroy(effect, 2f);
	}
}
