using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSlash : EnemyAttack {
	private Rigidbody myRigid;

	protected override void Start() {
		base.Start();
		myRigid = GetComponentInParent<Rigidbody>();
	}

	public override IEnumerator Attack() {
		enemyAnimation.StartAttackAnimation(EnemyAttackType.AirSlash);
		Debug.Log("airslash");

		while (enemyAnimation.IsEndAnimation(0.02f)) {
			yield return null;
		}
	}

	protected override void OnHit(Collider collider) {
		if (TagNameManager.Equals(collider.tag, TagName.Player)) {
			DamageSource damage = new DamageSource(collider.ClosestPoint(this.transform.position),
												   power, holderEnemy);
			collider.GetComponent<PlayerAction>().OnHit(damage);
		}
	}
}