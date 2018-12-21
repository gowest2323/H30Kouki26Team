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
		yield return new WaitForSeconds(fade.fadeInTime + fade.fadeOutTime + 1);
	}
}
