using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookPart : CameraPart {
	[SerializeField]
	private GameObject point;

	public override IEnumerator MovieUpdate() {
		targetCamera.transform.LookAt(point.transform);
		yield break;
	}
}
