using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PointerUIがマーカーの位置を決めるのに使用します。
/// </summary>
public class PointerTarget : MonoBehaviour {
	[SerializeField]
	public Transform pointerPosition;

	// Use this for initialization
	void Start () {
		if(pointerPosition == null) {
			pointerPosition = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
