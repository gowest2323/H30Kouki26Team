using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RengekiPushEvent {
	public int pushMaxCount { private set; get; }
	public int pushCurrentCount { private set; get; }
	public float parcent { get { return ((float)pushCurrentCount / (float)pushMaxCount); } }

	public RengekiPushEvent(int pushMaxCount, int pushCurrentCount) {
		this.pushMaxCount = pushMaxCount;
		this.pushCurrentCount = pushCurrentCount;
	}
}

public class Rengeki : MonoBehaviour {
	[SerializeField]
	private int pushMaxCount = 20;

	private bool triggered;
	private int pushCurrentCount;

	public IObservable<RengekiPushEvent> onPush { get { return push; } }
	private Subject<RengekiPushEvent> push;

	private System.IDisposable startObserver;
	private System.IDisposable endObserver;

	// Use this for initialization
	void Start () {
		this.push = new Subject<RengekiPushEvent>();
		this.startObserver = Slow.Instance.onStart.Subscribe(OnSlowStart);
		this.endObserver = Slow.Instance.onEnd.Subscribe(OnSlowStart);
	}
	
	// Update is called once per frame
	void Update () {
		if(!Slow.Instance.isSlowNow) {
			return;
		}
		//押してない || もう発動中
		if(!Input.GetButtonDown(InputMap.Type.XButton.GetInputName()) ||
			pushCurrentCount >= pushMaxCount) {
			return;
		}
		pushCurrentCount++;
		push.OnNext(new RengekiPushEvent(pushMaxCount, pushCurrentCount));
		if(pushCurrentCount >= pushMaxCount) {
			Debug.Log("rengeki");
		}
	}

	private void OnDestroy() {
		startObserver.Dispose();
		endObserver.Dispose();
	}

	private void OnSlowStart(bool b) {
		this.pushCurrentCount = 0;
	}

	private void OnSlowEnd(bool b) {

	}
}
