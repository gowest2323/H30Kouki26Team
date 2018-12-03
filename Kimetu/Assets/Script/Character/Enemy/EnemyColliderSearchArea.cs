using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderSearchArea : EnemySearchableAreaBase {

	[SerializeField, Header("目の場所")]
	private Transform eyeTransform;
	[SerializeField]
	private Collider searchCollider;
	private bool isPlayerInArea; //プレイヤーがBoxの中にいるか

	// Use this for initialization
	void Start() {
		isPlayerInArea = false;
	}

	/// <summary>
	/// プレイヤーが範囲内にいるか？
	/// </summary>
	/// <param name="player"></param>
	/// <param name="area"></param>
	/// <returns></returns>
	public override bool IsPlayerInArea(GameObject player, bool toRayCast) {
		//範囲内にいなければ終了
		if (!isPlayerInArea) return false;

		if (!toRayCast) return true;

		//自分からプレイヤーに向かってレイを飛ばす
		Vector3 targetPosition = player.transform.position;
		Vector3 toTargetDir = (targetPosition - eyeTransform.position).normalized;

		//間に障害物がなければ範囲内にいる
		if (IsHitRayToPlayer(eyeTransform.position, toTargetDir, player, Mathf.Infinity)) {
			return true;
		}

		return false;
	}

	private void OnTriggerEnter(Collider other) {
		//プレイヤーが範囲内に入った
		if (other.tag == TagName.Player.String()) {
			isPlayerInArea = true;
		}
	}


	private void OnTriggerExit(Collider other) {
		//プレイヤーが範囲内から出た
		if (other.tag == TagName.Player.String()) {
			isPlayerInArea = false;
		}
	}
}
