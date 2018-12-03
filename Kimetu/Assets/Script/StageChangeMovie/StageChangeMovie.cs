using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Playables;



public class StageChangeMovie : MonoBehaviour {
	[SerializeField, Header("再生するタイムライン")]
	private PlayableDirector playableDirector;
	[SerializeField, Header("タイムライン再生時、プレイヤーにいてほしい座標")]
	private Transform playerStartTransform;
	[SerializeField, Header("タイムライン再生時、カメラにいてほしい座標と角度")]
	private Transform cameraStartTransform;
	[SerializeField, Header("ドアの場所")]
	private Transform doorPosition;
	[SerializeField, Header("タイムライン再生前に移動するプレイヤーの移動速度")]
	private float speed;
	private GameObject player; //プレイヤーオブジェクト
	private Camera mainCamera; //メインカメラ
	private Rigidbody playerRigid; //プレイヤーのリジッドボディ
	/// <summary>
	/// ムービー再生中か？
	/// </summary>
	public bool isMoviePlaying { private set; get; }

	private void Start() {
		player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		playerRigid = player.GetComponent<Rigidbody>();
		mainCamera = Camera.main;
	}

	/// <summary>
	/// タイムライン再生
	/// </summary>
	public void StartMovie() {
		isMoviePlaying = true;
		StartCoroutine(Movie());
	}

	/// <summary>
	/// ムービー終了時のコールバック
	/// </summary>
	/// <param name="director"></param>
	public void MovieStop(PlayableDirector director) {
		if (director == playableDirector) {
			isMoviePlaying = false;
		}
	}

	/// <summary>
	/// ムービー再生
	/// </summary>
	/// <returns></returns>
	private IEnumerator Movie() {
		isMoviePlaying = true;
		//プレイヤーを初期地点まで移動させる
		yield return StartCoroutine(PlayerPreparation());
		//カメラを初期地点に移動させる
		yield return StartCoroutine(CameraPreparation());
		//ムービー終了時のコールバック登録
		playableDirector.stopped += MovieStop;
		Destroy(playerRigid);
		playableDirector.Play();
	}

	/// <summary>
	/// プレイヤーのムービー準備
	/// </summary>
	/// <returns></returns>
	private IEnumerator PlayerPreparation() {
		//初期地点に向かせる
		player.transform.LookAt(playerStartTransform);
		//方向と距離を取得
		Vector3 pos = playerRigid.position;
		Vector3 direction = playerStartTransform.position - playerRigid.position;
		float distance = direction.magnitude;
		direction.Normalize();

		//一定距離まで移動させる
		while (distance > 0.1f) {
			//向いている方向に一定速度で移動する
			playerRigid.MovePosition(player.transform.position + player.transform.forward * speed * Time.deltaTime);
			distance = (playerStartTransform.position - playerRigid.position).magnitude;
			yield return null;
		}

		//場所を補正
		player.transform.position = playerStartTransform.position;
		//ドアの方向に向かせる
		Vector3 target = doorPosition.position - player.transform.position;
		target.y = player.transform.position.y;
		player.transform.rotation = Quaternion.LookRotation(target, Vector3.up);
	}

	/// <summary>
	/// カメラのムービー準備
	/// </summary>
	/// <returns></returns>
	private IEnumerator CameraPreparation() {
		float cameraMoveTime = 1.0f;
		Vector3 position = mainCamera.transform.position;
		Quaternion rotate = mainCamera.transform.rotation;
		float time = 0.0f;

		//カメラを時間まで移動、回転させる
		while (time < cameraMoveTime) {
			mainCamera.transform.position = Vector3.Lerp(position, cameraStartTransform.position, time / cameraMoveTime);
			mainCamera.transform.rotation = Quaternion.Lerp(rotate, cameraStartTransform.rotation, time / cameraMoveTime);
			time += Time.deltaTime;
			yield return null;
		}

		//カメラの座標、角度補正
		mainCamera.transform.position = cameraStartTransform.position;
		mainCamera.transform.rotation = cameraStartTransform.rotation;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			StartMovie();
		}
	}
}
