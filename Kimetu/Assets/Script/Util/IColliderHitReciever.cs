using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColliderHitReciever {
	void RecieveOnTriggerEnter(Collider other);

	void RecieveOnTriggerExit(Collider other);
}