using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;
	private Vector3 offset;
	private GameObject nearObj;
	[SerializeField]
	private float lockRange = 10.0f;
	bool isLockOn;
	bool interval;
	float intervalTime;
	public bool isInverted_UpDown;
	public bool isInverted_LeftRight;


	[SerializeField]
	private float defaultDistance = 4.0f;    // 注視対象プレイヤーからカメラを離す距離
	[SerializeField]
	private Quaternion vRotation;      // カメラの垂直回転(見下ろし回転)
	[SerializeField]
	public Quaternion hRotation;      // カメラの水平回転
	[SerializeField]
	private float turnSpeed = 10.0f;   // 回転速度

	private Coroutine rotateCoroutine;
	private bool finished;
	private int coroutineCount;

	[SerializeField]
	private float lockonCameraAngleY = 0.9f;
	[SerializeField]
	private float nearLockonCameraAngleY = 1.5f;

	[SerializeField]
	private float cameraHighestAngle = 45.0f;
	[SerializeField]
	private float cameraLowestAngle = -10.0f;   // カメラ垂直最低角度

	private float nowDistance = 0;  //今カメラとプレイヤーの距離

	[HideInInspector]
	public Vector3 playerDir;   //プレイヤーの向き

	private bool chooseTargetSelf = false;//手動でロックオンターゲット選択か？
	private bool isCheckingTargetChange = false;//ターゲット変更チェック中か？

	// Use this for initialization
	private void Start() {
		offset = transform.position - player.transform.position;
		isLockOn = false;
		interval = false;
		GetIsInveted();
		intervalTime = 0;
		// 回転の初期化
		vRotation = Quaternion.Euler(20, 0, 0);         // 垂直回転(X軸を軸とする回転)は、30度見下ろす回転
		hRotation = Quaternion.identity;                // 水平回転(Y軸を軸とする回転)は、無回転
		transform.rotation = hRotation * vRotation;     // 最終的なカメラの回転は、垂直回転してから水平回転する合成回転

		nowDistance = defaultDistance;

		// 位置の初期化
		// player位置から距離distanceだけ手前に引いた位置を設定します
		transform.position = player.transform.position - transform.rotation * Vector3.forward * nowDistance;
	}

	// Update is called once per frame
	private void Update() {
		DefaultControl();
		IsLockOnChange();
		CheckLockonMode();
		LockOn();
		Interval();
	}

	private void DefaultControl() {
		if (coroutineCount > 0 || finished) {
			return;
		}

		//*
		//Debug.Log("DefaultControl");
		float hor = Input.GetAxis(InputMap.Type.RStick_Horizontal.GetInputName());
		float ver = Input.GetAxis(InputMap.Type.RStick_Vertical.GetInputName());

		// カメラの回転(transform.rotation)の更新
		// 垂直回転してから水平回転する合成回転とします

		//回転計算
		if (Mathf.Abs(hor) >= 0.1f ||                                           //カメラの水平操作がある時
				player.GetComponent<PlayerAction>().state == PlayerState.Defence ) { //防御中
			if (!isInverted_LeftRight) {
				//カメラの操作に任せる
				transform.RotateAround(player.transform.position, Vector3.up, hor * turnSpeed);
				//プレイヤーの向きの計算（水平）
				hRotation *= Quaternion.Euler(0, hor * turnSpeed, 0);
			} else {
				//カメラの操作に任せる
				transform.RotateAround(player.transform.position, Vector3.up, -hor * turnSpeed);
				//プレイヤーの向きの計算（水平）
				hRotation *= Quaternion.Euler(0, -hor * turnSpeed, 0);
			}
		} else { //カメラの水平操作がない時
			//プレイヤーの向きに沿って自動水平回転（速度半分）
			if (Mathf.Abs(playerDir.x) >= 0.1f)
				transform.RotateAround(player.transform.position, Vector3.up, playerDir.x * (turnSpeed / 2));

			//プレイヤーの向きの計算（水平）
			hRotation *= Quaternion.Euler(0, playerDir.x * (turnSpeed / 2), 0);
		}

		if (Mathf.Abs(ver) >= 0.05f) { //カメラの垂直操作がある時
			if (!isInverted_UpDown) {
				transform.Rotate(new Vector3(ver * turnSpeed, 0, 0));
				//プレイヤーの向きの計算（垂直）
				vRotation *= Quaternion.Euler(ver * turnSpeed, 0, 0);
			} else {
				transform.Rotate(new Vector3(-ver * turnSpeed, 0, 0));
				//プレイヤーの向きの計算（垂直）
				vRotation *= Quaternion.Euler(-ver * turnSpeed, 0, 0);
			}
		}

		//角度制限
		float rotationX = transform.eulerAngles.x;
		rotationX = (rotationX > 180) ? rotationX -= 360 : rotationX;

		if (rotationX > cameraHighestAngle) {
			rotationX = cameraHighestAngle;
		} else if (rotationX < cameraLowestAngle) {
			rotationX = cameraLowestAngle;
		}

		transform.eulerAngles = new Vector3(rotationX, transform.eulerAngles.y, 0);

		// カメラの位置(transform.position)の更新
		// player位置から距離distanceだけ手前に引いた位置を設定します

		// 位置を壁の内側に
		nowDistance = Distance_PlayerNearWall() > 0f ? Distance_PlayerNearWall() : defaultDistance;

		if (nowDistance <= 0f) nowDistance = 0f;

		transform.position = player.transform.position + new Vector3(0, 1, 0) - transform.rotation * Vector3.forward * nowDistance;
		//*/
	}

	public bool IsLockOn() {
		return isLockOn;
	}

	private void GetIsInveted() {
		isInverted_LeftRight = OptionDataPrefs.GetLeftRightBool(false);
		isInverted_UpDown = OptionDataPrefs.GetUpDownBool(false);
	}

	private void LockOn() {
		if (!isLockOn || nearObj == null) {
			StopLockOnStart();
			this.finished = false;
			return;
		}

		if (this.rotateCoroutine != null) {
			if (!finished) { return; }

			StopLockOnStart();
		}

		this.finished = false;
		this.rotateCoroutine = StartCoroutine(LockOnStart(nearObj.transform.position));
	}

	private void StopLockOnStart() {
		if (rotateCoroutine != null) {
			StopCoroutine(rotateCoroutine);
			this.rotateCoroutine = null;
			this.coroutineCount = 0;
		}
	}

	private IEnumerator LockOnStart(Vector3 nearPos) {
		this.finished = false;
		this.coroutineCount++;
		var waitOne = new WaitForEndOfFrame();
		var selfPos = transform.position;
		var playerPos = player.transform.position;
		var playerLookNearPos = nearPos;//注目標的(敵)の位置(足元)
		var selfStartRotation = transform.rotation;
		var selfEndRotation = Quaternion.LookRotation((nearPos + new Vector3(0, 1.7f, 0)) - selfPos, Vector3.up);//注目点と注目元の高さを頭ぐらいの高さに
		var playerStartRotation = player.transform.rotation;
		var playerEndRotation = Quaternion.LookRotation(new Vector3(playerLookNearPos.x, selfPos.y, playerLookNearPos.z) - new Vector3(playerPos.x, selfPos.y, playerPos.z),
								Vector3.up);//注目点と注目元の高さをカメラの高さに合わせないとプレイヤーが回転しちゃう

		if (Quaternion.Angle(selfStartRotation, selfEndRotation) < 0.1f &&
				Quaternion.Angle(playerStartRotation, playerEndRotation) < 0.1f) {
			this.coroutineCount--;
			this.finished = true;
			Debug.Log("BREAK");
			yield break;
		}

		var offset = 0f;
		var seconds = 0.2f;

		while (offset < seconds) {
			var t = Time.time;
			yield return waitOne;
			var diff = (Time.time - t);
			offset += diff;
			offset = Mathf.Clamp(offset, 0, seconds);
			var selfProgRot = Quaternion.Slerp(selfStartRotation, selfEndRotation, offset / seconds);
			var playerProgRot = Quaternion.Slerp(playerStartRotation, playerEndRotation, offset / seconds);
			transform.rotation = selfProgRot;
			player.transform.rotation = playerProgRot;

			// 位置を壁の内側に
			nowDistance = Distance_PlayerNearWall() > 0f ? Distance_PlayerNearWall() : defaultDistance;

			if (nowDistance <= 0f) nowDistance = 0f;

			transform.position = player.transform.position + new Vector3(0, 1, 0) - transform.rotation * Vector3.forward * nowDistance;

			//微妙に見下ろすように
			if (nowDistance < defaultDistance)
				transform.position += (Vector3.up * nearLockonCameraAngleY);
			else
				transform.position += (Vector3.up * lockonCameraAngleY);

			//回転をプレイヤーへ適用
			var euler = transform.rotation.eulerAngles;
			hRotation = Quaternion.Euler(0, euler.y, 0);
			vRotation = Quaternion.Euler(euler.x, 0, 0);
		}

		yield return waitOne;
		this.coroutineCount--;
		this.finished = true;
	}

	private void IsLockOnChange() {
		if (Input.GetButtonDown(InputMap.Type.RStickClick.GetInputName())) {
			if (interval) return;

			isLockOn = !isLockOn;

			if (!isLockOn) {
				chooseTargetSelf = false;
			}

			interval = true;
		}
	}

	private void CheckLockonMode() {
		if (isLockOn == true) {
			//入力
			var targetInput = Input.GetAxis(InputMap.Type.RStick_Horizontal.GetInputName());

			if (Mathf.Abs(targetInput) > 0.3f) chooseTargetSelf = true;

			//ターゲット選択モード
			if (chooseTargetSelf)
				nearObj = SearchTagWithInput(gameObject, "Enemy");
			else
				nearObj = SearchTagWithDirection(gameObject, "Enemy");

			if (nearObj == null) {
				isLockOn = false;
				interval = false;
			}
		}
	}

	private void Interval() {
		if (interval) {
			intervalTime += 0.1f;

			if (intervalTime >= 0.6f) {
				interval = false;
				intervalTime = 0;
			}
		}
	}

	/// <summary>
	/// 近くにあるtagnameのオブジェクトを向きで取得
	/// </summary>
	/// <param name="nowObj"></param>
	/// <param name="tagName"></param>
	/// <returns></returns>
	GameObject SearchTagWithDirection(GameObject nowObj, string tagName) {
		float tmpDis = 0;
		Vector3 tmpDir = Vector3.zero;//方向

		float currentFace = 0;
		float lastFace = 0;

		GameObject targetObj = null;

		foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName)) {
			tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);
			tmpDir = (obs.transform.position - nowObj.transform.position).normalized;

			var enemyStatus = obs.GetComponent<EnemyStatus>();

			//死亡しているエネミーにはロックオンしない
			if (enemyStatus.IsDead()) {
				continue;
			}

			currentFace = Vector3.Dot(nowObj.transform.forward, tmpDir);

			//ターゲットがない時
			if (targetObj == null) {
				//範囲内にいたらターゲットにする(nowObj向き関係ない)
				if (lockRange > tmpDis) {
					targetObj = obs;
				}
			}

			//それ以降範囲内＋nowObj正面に一番近い敵をターゲットに
			if (lockRange > tmpDis &&
					currentFace > lastFace) {
				targetObj = obs;
			}

			lastFace = currentFace;
		}

		return targetObj;
	}

	/// <summary>
	/// 近くにあるtagnameのオブジェクトを入力で取得
	/// </summary>
	/// <param name="nowObj"></param>
	/// <param name="tagName"></param>
	/// <returns></returns>
	GameObject SearchTagWithInput(GameObject nowObj, string tagName) {
		GameObject targetObj = null;
		float tmpDis = 0;
		Vector3 tmpDir = Vector3.zero;//方向

		//一回目
		if (nearObj == null) {
			foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName)) {
				tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

				var enemyStatus = obs.GetComponent<EnemyStatus>();

				//死亡しているエネミーにはロックオンしない
				if (enemyStatus.IsDead()) {
					continue;
				}

				if (lockRange > tmpDis) {
					targetObj = obs;
				}
			}
		} else { //二回以降
			//今の選択を維持
			targetObj = nearObj;

			//入力
			var targetInput = Input.GetAxis(InputMap.Type.RStick_Horizontal.GetInputName());

			if (Mathf.Abs(targetInput) > 0.3f) isCheckingTargetChange = true;

			if (isCheckingTargetChange) {
				foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName)) {
					tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

					// 距離外の敵取っちゃったらチェックしない
					if (lockRange < tmpDis) continue;

					tmpDir = (obs.transform.position - nowObj.transform.position).normalized;

					var targetDir = (targetObj.transform.position - nowObj.transform.position).normalized;

					var enemyStatus = obs.GetComponent<EnemyStatus>();

					//死亡しているエネミーにはロックオンしない
					if (enemyStatus.IsDead()) {
						continue;
					}

					//今のターゲットと同じならチェックしない
					if (obs == targetObj) continue;

					//左右判定
					var nextDir = Vector3.Cross(targetDir, tmpDir).y > 0 ? 1 : -1;

					//右
					if (targetInput > 0.3f && nextDir > 0) {
						targetObj = obs;
						isCheckingTargetChange = false;
					}

					//左
					if (targetInput < -0.3f && nextDir < 0) {
						targetObj = obs;
						isCheckingTargetChange = false;
					}
				}
			}

			//距離チェック
			tmpDis = Vector3.Distance(targetObj.transform.position, nowObj.transform.position);

			if (lockRange < tmpDis) {
				targetObj = null;
			}
		}

		return targetObj;
	}

	/// <summary>
	/// プレイヤーが壁にカメラのデフォ距離値より近づいた時距離を返す
	/// </summary>
	/// <returns>壁との距離</returns>
	private float Distance_PlayerNearWall() {
		Ray ray = new Ray(player.transform.position + new Vector3(0, 1, 0), -transform.forward);

		RaycastHit hit;
		float distanceW = 0;

		if (Physics.Raycast(ray, out hit, defaultDistance + 1, LayerMask.GetMask("Stage"))) {
			distanceW = Vector3.Distance(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z),
										 new Vector3(hit.point.x, transform.position.y, hit.point.z));
		} else {
			distanceW = 0;
		}

		return distanceW;
	}

	//カメラがプレイヤーの背後に戻す
	public void PositionToPlayerBack() {
		vRotation = Quaternion.Euler(20, 0, 0);
		hRotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0);//プレイヤーの向きに合わせ

		//回転
		transform.rotation = hRotation * vRotation;

		// 位置
		nowDistance = defaultDistance;
		transform.position = player.transform.position - transform.rotation * Vector3.forward * nowDistance;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Ray ray = new Ray(player.transform.position + new Vector3(0, 1, 0), -transform.forward);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, defaultDistance + 1, LayerMask.GetMask("Stage"))) {
			Gizmos.DrawLine(transform.position, hit.point);
		}
	}
}