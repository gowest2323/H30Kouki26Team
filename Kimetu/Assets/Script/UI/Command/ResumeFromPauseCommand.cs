using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeFromPauseCommand : MonoBehaviour, IExecuteCommand {
	private PauseManager pauseManager;

	// Use this for initialization
	void Start () {
		this.pauseManager = GameObject.FindGameObjectWithTag(TagName.PauseManager.String()).GetComponent<PauseManager>();
	}

	// Update is called once per frame
	void Update () {

	}

	public IEnumerator OnExecute() {
		pauseManager.Resume();
		yield return new WaitForEndOfFrame();
	}
}
