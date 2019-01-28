using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ムービーの一部分。
/// </summary>
public abstract class MoviePart : MonoBehaviour {

	// Use this for initialization
	protected virtual void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

	public abstract IEnumerator MovieUpdate();
}
