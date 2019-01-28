using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class WimpChasePlayer : ActionBase {
	[SerializeField, Tooltip("十分近づいたと判断する範囲")]
	private EnemySearchableAreaBase judgeArea;
	[SerializeField, Header("視界")]
	private EnemySearchableAreaBase fieldOfVision;
	[SerializeField, Header("プレイヤーを見失った時この時間経過したら追跡をあきらめる")]
	private float giveUpTime;
	[SerializeField, Header("プレイヤーの方向を向くかどうか")]
	private bool toRotate;
	[SerializeField]
	private float rotateSpeed;
	private NavMeshAgent agent;
	private GameObject player;
	private Transform rootTransform;
	/// <summary>
	/// 十分近づいたか
	/// </summary>
	public bool isNearPlayer { get; private set; }

	protected override void Start() {
		base.Start();
		rootTransform = GetRootTransform();
		agent = rootTransform.GetComponent<NavMeshAgent>();
		Assert.IsNotNull(agent, "NavMeshが取得できませんでした。");
		player = GetPlayer();
		isNearPlayer = false;
	}

	public override IEnumerator Action() {
		cancelFlag = false;
		//NavMeshを動かす
		NavMeshStart();
		isNearPlayer = false;
		enemyAnimation.StartRunAnimation();
		float time = 0.0f;

		while (!cancelFlag) {
			UpdateNavMesh(player.transform.position);

			if (IsEndChase()) break;

			//プレイヤーが範囲内にいる
			if (IsPlayerInArea()) {
				if (toRotate) {
					NavMeshStop();
					LookPlayer();
				}
			}
			//プレイヤーが視界外にいる
			else if (!fieldOfVision.IsPlayerInArea(player, true) || IsLookingPlayer()) {
				time += Slow.Instance.DeltaTime();

				if (time >= giveUpTime) {
					NavMeshStop();
					break;
				}
			}
			//視界内にいて近づいていなければ移動する
			else {
				NavMeshStart();
			}

			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}

		isNearPlayer = IsPlayerInArea();
		enemyAnimation.StopRunAnimation();
		NavMeshStop();
	}

	private bool IsPlayerInArea() {
		return judgeArea.IsPlayerInArea(player, true);
	}

	private void NavMeshStop() {
		agent.velocity = Vector3.zero;
		//agent.SetDestination(transform.position);
		agent.updatePosition = false;
		agent.updateRotation = false;
		agent.isStopped = true;
	}

	private void NavMeshStart() {
		agent.updatePosition = true;
		agent.updateRotation = true;
		agent.isStopped = false;
	}

	private void UpdateNavMesh(Vector3 position) {
		agent.destination = (position);
		agent.isStopped = false;
	}

	private bool IsEndChase() {
		//近づいていなかったらfalse
		if (!IsPlayerInArea()) return false;

		//プレイヤーの方向を向いてなくてよいならここでtrueを返す
		if (!toRotate) return true;

		//プレイヤーの方向を向いているか
		return IsLookingPlayer();
	}

	private bool IsLookingPlayer() {
		int layerMask = LayerMask.GetMask(new string[] { LayerName.Stage.String(), LayerName.PlayerDamageable.String() });
		//プレイヤーのピボットが変な場所にあるので少し上げる
		Vector3 eyePos = rootTransform.position;
		eyePos.y = player.transform.position.y + 1;
		Ray ray = new Ray(eyePos, transform.forward);
		//前方にレイを飛ばして最初に当たったのがプレイヤーならプレイヤーを見ているとする
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
			if (hit.collider.tag == player.tag) {
				return true;
			}
		}

		return false;
	}

	private void LookPlayer() {
		Quaternion current = rootTransform.rotation;
		Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - rootTransform.position);
		float deltaAngle = Mathf.DeltaAngle(current.eulerAngles.y, lookRotation.eulerAngles.y);
		float rotateY = deltaAngle;

		//一定値以下なら回転しない
		if (Mathf.Abs(deltaAngle) < 0.1f) return;

		if (deltaAngle > 0.0f) {
			rotateY = Mathf.Max(rotateSpeed * Slow.Instance.DeltaTime(), deltaAngle);
		} else {
			rotateY = Mathf.Min(-rotateSpeed * Slow.Instance.DeltaTime(), deltaAngle);
		}

		rootTransform.Rotate(new Vector3(0, rotateY, 0));
	}
}
