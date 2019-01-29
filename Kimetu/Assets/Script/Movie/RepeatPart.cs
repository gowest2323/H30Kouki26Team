using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatPart : MoviePart {
	[SerializeField]
	private int count;

	[SerializeField]
	private MoviePart part;

	public override IEnumerator MovieUpdate() {
		for (int i = 0; i < count; i++) {
			yield return part.MovieUpdate();
		}
	}
}
