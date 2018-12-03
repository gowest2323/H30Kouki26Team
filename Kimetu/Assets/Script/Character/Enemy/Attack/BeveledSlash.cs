using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeveledSlash : EnemyAttack {

	public override IEnumerator Attack() {
		//斜め切りのアニメーション
		enemyAnimation.StartAttackAnimation(EnemyAttackType.Beveled);
		yield return new WaitWhile(() => enemyAnimation.IsEndAnimation(0.02f));
	}

	protected override void OnHit(Collider collider) {
		if (TagNameManager.Equals(collider.tag, TagName.Player)) {
			DamageSource damage = new DamageSource(collider.ClosestPoint(this.transform.position),
												   power, holderEnemy);
			collider.GetComponent<PlayerAction>().OnHit(damage);
		}
	}
}
