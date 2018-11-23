//AUTO-GENERATED-CODE
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

[DisallowMultipleComponent]
public class OniRunHook : MonoBehaviour {
	public bool animationNow { private set; get; }
	public IObservable<bool> trigger { get { return subject; }}
	private Subject<bool> subject;

	void Awake() {
		this.subject = new Subject<bool>();
	}

	void Start() {
	}

	public void OniRunStart() {
		Debug.Log("OniRunStart");
		this.animationNow = true;
		subject.OnNext(true);
	}

	public void OniRunEnd() {
		Debug.Log("OniRunEnd");
		this.animationNow = false;
		subject.OnNext(false);
	}

	public IEnumerator Wait() {
		yield return new WaitWhile(() => !animationNow);
		yield return new WaitWhile(() => animationNow);
	}
}
