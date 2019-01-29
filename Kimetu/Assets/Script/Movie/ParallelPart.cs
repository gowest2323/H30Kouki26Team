using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelPart : MoviePart {
	[SerializeField]
	private MoviePart primary;

	[SerializeField]
	private MoviePart[] asyncParts;

	public override IEnumerator MovieUpdate() {
		foreach (var part in asyncParts) {
			StartCoroutine(part.MovieUpdate());
		}

		yield return primary.MovieUpdate();
	}

}
