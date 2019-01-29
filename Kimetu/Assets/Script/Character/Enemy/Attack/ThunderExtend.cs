using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderExtend : MonoBehaviour {
	[SerializeField]
	private string startObjectName = "LightningStart";
	[SerializeField]
	private string endObjectName = "LightningEnd";
	private Transform start;
	private Transform end;
	// Use this for initialization
	void Awake () {
		start = transform.Find(startObjectName);
		end = transform.Find(endObjectName);
	}


	public void Init() {
		end.transform.position = Vector3.zero;
	}

	public void Extend(float length) {
		Vector3 pos = start.transform.localPosition;
		pos.x += length;
		end.transform.localPosition = pos;
	}
}
