using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵の単純な攻撃（コライダーのオンオフで表現できる）
/// </summary>
public class SimpleAttack : EnemyAttack, IAttackEventHandler {
	[SerializeField, Header("攻撃の種類")]
	private EnemyAttackType attackType;
	[SerializeField, Header("攻撃のアニメーションの名前(oni@〇〇)")]
	private string attackAnimationName = "attack";
	private System.IDisposable observer;

	protected override void Start() {
		base.Start();
		System.Type type = EnemyAttackTypeDictionary.typeDictionary[attackType];
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
		if (areaDrawer != null) {
			areaDrawer.DrawStart();
		}

		enemyAnimation.StartAttackAnimation(attackType);
		yield return enemyAnimation.WaitAnimation("oni", attackAnimationName);

		if (areaDrawer != null) {
			areaDrawer.DrawEnd();
		}
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
}
