using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPart : MoviePart {
	[SerializeField]
	private GameObject target;
	[SerializeField]
	private GameObject point;
	[SerializeField]
	private float seconds = 1.0f;
	//FIXME:CameraRotateToのコピペ
	public override IEnumerator MovieUpdate() {
		var selfPos = target.transform.position;
		var targetPos = point.transform.position;
		targetPos.y = selfPos.y;
		var dir = (targetPos - selfPos).normalized;
		var startRotation = target.transform.rotation;
		var endRotation = Quaternion.LookRotation(dir);
		var offset = 0f;
		while (offset < seconds) {
			var t = Time.time;
			yield return null;
			offset += (Time.time - t);
			target.transform.rotation = Quaternion.Slerp(startRotation, endRotation, offset / seconds);
		}
	}
}
