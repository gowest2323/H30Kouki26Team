﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayDataCommand : MonoBehaviour, IExecuteCommand {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator OnExecute() {
		StageDataPrefs.DeleteAll();
		yield break;
	}
}
