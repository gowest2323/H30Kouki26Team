using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateIfDebug : MonoBehaviour {
	[SerializeField]
	private GameObject prefab;

	// Use this for initialization
	void Start () {
#if DEBUG
		GameObject.Instantiate(prefab);
#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
