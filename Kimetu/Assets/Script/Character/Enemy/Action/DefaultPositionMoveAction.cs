﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPositionMoveAction : MoveAction {
	protected override void Awake() {
		base.Awake();
		movePosition = this.transform.position;
		moveRotation = this.transform.rotation;
	}

	public bool IsDefaultPosition() {
		if(this.topTransform == null) {
			this.topTransform = GetTopTransform();
		}
		//距離が一定以上離れていたらその場所にいない
		if (Vector3.Distance(movePosition, topTransform.position) > 0.1f) { return false; }

		//角度が一定以上離れていたらデフォルトの場所にいないとする
		if (Quaternion.Angle(moveRotation, topTransform.rotation) > 0.1f) { return false; }

		return true;
	}
}
