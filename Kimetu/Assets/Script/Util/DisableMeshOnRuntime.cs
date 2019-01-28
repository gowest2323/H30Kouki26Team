using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMeshOnRuntime : MonoBehaviour {
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		if(meshRenderer == null) {
			this.meshRenderer = GetComponent<MeshRenderer>();
		}
		GameObject.Destroy(meshRenderer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
