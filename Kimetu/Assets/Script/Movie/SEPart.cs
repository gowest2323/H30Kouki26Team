using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPart : MoviePart {
	[SerializeField]
	private AudioName audio;

	public override IEnumerator MovieUpdate() {
		AudioManager.Instance.PlayPlayerSE(audio.String());
		yield break;
	}
}
