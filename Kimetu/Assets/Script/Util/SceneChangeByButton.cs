using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタン入力によってシーンを変更するスクリプト。
/// </summary>
public class SceneChangeByButton : MonoBehaviour {
	[SerializeField]
	private InputMap.Type type;

	[SerializeField]
	private SceneName sceneName;

	private bool change;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (change) { return; }

		if (Input.GetButton(type.GetInputName())) {
			this.change = true;
			SceneChanger.Instance().Change(sceneName, new FadeData(1f, 1f, Color.black));
		}
	}
}
