using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGizmo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos() {
		var color = Gizmos.color;
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(transform.position, transform.localScale);
		Gizmos.color = color;
	}
}
