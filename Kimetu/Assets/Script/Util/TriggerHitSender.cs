using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コライダーのトリガーの衝突通知
/// </summary>
public class TriggerHitSender : MonoBehaviour {
	//interfaceはインスペクターから編集できないため使用側がrecieverにthisをセットする
	public IColliderHitReciever reciever { get; set; }

	private void OnTriggerEnter(Collider other) {
		reciever.RecieveOnTriggerEnter(other);
	}

	private void OnTriggerExit(Collider other) {
		reciever.RecieveOnTriggerExit(other);
	}
}
