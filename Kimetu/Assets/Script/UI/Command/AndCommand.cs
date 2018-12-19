using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndCommand : MonoBehaviour, IExecuteCommand {
	[SerializeField]
	private GameObject first;

	[SerializeField]
	private GameObject second;

	private IExecuteCommand firstCmd, secondCmd;

	// Use this for initialization
	void Start () {
		this.firstCmd = first.GetComponent<IExecuteCommand>();
		this.secondCmd = second.GetComponent<IExecuteCommand>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnExecute() {
		firstCmd.OnExecute();
		secondCmd.OnExecute();
	}
}
