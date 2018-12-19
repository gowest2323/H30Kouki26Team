using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitCommand : MonoBehaviour, IExecuteCommand {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public CommandResult OnExecute() {
		Application.Quit();
		return CommandResult.Terminate;
	}
}
