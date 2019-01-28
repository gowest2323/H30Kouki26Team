using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePart : MoviePart {
	[SerializeField]
	private GameObject forward;
	[SerializeField]
	private GameObject[] targets;
	[SerializeField]
	private float distance = 2.2f;
	[SerializeField]
	private float seconds = 5f;
	[SerializeField]
	private MoviePart asyncNestMovie;

	public override IEnumerator MovieUpdate() {
		var dir = forward.transform.forward;
		var startPosList = new Vector3[targets.Length];
		var endPosList = new Vector3[targets.Length];
		for(int i=0; i<startPosList.Length; i++) {
			startPosList[i] = targets[i].transform.position;
		}
		for(int i=0; i<endPosList.Length; i++) {
			endPosList[i] = targets[i].transform.position + (targets[i].transform.forward * distance);
		}
		var offset = 0f;
		while(offset < seconds) {
			var t = Time.time;
			yield return null;
			offset += (Time.time - t);
			if(!AudioManager.Instance.IsPlayingPlayerSE()) {
				AudioManager.Instance.PlayPlayerSE(AudioName.Walk_Short.String());
			}

			for (int i=0; i<targets.Length; i++) {
				var start = startPosList[i];
				var end = endPosList[i];
				targets[i].transform.position = Vector3.Lerp(start, end, offset / seconds);
			}
		}
	}
}
