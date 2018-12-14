using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public abstract class ActionBase : MonoBehaviour {
	protected EnemyAnimation enemyAnimation;
	protected bool cancelFlag;

	protected virtual void Awake() {
		enemyAnimation = GetComponentInParent<EnemyAnimation>();
		Assert.IsNotNull(enemyAnimation, "EnemyAnimationが取得できませんでした。");
		cancelFlag = false;
	}

	protected virtual void Start() {
	}

	public abstract IEnumerator Action();

	/// <summary>
	/// 自分を保有するオブジェクトのTransformを取得する
	/// </summary>
	/// <returns></returns>
	protected Transform GetRootTransform() {
		Rigidbody rootRigid = GetComponentInParent<Rigidbody>();
		Assert.IsNotNull(rootRigid, "EnemyにRigidBodyがアタッチされていません。");
		return rootRigid.transform;
	}

	/// <summary>
	/// プレイヤーの取得
	/// </summary>
	/// <returns></returns>
	protected GameObject GetPlayer() {
		GameObject player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		Assert.IsNotNull(player, "Playerが取得できませんでした。");
		return player;
	}

	public virtual void Cancel() {
		cancelFlag = true;
	}
}
