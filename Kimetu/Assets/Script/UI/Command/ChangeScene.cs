using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour, IExecuteCommand {
	[SerializeField]
	private SceneName sceneName;

	[SerializeField]
	private FadeData fade;

	public void OnExecute() {
		SceneChanger.Instance().Change(sceneName, fade);
	}
}
