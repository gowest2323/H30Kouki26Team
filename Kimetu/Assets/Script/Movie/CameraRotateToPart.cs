using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateToPart : CameraPart {
	[SerializeField]
	private GameObject point;
	[SerializeField]
	private float seconds = 0.5f;

	public override IEnumerator MovieUpdate() {
		var pos = targetCamera.transform.position;
		var dir = (point.transform.position - pos).normalized;
		var startRotation = targetCamera.transform.rotation;
		var endRotation = Quaternion.LookRotation(dir);
		var offset = 0f;
		while(offset < seconds) {
			var t = Time.time;
			yield return null;
			offset += (Time.time - t);
			targetCamera.transform.rotation = Quaternion.Slerp(startRotation, endRotation, offset / seconds);
		}
	}
}
