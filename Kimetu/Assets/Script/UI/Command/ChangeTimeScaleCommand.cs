using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeScaleCommand : MonoBehaviour, IExecuteCommand {
	[SerializeField]
	private float timeScale = 1.0f;

	public void OnExecute() {
		Time.timeScale = timeScale;
	}
}
