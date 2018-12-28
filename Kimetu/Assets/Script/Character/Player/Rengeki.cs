using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Assertions;

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
	[SerializeField, Header("何回押したら開始するか")]
	private int pushMaxCount = 20;

	[SerializeField, Header("回りこむのにかかる秒数")]
	private float turnSeconds = 0.8f;

	[SerializeField, Header("エネミーに対してどれだけ回りこむか"), Range(0,360)]
	private float turnDegree = 100f;

	public bool turnNow { private set; get; }

	private bool triggered;
	private int pushCurrentCount;
	private GameObject target;

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
			this.target = Utilities.SearchMostNearEnemyInTheRange(transform.position, 5.0f, false);
			Assert.IsTrue(target != null);
			StartCoroutine(RengekiUpdate());
		}
	}

	private void OnDestroy() {
		startObserver.Dispose();
		endObserver.Dispose();
	}

	private IEnumerator RengekiUpdate() {
		yield return TurnToEnemyBack();
	}

	private IEnumerator TurnToEnemyBack() {
		this.turnNow = true;
		//円の中心をプレイヤーとする
		var center = target.transform.position;
		var distance = Utilities.DistanceXZ(center, transform.position);
		//自分からプレイヤーへの線が難度であるかをここで取得する
		var dirToCenter = center - transform.position;
		var radian = Mathf.Atan2(dirToCenter.z, dirToCenter.x) + (180f * Mathf.Deg2Rad);
		//一定時間で補完する
		var start = transform.position;
		var offset = 0f;
		while (offset < turnSeconds) {
			var t = Time.time;
			yield return null;
			var diff = (Time.time - t) * Slow.Instance.GetPlayerSpeed();
			offset += diff;
			//最初にいた時点での角度 + 移動する角度
			var parcent = Mathf.Clamp01(offset / turnSeconds);
			var dirx = Mathf.Cos(radian + ((turnDegree * parcent) * Mathf.Deg2Rad));
			var dirz = Mathf.Sin(radian + ((turnDegree * parcent) * Mathf.Deg2Rad));
			var dirv = new Vector3(dirx, 0, dirz);
			transform.LookAt(target.transform.position);
			transform.position = center + (dirv * distance);
		}
		this.turnNow = false;
	}


	private void OnSlowStart(bool b) {
		this.pushCurrentCount = 0;
	}

	private void OnSlowEnd(bool b) {

	}
}
