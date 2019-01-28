//AUTO-GENERATED-CODE
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public interface IEventHook {
	bool animationNow { get; }
	IObservable<bool> trigger { get; }
	IEnumerator Wait();
}