//AUTO-GENERATED-CODE
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

[DisallowMultipleComponent]
public class KioniIdleHook : MonoBehaviour, IEventHook {
	public bool animationNow { private set; get; }
	public IObservable<bool> trigger { get { return subject; }}
	private Subject<bool> subject;

	void Awake() {
		this.subject = new Subject<bool>();
	}

	void Start() {
	}

	public void KioniIdleStart() {
		Debug.Log("KioniIdleStart");
		this.animationNow = true;
		subject.OnNext(true);
	}

	public void KioniIdleEnd() {
		Debug.Log("KioniIdleEnd");
		this.animationNow = false;
		subject.OnNext(false);
	}

	public IEnumerator Wait() {
		yield return new WaitWhile(() => !animationNow);
		yield return new WaitWhile(() => animationNow);
	}
}
