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

	public IEnumerator OnExecute() {
		StageManager.Resume(fade);
		yield return new WaitForSeconds(fade.fadeInTime + fade.fadeOutTime + 1);
	}
}
