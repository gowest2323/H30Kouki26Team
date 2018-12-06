using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScene : MonoBehaviour {
	[SerializeField]
	private FadeData changeTitleFade;
	private PauseManager pauseManager;
	[SerializeField]
	private Button[] buttons;

	[SerializeField]
	private GameObject controlInfoPanel;

	private bool visibleControlInfo;
	private int changeScene;

	private void Start() {
		pauseManager = GameObject.FindObjectOfType<PauseManager>();
	}

	private void Update() {
		ChangeScene();
	}

	private void ChangeScene() {
		if (!visibleControlInfo || !Input.GetButtonDown(InputMap.Type.AButton.GetInputName())) {
			return;
		}

		if (changeScene == 0) {
			StartCoroutine(LateChangeScene());
		}
	}

	private IEnumerator LateChangeScene() {
		this.changeScene++;
		var listUI = controlInfoPanel.GetComponent<ListUI>();

		if (listUI.selected == 0) {
			//戻る
			controlInfoPanel.SetActive(false);
			SetEnabledButtons(true);
			//遅延させないとすぐに戻ってしまう
			yield return new WaitForSecondsRealtime(1f);
			this.visibleControlInfo = false;
		} else if (listUI.selected == 1) {
			//タイトルへ
			Time.timeScale = 1;
			SceneChanger.Instance().Change(SceneName.Title, new FadeData(1, 1, Color.black));
		}

		this.changeScene--;
	}

	/// <summary>
	/// ゲームに戻る
	/// </summary>
	public void ReturnGame() {
		pauseManager.Resume();
	}

	/// <summary>
	/// 操作説明表示
	/// </summary>
	public void PrintOperationDescription() {
		if (visibleControlInfo) { return; }

		SetEnabledButtons(false);
		controlInfoPanel.SetActive(true);
		this.visibleControlInfo = true;
	}

	/// <summary>
	/// タイトルへ戻る
	/// </summary>
	public void ToTitle() {
		//シーンの切り替えにコルーチンを利用しているためtimescaleを戻す
		Time.timeScale = 1.0f;
		SceneChanger.Instance().Change(SceneName.Title, changeTitleFade);
	}

	private void SetEnabledButtons(bool b) {
		foreach (var btn in buttons) {
			btn.interactable = b;
		}
	}
}
