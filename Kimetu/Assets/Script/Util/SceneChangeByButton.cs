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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton(type.GetInputName())) {
			SceneChanger.Instance().Change(sceneName, new FadeData(1f, 1f, Color.black));
		}
	}
}
