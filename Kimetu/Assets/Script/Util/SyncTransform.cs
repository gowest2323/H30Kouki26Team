using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTransform : MonoBehaviour {
    [SerializeField]
    private GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
	}
}
