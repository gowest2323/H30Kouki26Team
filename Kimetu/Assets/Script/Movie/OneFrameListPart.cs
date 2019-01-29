using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneFrameListPart : MoviePart {
	[SerializeField]
	private MoviePart[] parts;

	public override IEnumerator MovieUpdate() {
		foreach (var part in parts) {
			var enumer = part.MovieUpdate();

			while (enumer.MoveNext()) {
				//ここでWaitForSecondsやnullを無視する
				;
			}
		}

		yield return null;
	}

}
