using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class EnemyAttack : MonoBehaviour, IAttackEventHandler {
	[SerializeField, Tooltip("攻撃力")]
	protected int power;
	protected EnemyAI holderEnemyAI; //この攻撃方法の持ち主
	[SerializeField, Header("攻撃をするかどうか判断する範囲")]
	protected EnemySearchableAreaBase attackArea;
	[SerializeField, Tooltip("攻撃範囲描画オブジェクト")]
	protected EnemyAttackAreaDrawer attackAreaDrawer;
	[SerializeField]
	protected string attackStateName;
    [SerializeField]
    protected EnemyAttackType attackType;
	protected Collider attackCollider;
	protected EnemyAnimation enemyAnimation;
	protected bool isHit; //攻撃が当たったかどうか（多段ヒット無効のため）
	protected bool isRunning; //現在この攻撃方法が開始されているか（別攻撃での判定を無効にするため）
	protected bool cancelFlag;

	protected virtual void Start() {
		isHit = false;
		holderEnemyAI = GetComponentInParent<EnemyAI>();
		attackCollider = GetComponent<Collider>();
		attackCollider.enabled = false;
		enemyAnimation = GetComponentInParent<EnemyAnimation>();
		Assert.IsNotNull(enemyAnimation, "EnemyAnimationが存在しません。");
		Assert.IsNotNull(holderEnemyAI, "この攻撃方法の持ち主が存在しません。");
		Assert.IsNotNull(attackArea, "EnemySearchAreaがありません。");
		Assert.IsNotNull(attackAreaDrawer, "EnemyAttackAreaDrawerがありません。");
	}

	public abstract IEnumerator Attack();

	public bool CanAttack(GameObject target) {
		return attackArea.IsPlayerInArea(target, true);
	}

	private void OnTriggerEnter(Collider other) {
		if (isRunning && !isHit) {
			OnHit(other);
		}
	}

	protected virtual void OnHit(Collider collider) {
		if (collider.tag == TagName.Player.String()) {
			isHit = true;
			DamageSource damage = new DamageSource(collider.ClosestPoint(this.transform.position),
												   power, holderEnemyAI);
			PlayerAction player = collider.GetComponent<PlayerAction>();
			Assert.IsNotNull(player, "PlayerActionが取得できませんでした。");
			player.OnHit(damage);
		}
	}

	public virtual void AttackStart() {
		isRunning = true;
		isHit = false;
		attackCollider.enabled = true;
        Debug.Log("attack start");
	}

	public virtual void AttackEnd() {
		isRunning = false;
		attackCollider.enabled = false;
	}

	public virtual void Cancel() {
		cancelFlag = true;
		isRunning = false;
		attackCollider.enabled = false;
	}

	protected GameObject GetPlayer() {
		GameObject player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		Assert.IsNotNull(player, "プレイヤーが取得できませんでした。");
		return player;
	}

	protected Transform GetTopTransform() {
		Rigidbody topRigid = GetComponentInParent<Rigidbody>();
		Assert.IsNotNull(topRigid, "Rigidbodyが見つかりませんでした。");
		return topRigid.transform;
	}

	protected void DrawStartAttackArea() {
		if (attackAreaDrawer != null) {
			attackAreaDrawer.DrawStart();
		}
	}
	protected void DrawEndAttackArea() {
		if (attackAreaDrawer != null) {
			attackAreaDrawer.DrawEnd();
		}
	}

	protected IEnumerator WaitStartAttackAnimation() {
		while (!enemyAnimation.IsPlayingAnimation("oni", attackStateName)) {
			if (cancelFlag) break;

			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}
	}

	protected IEnumerator WaitEndAttackAnimation() {
		while (!enemyAnimation.IsEndAnimation(0.1f)) {
			if (cancelFlag) break;

			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}
	}

	/// <summary>
	/// 強制的に待機する時間
	/// </summary>
	/// <returns></returns>
	protected float GetForcedWaitTime() {
		return 0.25f / Slow.Instance.GetCurrentOtherSpeed();
	}

	protected virtual IEnumerator WaitForce() {
		float time = 0.0f;

		while (!cancelFlag && time < GetForcedWaitTime()) {
			time += Slow.Instance.DeltaTime();
			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}
	}
	public AnimationClip FindAnimationClip(string animationClipName) {
		foreach (var clip in enemyAnimation.anim.runtimeAnimatorController.animationClips) {
			if (clip.name == animationClipName) {
				return clip;
			}
		}

		Debug.LogError(animationClipName + "が存在しません");
		return null;
	}

}
