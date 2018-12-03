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

	private void Start() {
		pauseManager = GameObject.FindObjectOfType<PauseManager>();
	}

	private void Update() {
		if (visibleControlInfo && Input.GetButton(InputMap.Type.AButton.GetInputName())) {
			this.visibleControlInfo = false;
			controlInfoPanel.SetActive(false);
			SetEnabledButtons(true);
		}
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
