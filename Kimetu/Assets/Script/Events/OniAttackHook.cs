﻿//AUTO-GENERATED-CODE
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

[DisallowMultipleComponent]
public class OniAttackHook : MonoBehaviour {
	public bool animationNow { private set; get; }
	public IObservable<bool> trigger { get { return subject; }}
	private Subject<bool> subject;

	void Start() {
		this.subject = new Subject<bool>();
	}

	void End() {
	}

	public void OniAttackStart() {
		Debug.Log("OniAttackStart");
		this.animationNow = true;
		subject.OnNext(true);
	}

	public void OniAttackEnd() {
		Debug.Log("OniAttackEnd");
		this.animationNow = false;
		subject.OnNext(false);
	}

	public IEnumerator Wait() {
		yield return new WaitWhile(() => animationNow);
	}
}
