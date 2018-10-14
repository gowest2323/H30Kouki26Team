using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDemo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton(InputMap.Type.AButton.GetInputName())) {
			Debug.Log("a button");
		}
		if(Input.GetButton(InputMap.Type.BButton.GetInputName())) {
			Debug.Log("b button");
		}
		if(Input.GetButton(InputMap.Type.XButton.GetInputName())) {
			Debug.Log("x button");
		}
		if(Input.GetButton(InputMap.Type.YButton.GetInputName())) {
			Debug.Log("y button");
		}
	}
}
