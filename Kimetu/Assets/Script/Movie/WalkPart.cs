using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPart : MoviePart {
	[SerializeField]
	private GameObject target;

	[SerializeField]
	private GameObject walkTo;

	[SerializeField]
	private float seconds = 3f;

	public override IEnumerator MovieUpdate() {
		var enemyAnimation = target.GetComponent<EnemyAnimation>();
		var offset = 0f;
		var start = target.transform.position;
		var end = walkTo.transform.position;
		enemyAnimation.StartRunAnimation();
		while(offset < seconds) {
			var t = Time.time;
			yield return null;
			offset += (Time.time - t);
			target.transform.position = Vector3.Lerp(start, end, offset / seconds);
		}
		target.transform.position = end;
		enemyAnimation.StopRunAnimation();
	}
}
