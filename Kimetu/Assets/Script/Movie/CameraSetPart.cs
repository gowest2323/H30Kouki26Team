using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetPart : CameraPart {
	[SerializeField]
	private GameObject point;

	public override IEnumerator MovieUpdate() {
		targetCamera.transform.position = point.transform.position;
		yield break;
	}
}
