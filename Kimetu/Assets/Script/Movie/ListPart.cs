using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListPart : MoviePart {
	[SerializeField]
	private MoviePart[] parts;

	public override IEnumerator MovieUpdate() {
		foreach(var part in parts) {
			yield return part.MovieUpdate();
		}
	}
}
