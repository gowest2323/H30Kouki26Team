using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChangeSceneAtFinishedTimeLine : MonoBehaviour {
	[SerializeField]
	private PlayableDirector director;

	[SerializeField]
	private SceneName scene;

	[SerializeField]
	private FadeData fadeData;

	[SerializeField]
	private float addWait = 1.0f;

	private bool triggered;

	// Use this for initialization
	void Start () {
		director.stopped += (e) => {
			if (!triggered) {
				triggered = true;
				StartCoroutine(ChangeScene());
			}
		};
	}
	
	// Update is called once per frame
	void Update () {
	}

	private IEnumerator ChangeScene() {
		yield return new WaitForSeconds(addWait);
		SceneChanger.Instance().Change(scene, fadeData);
	}
}
