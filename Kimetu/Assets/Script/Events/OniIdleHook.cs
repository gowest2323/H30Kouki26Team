//AUTO-GENERATED-CODE
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

[DisallowMultipleComponent]
public class OniIdleHook : MonoBehaviour {
	public bool animationNow { private set; get; }
	public IObservable<bool> trigger { get { return subject; }}
	private Subject<bool> subject;

	void Start() {
		this.subject = new Subject<bool>();
	}

	void End() {
	}

	public void OniIdleStart() {
		Debug.Log("OniIdleStart");
		this.animationNow = true;
		subject.OnNext(true);
	}

	public void OniIdleEnd() {
		Debug.Log("OniIdleEnd");
		this.animationNow = false;
		subject.OnNext(false);
	}

	public IEnumerator Wait() {
		yield return new WaitWhile(() => animationNow);
	}
}
