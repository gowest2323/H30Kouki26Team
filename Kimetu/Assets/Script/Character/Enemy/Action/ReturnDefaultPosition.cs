using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class ReturnDefaultPosition : ActionBase {
	private GameObject player;
	private Vector3 defaultPosition;
	private Quaternion defaultRotation;
	private Transform rootTransform;
	[SerializeField, Header("プレイヤーを途中で見つけたら中断するか")]
	private bool isCancelDetectPlayer;
	public bool detectPlayer { get; private set; }
	private NavMeshAgent agent;
	[SerializeField, Header("目的地に近づいたと判断する距離")]
	private float remainingDistance = 0.7f;
	[SerializeField, Header("目的角に回転したと判断する大きさ")]
	private float remainingRotate = 1.0f;
	[SerializeField, Header("目的角に回転するのにかける時間")]
	private float rotateSecond = 1.0f;
	[SerializeField, Header("敵の視界")]
	private EnemySearchableAreaBase searchArea;
	protected override void Start() {
		base.Start();
		player = GetPlayer();
		rootTransform = GetRootTransform();
		agent = rootTransform.GetComponent<NavMeshAgent>();
		Assert.IsNotNull(agent, "NavMeshが取得できませんでした。");
		defaultPosition = rootTransform.position;
		defaultRotation = rootTransform.rotation;
	}


	public override IEnumerator Action() {
		cancelFlag = false;
		enemyAnimation.StartRunAnimation();
		agent.isStopped = false;
		agent.updatePosition = true;
		agent.updateRotation = true;
		agent.SetDestination(defaultPosition);
		//パスの計算が終わるまで待機
		yield return new WaitWhile(() => agent.pathPending);
		detectPlayer = false;

		//元の場所に戻るまで続ける
		while (!IsReturnPosition()) {
			if (ForcedTermination()) break;

			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}

		StopNavMeshAgent();
		float time = 0.0f;
		Quaternion beforeRotation = rootTransform.rotation;

		//元の角度に戻るまで続ける
		while (!IsReturnRotation()) {
			if (ForcedTermination()) break;

			float slowDelta = Slow.Instance.DeltaTime();
			time += slowDelta;
			rootTransform.rotation = Quaternion.Slerp(beforeRotation, defaultRotation, (time / rotateSecond));
			yield return new WaitForSeconds(slowDelta);
		}

		StopNavMeshAgent();
		detectPlayer = IsDetectPlayer();
	}

	// Use this for initialization
	private void StopNavMeshAgent() {
		agent.isStopped = true;
		agent.velocity = Vector3.zero;
		agent.updatePosition = false;
		agent.updateRotation = false;
	}

	private bool ForcedTermination() {
		//キャンセル
		if (cancelFlag) {
			return true;
		}

		//プレイヤーを見つけたらキャンセルする場合プレイヤーを見つけたかどうか判定
		if (isCancelDetectPlayer) {
			//見つけたら終了
			if (IsDetectPlayer()) {
				return true;
			}
		}

		return false;
	}

	private bool IsReturnPosition() {
		return (agent.remainingDistance < remainingDistance);
	}

	private bool IsReturnRotation() {
		return Quaternion.Angle(defaultRotation, rootTransform.rotation) < remainingRotate;
	}

	private bool IsDetectPlayer() {
		return searchArea.IsPlayerInArea(player, true);
	}

	private bool IsEnd() {
		//プレイヤーを見つけたらキャンセルするか
		if (isCancelDetectPlayer) {
			//プレイヤーを見つけたらキャンセルする
			if (IsDetectPlayer()) return true;
		}

		//元の場所に戻っていなければfalse
		if (!IsReturnPosition()) return false;

		return IsReturnRotation();
	}
}