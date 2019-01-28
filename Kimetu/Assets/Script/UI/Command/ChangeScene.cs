using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour, IExecuteCommand {
	[SerializeField]
	private SceneName sceneName;

	[SerializeField]
	private FadeData fade;

	public IEnumerator OnExecute() {
		SceneChanger.Instance().Change(sceneName, fade);
		while(true) {
			yield return null;
		}
	}
}
