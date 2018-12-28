using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : MonoBehaviour {
	[SerializeField]
	private GameObject terminalPrefab;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		GameObject.Instantiate(terminalPrefab);
#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
