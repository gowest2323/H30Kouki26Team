using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitMovie : MoviePart {
	[SerializeField]
	private float seconds = 1.0f;

	public override IEnumerator MovieUpdate() {
		yield return new WaitForSeconds(seconds);
	}
}
