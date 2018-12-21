using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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


    bool isPierceMove = false;// 吸生カメラ中か

    private PlayerState prePlayerState;// 前フレームのPlayerState
    private PlayerState curPlayerState;// 現フレームのPlayerState
    private List<MeshRenderer> hitsMeshRenderer = new List<MeshRenderer>();//非表示メッシュレンダラー格納リスト

    private bool isCounterMove = false;// はじきカメラ中か
    private bool preIsSlow = false;// 前フレームのisSlowNow
    private bool curIsSlow = false;// 現フレームのisSlowNow

    // Use this for initialization
    private void Start() {
        GetPlayerIfNull();
		offset = transform.position - player.transform.position;
		isLockOn = false;
		interval = false;
		GetIsInveted();
		intervalTime = 0;

        // 回転・位置の初期化
        PositionToPlayerBack();
	}

	// Update is called once per frame
	private void Update() {

        curPlayerState = player.GetComponent<PlayerAction>().state;
        curIsSlow = Slow.Instance.isSlowNow;

        DefaultControl();
		IsLockOnChange();
		CheckLockonMode();
		LockOn();
		Interval();
        PierceCamera();
        CheckPierceState();
        CounterCamera();
        CheckCounterSlow();

        prePlayerState = curPlayerState;
        preIsSlow = curIsSlow;
    }

	/// <summary>
	/// カメラ設定を更新します。
	/// ポーズ画面からもオプションへ飛べるようになったため
	/// Start() で一度だけ反転の情報を取得する実装のままだと、
	/// プレイ中にカメラ設定を更新できなくなってしまいます。
	/// そこで、ポーズからプレイ画面に戻るたびに毎回このメソッドを呼び出します。
	/// </summary>
	public void UpdateSetting() {
		GetIsInveted();
	}

    private void DefaultControl() {
		if (coroutineCount > 0 || finished || isPierceMove || isCounterMove) {
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
		if (!isLockOn || nearObj == null || isCounterMove || isPierceMove) {
			
		} {
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
            SetPlayerRotation();
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

        return distanceW = distanceW > defaultDistance ? defaultDistance : distanceW;
	}

	//カメラがプレイヤーの背後に戻す
	public void PositionToPlayerBack() {
        //1fボス部屋でプレイヤーを参照するよりさきに
        //このメソッドが呼び出されることにより NullReferenceException が発生していたため
        GetPlayerIfNull();
        //回転
        vRotation = Quaternion.Euler(20, 0, 0);                                     // 垂直回転(X軸を軸とする回転)は、20度見下ろす回転
        hRotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0);// 水平回転(Y軸を軸とする回転)は、プレイヤーの向きに合わせ
        transform.rotation = hRotation * vRotation;                                 // 最終的なカメラの回転は、垂直回転してから水平回転する合成回転

        // 位置
        nowDistance = defaultDistance;
		transform.position = player.transform.position - transform.rotation * Vector3.forward * nowDistance;
	}

    /// <summary>
    /// 回転をプレイヤーへ適用
    /// </summary>
    private void SetPlayerRotation()
    {
        var euler = transform.rotation.eulerAngles;
        hRotation = Quaternion.Euler(0, euler.y, 0);
        vRotation = Quaternion.Euler(euler.x, 0, 0);
    }

    /// <summary>
    /// はじきカメラ
    /// </summary>
    private void CounterCamera() {
        if (isCounterMove) return;

        if (curIsSlow) {
            isCounterMove = true;

            //プレイヤーと壁の距離を取る
            float defaultRightRange = 3;
            float defaultBackRange = 1.5f;
            //はじきカメラ経路終点設定
            Vector3 direction2EndPos = (-player.transform.forward * defaultBackRange + player.transform.right * defaultRightRange);
            Vector3 endPos = player.transform.position + new Vector3(0, 1.5f, 0) + direction2EndPos;
            float distance2EndPos = Vector3.Distance(player.transform.position + new Vector3(0, 1, 0), endPos);
            Ray counterRay = new Ray(player.transform.position + new Vector3(0, 1, 0), direction2EndPos.normalized);
            RaycastHit counterHit;
            if (Physics.Raycast(counterRay, out counterHit, distance2EndPos, LayerMask.GetMask("Stage")))
            {
                endPos = counterHit.point;
            }

            Vector3 defaultPos = transform.position;
            //カメラ経路を設定
            Vector3[] tmpPos = new Vector3[] { defaultPos,
                                                endPos };
            //DOTweenでカメラを動かす
            transform.DOLocalPath(tmpPos, 3f, PathType.CatmullRom, PathMode.Full3D, 10, Color.green).SetEase(Ease.OutQuart);
        }
    }

    /// <summary>
    /// はじきのスローチェック
    /// </summary>
    private void CheckCounterSlow() {
        if (isCounterMove) {
            //プレイヤーを注目
            transform.LookAt(player.transform.position + new Vector3(0, 1.5f, 0) + (player.transform.forward * 0.5f), Vector3.up);
            SetPlayerRotation();
        }

        if (preIsSlow && !curIsSlow) {
            isCounterMove = false;
            if (hitsMeshRenderer.Count != 0) {
                foreach (var rh in hitsMeshRenderer) {
                    rh.enabled = true;
                }
                hitsMeshRenderer.Clear();
            }
            SetPlayerRotation();
        }
    }

    /// <summary>
    /// 吸生カメラ
    /// </summary>
    private void PierceCamera() {
        if (isPierceMove) return;

        //吸生状態
        if (player.GetComponent<PlayerAction>().state == PlayerState.Pierce) {
            isPierceMove = true;
            PositionToPlayerBack();//一回初期位置に戻す
            Vector3 defaultPos = transform.position;

            //プレイヤーと壁の距離を取る
            float defaultRightRange = 2;
            float defaultForwardRange = 2;

            //吸生カメラ経路終点設定
            Vector3 direction2EndPos = player.transform.right * (defaultRightRange * 0.75f) + player.transform.forward * defaultForwardRange;
            Vector3 direction2EndPosREV = -player.transform.right * (defaultRightRange * 0.75f) + player.transform.forward * defaultForwardRange;

            Vector3 endPos = player.transform.position + new Vector3(0, 1f, 0) + direction2EndPos;
            Vector3 endPosREV = player.transform.position + new Vector3(0, 1f, 0) + direction2EndPosREV;

            float distance2EndPos = Vector3.Distance(player.transform.position + new Vector3(0, 1, 0), endPos);
            Ray pierceRay = new Ray(player.transform.position + new Vector3(0, 1, 0), direction2EndPos.normalized);
            RaycastHit pierceHit;
            if (Physics.Raycast(pierceRay, out pierceHit, distance2EndPos, LayerMask.GetMask("Stage")))
            {
                endPos = pierceHit.point;
                if (pierceHit.distance <= 1f) endPos = endPosREV;
            }

            //吸生カメラ経路中点設定
            Ray rightRay = new Ray(player.transform.position + new Vector3(0, 1, 0), player.transform.right);
            RaycastHit rightHit;
            float rightRange = defaultRightRange;
            if (Physics.Raycast(rightRay, out rightHit, defaultRightRange + 1, LayerMask.GetMask("Stage")))
            {
                rightRange = rightHit.distance <= defaultRightRange ? rightHit.distance : defaultRightRange;
                if (rightHit.distance <= 1f) rightRange = -defaultRightRange;
            }

            //カメラ経路を設定
            Vector3[] tmpPos = new Vector3[] { defaultPos,
                                                player.transform.position + new Vector3(0, 1.5f, 0) + player.transform.right*rightRange,
                                                endPos };
            //DOTweenでカメラを動かす
            transform.DOLocalPath(tmpPos, 1.5f, PathType.CatmullRom, PathMode.Full3D, 10, Color.green).SetEase(Ease.OutQuart);
        }
    }

    /// <summary>
    /// 吸生状態チェック
    /// </summary>
    private void CheckPierceState() {
        //吸生カメラ中
        if (isPierceMove) {
            //プレイヤーを注目
            transform.LookAt(player.transform.position + new Vector3(0, 1, 0), Vector3.up);
        }
        //吸生状態終了時
        if (prePlayerState == PlayerState.Pierce && curPlayerState != PlayerState.Pierce) {
            isPierceMove = false;
            if (hitsMeshRenderer.Count != 0) {
                foreach (var rh in hitsMeshRenderer) {
                    rh.enabled = true;
                }
                hitsMeshRenderer.Clear();
            }
            //位置をデフォに
            PositionToPlayerBack();
        }
    }

    /// <summary>
    /// プレイヤーのY1のところからカメラまでの距離
    /// </summary>
    /// <returns></returns>
    private float Distance_Player2Camera() {
        return Vector3.Distance(new Vector3(player.transform.position.x, 1, player.transform.position.z),
                                transform.position);
    }

    /// <summary>
    /// プレイヤーとの間にステージがあったら表示しない（簡易めり込み対策）
    /// </summary>
    private void NotDisplayStageBetweenWithPlayer() {
        Ray ray = new Ray(player.transform.position + new Vector3(0, 1, 0), -transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Distance_Player2Camera() + 1, LayerMask.GetMask("Stage"))) {
            MeshRenderer pMeshR = hit.transform.parent.parent.GetComponent<MeshRenderer>();
            if (pMeshR.enabled) {
                hitsMeshRenderer.Add(pMeshR);
                pMeshR.enabled = false;
            }
        }
        else {
            if (hitsMeshRenderer.Count != 0) {
                foreach (var rh in hitsMeshRenderer) {
                    rh.enabled = true;
                }
                hitsMeshRenderer.Clear();
            }
        }
    }

    private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Ray ray = new Ray(player.transform.position + new Vector3(0, 1, 0), -transform.forward);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, defaultDistance + 1, LayerMask.GetMask("Stage"))) {
			Gizmos.DrawLine(transform.position, hit.point);
		}
    }

    private void GetPlayerIfNull() {
        if (player == null) {
            this.player = GameObject.FindGameObjectWithTag(TagName.Player.String());
        }
    }
}