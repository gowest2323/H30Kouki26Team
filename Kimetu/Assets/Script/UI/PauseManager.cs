using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
	public bool isPause { private set; get; }
	public bool isReturnFromPause { set; get; }
	private CameraController cameraController;
	private PlayerStatus playerStatus;

	private void Start() {
		isPause = false;
		this.cameraController = Camera.main.gameObject.GetComponent<CameraController>();
		this.playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
	}

	/// <summary>
	/// ポーズ開始
	/// </summary>
	public void Pause() {
		isPause = true;
		isReturnFromPause = false;
		CoroutineManager.Instance.StopAllCoroutineEx(false);
		Time.timeScale = 0.0f;
		//ポーズシーンを追加読み込み
		SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
	}

	/// <summary>
	/// 再開
	/// </summary>
	public void Resume() {
		Time.timeScale = 1.0f;
		cameraController.UpdateSetting();
		CoroutineManager.Instance.StartAllCoroutineEx();
		//ポーズシーンを削除
		SceneManager.UnloadSceneAsync("Pause");
		isReturnFromPause = true;
		isPause = false;
	}

	private void Update() {
		if (isPause) return;

		if (Input.GetButtonDown(InputMap.GetInputName(InputMap.Type.StartButton)) &&
				!Fade.Instance().isFading &&
				!SceneChanger.Instance().isChanging &&
				!playerStatus.IsDead()) {
			Pause();
		}
	}

	public static PauseManager GetInstance() {
		return GameObject.FindGameObjectWithTag(TagName.PauseManager.String()).GetComponent<PauseManager>();
	}
}