using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeFromLastPlay : MonoBehaviour, IExecuteCommand {

	[SerializeField]
	private FadeData fade;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnExecute() {
		StageManager.Resume(fade);
	}
}
