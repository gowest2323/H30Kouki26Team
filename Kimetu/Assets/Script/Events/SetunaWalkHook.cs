﻿//AUTO-GENERATED-CODE
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

[DisallowMultipleComponent]
public class SetunaWalkHook : MonoBehaviour, IEventHook {
	public bool animationNow { private set; get; }
	public IObservable<bool> trigger { get { return subject; }}
	private Subject<bool> subject;

	void Awake() {
		this.subject = new Subject<bool>();
	}

	void Start() {
	}

	public void SetunaWalkStart() {
		Debug.Log("SetunaWalkStart");
		this.animationNow = true;
		subject.OnNext(true);
	}

	public void SetunaWalkEnd() {
		Debug.Log("SetunaWalkEnd");
		this.animationNow = false;
		subject.OnNext(false);
	}

	public IEnumerator Wait() {
		yield return new WaitWhile(() => !animationNow);
		yield return new WaitWhile(() => animationNow);
	}
}
