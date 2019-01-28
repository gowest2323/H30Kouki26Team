using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEnemyActionable {
	IEnumerator Action(UnityAction callBack);
}
