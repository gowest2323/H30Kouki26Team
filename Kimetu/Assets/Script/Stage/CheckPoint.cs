using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CheckPoint : MonoBehaviour {
	[SerializeField]
	private StageManager stageManager;
	public void OnTriggerEnter(Collider collider) {
		stageManager.Pass(transform.position);
	}
}