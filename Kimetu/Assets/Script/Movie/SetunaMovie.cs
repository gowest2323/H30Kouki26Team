using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ラスボス前のムービー。
/// </summary>
public class SetunaMovie : MonoBehaviour {
	[SerializeField]
	private MoviePart[] parts;

	// Use this for initialization
	void Start () {
		StartCoroutine(MovieUpdate());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator MovieUpdate() {
		foreach(var part in parts) {
			if(part == null) {
				continue;
			}
			yield return part.MovieUpdate();
		}
	}
}
