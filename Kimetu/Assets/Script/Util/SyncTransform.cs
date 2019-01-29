using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTransform : MonoBehaviour {
	private GameObject target;

	// Use this for initialization
	void Start () {
		this.target = Camera.main.gameObject;
	}

	// Update is called once per frame
	void Update () {
		transform.position = target.transform.position;
		transform.rotation = target.transform.rotation;
	}
}
