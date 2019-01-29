using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPart : MoviePart {
	[SerializeField]
	private GameObject target;
	[SerializeField]
	private GameObject[] otherObjects;
	[SerializeField]
	private GameObject point;
	[SerializeField]
	private float seconds = 1.0f;

	public override IEnumerator MovieUpdate() {
		if (target == null) {
			target = Camera.main.gameObject;
		}

		var pointPos = point.transform.position;
		var pos = target.transform.position;
		pointPos.y = pos.y;
		var startRotation = target.transform.rotation;
		var endRotation = Quaternion.LookRotation((pointPos - pos).normalized);
		//他のオブジェクトの
		var startRotationList = new Quaternion[otherObjects.Length];
		var endRotationList = new Quaternion[otherObjects.Length];

		for (int i = 0; i < otherObjects.Length; i++) {
			startRotationList[i] = otherObjects[i].transform.rotation;
		}

		for (int i = 0; i < otherObjects.Length; i++) {
			pos = otherObjects[i].transform.position;
			pointPos.y = pos.y;
			endRotationList[i] = Quaternion.LookRotation((pointPos - pos).normalized);
		}

		var offset = 0f;

		while (offset < seconds) {
			var t = Time.time;
			yield return null;
			offset += (Time.time - t);
			target.transform.rotation = Quaternion.Slerp(startRotation, endRotation, offset / seconds);

			for (int i = 0; i < otherObjects.Length; i++) {
				Debug.Log(otherObjects[i].name);
				otherObjects[i].transform.rotation = Quaternion.Slerp(startRotationList[i], endRotationList[i], offset / seconds);
			}
		}
	}
}
