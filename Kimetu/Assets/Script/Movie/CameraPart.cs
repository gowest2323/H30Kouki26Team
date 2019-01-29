using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraPart : MoviePart {
	[SerializeField]
	protected GameObject targetCamera;
	// Use this for initialization
	protected override void Start() {
		if (targetCamera == null) {
			this.targetCamera = Camera.main.gameObject;
		}
	}

	public override abstract IEnumerator MovieUpdate();

}
