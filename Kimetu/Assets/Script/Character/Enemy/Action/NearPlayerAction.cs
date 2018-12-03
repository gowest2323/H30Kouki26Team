using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NearPlayerAction : MonoBehaviour, IEnemyActionable {
	[SerializeField, Tooltip("十分接近したと判断する範囲")]
	private EnemySearchableAreaBase nearJudgeArea;
	[SerializeField, Tooltip("目の場所")]
	private Transform eyeTransform;
	[SerializeField]
	private NavMeshAgent agent;
	[SerializeField, Tooltip("この時間経過したら視界外の場合追跡を終了する")]
	private float limitNearTime;
	[SerializeField, Header("すでに近づいているかを判定するためにレイを使用するなら true")]
	private bool useRay = false;
	[SerializeField]
	private float lookRotationSpeed = 6f;
	private GameObject playerObj;
	public bool isNearPlayer { private set; get; }
	private EnemyAnimation enemyAnimation;
	public string informationText { private set; get; }

	private void Awake() {
		enemyAnimation = GetComponentInParent<EnemyAnimation>();
		//プレイヤーを検索する
		playerObj = GameObject.FindGameObjectWithTag(TagName.Player.String());
	}

	private void Start() {
		enemyAnimation = GetComponentInParent<EnemyAnimation>();
	}

	/// <summary>
	/// 接近行動
	/// </summary>
	/// <param name="callBack"></param>
	/// <returns></returns>
	public IEnumerator Action(UnityAction callBack) {
		//初期化処理
		isNearPlayer = false;
		agent.isStopped = false;
		enemyAnimation.StartRunAnimation();
      

		float time = 0.0f;

		//範囲内にいてプレイヤーの方向を向いていたら近づいたと判断
		while (true) {
			informationText = "MoveNear";
			bool isPlayerInArea = nearJudgeArea.IsPlayerInArea(playerObj, true);
			bool isLookAtPlayer = IsLookAtPlayer(playerObj);
            EffectManager.Instance.EnemyMoveEffectCreate(gameObject);
            //範囲外かつ視界外で一定時間経過後に追跡終了
            if (!isPlayerInArea && !isLookAtPlayer) {
				time += Slow.Instance.DeltaTime();

				if (time > limitNearTime) {
					isNearPlayer = false;
					agent.isStopped = true;
					enemyAnimation.StopRunAnimation();
					callBack.Invoke();
					yield break;
				}
			}
			//視界内ならタイマーをカウントしない
			else {
				time = 0.0f;
			}

			//範囲外なら追尾
			if (!isPlayerInArea) {
				agent.isStopped = false;
				agent.SetDestination(playerObj.transform.position);
				yield return null;
			}
			//範囲内で視界外の場合プレイヤーの方向を向く
			else if (!isLookAtPlayer) {
				informationText = "Look";
				//NavMeshが動いているとプレイヤーに接近しすぎるので停止
				agent.isStopped = true;
				LookPlayer(playerObj, Slow.Instance.DeltaTime() * lookRotationSpeed);
				yield return null;
			}
			//範囲内かつ視界内なら終了
			else {
				break;
			}
		}

		//終了処理
		isNearPlayer = true;
		agent.isStopped = true;
		enemyAnimation.StopRunAnimation();
		callBack.Invoke();
	}

	/// <summary>
	/// プレイヤーを見ているか
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	private bool IsLookAtPlayer(GameObject player) {
		//攻撃手段が複数あるエネミーでは、useRayをfalseにしておく
		//このアクションで毎回プレイヤーの方を向いてしまうと、
		//振り下ろしのような前方に長い当たり判定を持つ攻撃ばかり使われてしまいます。
		//(FirstBossAI参照)
		if (!useRay) { return true; }

		int layerMask = LayerMask.GetMask(new string[] {LayerName.Stage.String(), LayerName.PlayerDamageable.String()});
		//プレイヤーのピボットが変な場所にあるので少し上げる
		var eyePos = eyeTransform.position;
		eyePos.y = playerObj.transform.position.y + 1;
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

	/// <summary>
	/// プレイヤーの方向を向く
	/// </summary>
	/// <param name="playerObj"></param>
	/// <param name="t"></param>
	private void LookPlayer(GameObject playerObj, float t) {
		//Enemyの一番上の要素を取得
		var topTransform = GetComponentInParent<Rigidbody>().transform;
		Vector3 targetPosition = playerObj.transform.position;
		targetPosition.y = topTransform.position.y;
		topTransform.rotation = Quaternion.Lerp(topTransform.rotation,
												Quaternion.LookRotation(targetPosition - topTransform.position),
												t);
	}

}