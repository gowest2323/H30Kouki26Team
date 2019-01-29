using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

public class OpeningMovie : MonoBehaviour {
	[SerializeField, Header("ムービーを終了するボタン")]
	private InputMap.Type[] forcedTerminationButtons;
	private ChangeScene changeScene;
	[SerializeField]
	private PlayableDirector playableDirector;
	private bool isEnd; //終了しているか

	// Use this for initialization
	void Start() {
		changeScene = GetComponent<ChangeScene>();
		Assert.IsNotNull(changeScene, "ChangeSceneが存在しません。");
		//ムービー終了時のコールバック
		playableDirector.stopped += MovieStop;
		isEnd = false;
	}

	/// <summary>
	/// ムービー終了時のコールバック
	/// </summary>
	/// <param name="obj"></param>
	private void MovieStop(PlayableDirector obj) {
		if (obj == playableDirector) {
			ChangeNextScene();
		}
	}

	// Update is called once per frame
	void Update() {
		foreach (var button in forcedTerminationButtons) {
			if (Input.GetButtonDown(button.GetInputName())) {
				ChangeNextScene();
				break;
			}
		}
	}

	/// <summary>
	/// シーン変更
	/// </summary>
	private void ChangeNextScene() {
		if (isEnd) return;

		isEnd = true;
		StartCoroutine(changeScene.OnExecute());
	}
}
