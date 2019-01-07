using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Assertions;
using UnityEngine.AI;

public class ThunderAttack : EnemyAttack {
	[SerializeField]
	private GameObject thunderPrefab;
	[SerializeField]
	private int thunderNum;
	private List<Thunder> thunders;
	[SerializeField]
	private float extendTime;
	[SerializeField]
	private float thunderTime;
	[SerializeField]
	private Transform attackPosition;
	private Transform topTransform;
	[SerializeField]
	private float extendLength = 10;
	private bool waitHurioroshi;
	private System.IDisposable observer;
	private NavMeshAgent agent;
	[SerializeField]
	private ParticleSystem misdirectionParticle;
	private ParticleSystem[] misdirections;
	[SerializeField]
	private Collider hurioroshiAttackCollider;
	private EnemyThunderAttackAreaDrawer thunderAttackAreaDrawer;

	protected override void Start() {
		topTransform = GetTopTransform();
		agent = GetComponentInParent<NavMeshAgent>();
		isHit = false;
		waitHurioroshi = true;
		holderEnemyAI = GetComponentInParent<EnemyAI>();
		enemyAnimation = GetComponentInParent<EnemyAnimation>();
		Assert.IsNotNull(enemyAnimation, "EnemyAnimationが存在しません。");
		Assert.IsNotNull(holderEnemyAI, "この攻撃方法の持ち主が存在しません。");

		System.Type type = EnemyAttackTypeDictionary.typeDictionary[attackType];
		var attackHook = GetComponentInParent(type) as IEventHook;

		if (attackHook == null) {
			Debug.Log("type: " + type.Name);
		}

		this.observer = attackHook.trigger.Subscribe((e) => {
			if (e) { AttackStart(); }
			else { AttackEnd(); }
		});
		thunders = new List<Thunder>(thunderNum);

		for (int i = 0; i < thunderNum; i++) {
			GameObject thunderObj = Instantiate(thunderPrefab, this.transform);
			thunderObj.SetActive(false);
			Thunder thunder = thunderObj.GetComponent<Thunder>();
			Assert.IsNotNull(thunder, "Thunderがアタッチされていません。");
			thunders.Add(thunder);
		}

		misdirections = new ParticleSystem[2];

		misdirections[0] = Instantiate(misdirectionParticle) as ParticleSystem;
		misdirections[1] = Instantiate(misdirectionParticle) as ParticleSystem;
		thunderAttackAreaDrawer = attackAreaDrawer as EnemyThunderAttackAreaDrawer;
		Assert.IsNotNull(thunderAttackAreaDrawer, "EnemyThunderAttackAreaDrawerが割り当てられていません。");
	}

	public override IEnumerator Attack() {
		waitHurioroshi = true;
		float rotate = 180.0f / (thunders.Count + 1);
		float[] directions = new float[thunders.Count];

		for (int i = 0; i < directions.Length; i++) {
			directions[i] = rotate * (i + 1);
		}

		thunderAttackAreaDrawer.rayCastPosition = attackPosition.position;
		thunderAttackAreaDrawer.DrawStart();
		misdirections[0].transform.position = this.transform.position;
		misdirections[0].Play();
		misdirections[1].transform.position = attackPosition.position;
		misdirections[1].Play();
		yield return new WaitForSeconds(2.0f);
		misdirections[0].Stop();
		misdirections[1].Stop();
		topTransform.position = attackPosition.position;
		topTransform.rotation = attackPosition.rotation;
		//NavMeshの座標の更新
		agent.Warp(topTransform.position);
		enemyAnimation.StartAttackAnimation(attackType);
		//アニメーション終了まで待機
		yield return WaitForce();

		while (waitHurioroshi && !cancelFlag) {
            Debug.Log("wait hurioroshi");
			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}


		for (int i = 0; i < thunders.Count; i++) {
			thunders[i].gameObject.SetActive(true);
			thunders[i].Init(power);
			float y = (rotate * (i + 1)) - 90.0f;
			RotateThunder(thunders[i], y);
		}

		float time = 0.0f;

		//終了フラグが立つか時間が経過するまで
		while (!cancelFlag && time < thunderTime) {
			float t = Mathf.Min(time / extendTime, 1);
			float len = Mathf.Lerp(0, extendLength, t);
			thunders.ForEach(thunder => thunder.UpdateThunder(len));
			time += Slow.Instance.DeltaTime();
            Debug.Log("thunder");
			yield return null;
		}

		thunderAttackAreaDrawer.DrawEnd();
		thunders.ForEach(t => t.gameObject.SetActive(false));
		//雷攻撃終了
		enemyAnimation.anim.SetTrigger("ThunderEnd");
	}

	public override void AttackStart() {
		isRunning = true;
		isHit = false;
		hurioroshiAttackCollider.enabled = true;
	}

	public override void AttackEnd() {
		isRunning = false;
		waitHurioroshi = false;
		hurioroshiAttackCollider.enabled = false;
        waitHurioroshi = false;
        Debug.Log("attack end");
	}

	private void RotateThunder(Thunder thunder, float rotateY) {
		Quaternion rot = thunder.transform.rotation;
		rot.eulerAngles = rot.eulerAngles + new Vector3(0, rotateY, 0);
		thunder.transform.rotation = rot;
	}

	protected override void OnHit(Collider collider) {
		if (isHit) return;

		if (TagNameManager.Equals(collider.tag, TagName.Player)) {
			DamageSource damage = new DamageSource(collider.ClosestPoint(this.transform.position),
												   power, holderEnemyAI);
			collider.GetComponent<PlayerAction>().OnHit(damage);
		}
	}

	public override void Cancel() {
		cancelFlag = true;
		isRunning = false;
		hurioroshiAttackCollider.enabled = false;
	}
}