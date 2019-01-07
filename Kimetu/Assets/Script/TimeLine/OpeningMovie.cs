using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningMovie : MonoBehaviour {
	[SerializeField]
	private InputMap.Type[] forcedTerminationButtons;
	private ChangeScene changeScene;
	// Use this for initialization
	void Start() {
		changeScene = GetComponent<ChangeScene>();
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

	private void ChangeNextScene() {
		StartCoroutine(changeScene.OnExecute());
	}
}
