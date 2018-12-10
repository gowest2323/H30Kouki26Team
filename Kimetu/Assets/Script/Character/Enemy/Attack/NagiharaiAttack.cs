using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class NagiharaiAttack : EnemyAttack, IAttackEventHandler {
	[SerializeField, Tooltip("薙ぎ払いのアニメーション名(oni@〇〇")]
	private string nagiharaiAttackAnimationName = "nagi";
	private Transform playerTransform;
	private float attackTime;
	private Animator anim;
	private Transform topTransform;
	private System.IDisposable observer;

	protected override void Start() {
		base.Start();
		System.Type type = EnemyAttackTypeDictionary.typeDictionary[EnemyAttackType.NagiharaiAttack];
		var attackHook = GetComponentInParent(type) as IEventHook;

		if (attackHook == null) {
			Debug.Log("type: " + type.Name);
		}

		this.observer = attackHook.trigger.Subscribe((e) => {
			if (e) { AttackStart(); }
			else { AttackEnd(); }
		});
		anim = enemyAnimation.anim;
		topTransform = GetTopTransform();
		playerTransform = GameObject.FindGameObjectWithTag(TagName.Player.String()).transform;
	}

	public override IEnumerator Attack() {
		if (areaDrawer != null) {
			areaDrawer.DrawStart();
		}

		enemyAnimation.StartAttackAnimation(EnemyAttackType.NagiharaiAttack);
		StartCoroutine(Rotate());
		yield return enemyAnimation.WaitAnimation("oni", nagiharaiAttackAnimationName);

		if (areaDrawer != null) {
			areaDrawer.DrawEnd();
		}
	}
	private void OnDestroy() {
		//イベントの購読を終了
		observer.Dispose();
	}

	private IEnumerator Rotate() {
		yield return WaitStartNagiharaiAttack();
		float rotateTime = RotationTime();
		float time = 0.0f;

		Quaternion before = topTransform.rotation;
		Vector3 aim = playerTransform.position - topTransform.position;
		Quaternion after = Quaternion.LookRotation(aim, Vector3.up);

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

	private IEnumerator WaitStartNagiharaiAttack() {
		AnimatorStateInfo info;
		yield return new WaitWhile(() => {
			//現在のアニメーション情報
			info = anim.GetCurrentAnimatorStateInfo(0);
			//今再生中のアニメーション名がattackAnimationNameでない間待機
			return info.fullPathHash != Animator.StringToHash("Base Layer.oni@" + nagiharaiAttackAnimationName);
		});
	}

	protected override void OnHit(Collider collider) {
		if (hitCount > 0) {
			return;
		}

		if (TagNameManager.Equals(collider.tag, TagName.Player)) {
			hitCount++;
			DamageSource damage = new DamageSource(collider.ClosestPoint(this.transform.position),
												   power, holderEnemy);
			collider.GetComponent<PlayerAction>().OnHit(damage);
		}
	}

	/// <summary>
	/// Enemyの一番上のTransformの取得
	/// </summary>
	/// <returns></returns>
	private Transform GetTopTransform() {
		//1番上のEnemyにはRigidBodyがついているためそのTransformを利用
		return GetComponentInParent<Rigidbody>().transform;
	}

	/// <summary>
	/// 回転にかける時間を取得
	/// </summary>
	/// <returns></returns>
	private float RotationTime() {
		AnimatorClipInfo info = anim.GetCurrentAnimatorClipInfo(0)[0];
		AnimationClip clip = info.clip;
		float startTime = clip.events[0].time;
		float endTime = clip.events[1].time;
		float rotateTime = endTime - startTime;
		return rotateTime;
	}
}
