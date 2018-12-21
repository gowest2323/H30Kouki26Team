using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOptionCommand : MonoBehaviour, IExecuteCommand {
	[SerializeField]
	private GameObject optionUIPrefab;

	private GameObject optionUIObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator OnExecute() {
		this.optionUIObj = GameObject.Instantiate(optionUIPrefab);
		var waitOne = new WaitForSecondsRealtime(0.1f);
		yield return waitOne;
		while(true) {
			if(Input.GetButtonDown(InputMap.Type.AButton.GetInputName())) {
				break;
			}
			yield return waitOne;
		}
		GameObject.Destroy(optionUIObj);
	}
}
