﻿//AUTO-GENERATED-CODE
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

[DisallowMultipleComponent]
public class OniHitHook : MonoBehaviour {
	public bool animationNow { private set; get; }
	public IObservable<bool> trigger { get { return subject; }}
	private Subject<bool> subject;

	void Awake() {
		this.subject = new Subject<bool>();
	}

	void Start() {
	}

	public void OniHitStart() {
		Debug.Log("OniHitStart");
		this.animationNow = true;
		subject.OnNext(true);
	}

	public void OniHitEnd() {
		Debug.Log("OniHitEnd");
		this.animationNow = false;
		subject.OnNext(false);
	}

	public IEnumerator Wait() {
		yield return new WaitWhile(() => animationNow);
	}
}
