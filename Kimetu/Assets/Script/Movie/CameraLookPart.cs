using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookPart : MoviePart {
	[SerializeField]
	private GameObject targetCamera;

	[SerializeField]
	private GameObject point;
	protected override void Start() {
		base.Start();
		if (targetCamera == null) {
			this.targetCamera = Camera.main.gameObject;
		}
	}

	public override IEnumerator MovieUpdate() {
		targetCamera.transform.LookAt(point.transform);
		yield break;
	}
}
