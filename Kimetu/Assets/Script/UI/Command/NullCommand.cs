using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullCommand : MonoBehaviour, IExecuteCommand {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnExecute() {
		Application.Quit();
	}
}
