using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActivePart : MoviePart {
	[SerializeField]
	private GameObject target;

	[SerializeField]
	private bool active;

	public override IEnumerator MovieUpdate() {
		target.SetActive(active);
		yield break;
	}
}
