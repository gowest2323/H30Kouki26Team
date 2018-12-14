using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TackleAttack : EnemyAttack {
	[SerializeField]
	private float tackleSpeed;
	[SerializeField]
	private float chargeTime;
	private Rigidbody myRigid;
	private NavMeshAgent navMesh;

	protected override void Start() {
		base.Start();
		navMesh = GetComponentInParent<NavMeshAgent>();
		myRigid = GetComponentInParent<Rigidbody>();
	}

	public override IEnumerator Attack() {
		enemyAnimation.StartAttackAnimation(EnemyAttackType.Tackle);
		navMesh.isStopped = true;
		yield return new WaitForSeconds(chargeTime);

		while (enemyAnimation.IsEndAnimation(0.02f)) {
			Debug.Log("tackle");
			//myRigid.velocity = transform.forward * tackleSpeed * Slow.Instance.DeltaTime();
			yield return null;
		}
	}

	protected override void OnHit(Collider collider) {
		if (TagNameManager.Equals(collider.tag, TagName.Player)) {
			DamageSource damage = new DamageSource(collider.ClosestPoint(this.transform.position),
												   power, holderEnemyAI);
			collider.GetComponent<PlayerAction>().OnHit(damage);
		}
	}
}
