using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

	[SerializeField, Header("エネミーとプレイヤーの距離")]
	private float distanceLimit = 1f;

	[SerializeField, Header("エネミーに対してどれだけ回りこむか"), Range(0,360)]
	private float turnDegree = 100f;

	[SerializeField]
	private PlayerAttackSequence attackSequence;

	[SerializeField]
	private PlayerAction playerAction;

	[SerializeField]
	private PlayerAnimation playerAnimation;

	public bool moveNow { private set; get; }
	public bool turnNow { private set; get; }
	public bool actionNow { private set; get; }

	private float slowRemine;
	private bool triggered;
	private int pushCurrentCount;
	private GameObject target;

	public IObservable<RengekiPushEvent> onPush { get { return push; } }
	private Subject<RengekiPushEvent> push;

	private System.IDisposable startObserver;
	private System.IDisposable endObserver;

	//アニメーションにかかる時間
	private static readonly float STEP_LENGTH = 0.98f;
	private static readonly float ATTACK_LENGTH = 0.583f;

	// Use this for initialization
	void Awake () {
		if(playerAnimation == null) {
			this.playerAnimation = GetComponent<PlayerAnimation>();
		}
		if(attackSequence == null) {
			this.attackSequence = GetComponent<PlayerAttackSequence>();
		}
		if (playerAction == null) {
			this.playerAction = GetComponent<PlayerAction>();
		}
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
			Assert.IsTrue(!turnNow);
			Assert.IsTrue(!actionNow);
			this.slowRemine = Slow.Instance.GetWaitSeconds() - Slow.Instance.elapsed;
			if (slowRemine > 0 && target != null) {
				StartCoroutine(RengekiUpdate());
			}
		}
	}

	private void OnDestroy() {
		startObserver.Dispose();
		endObserver.Dispose();
	}

	private IEnumerator RengekiUpdate() {
		yield return MoveToEnemy();
		yield return TurnToEnemyBack();
		yield return AutoAction();
	}

	private IEnumerator MoveToEnemy() {
		var distance = Utilities.DistanceXZ(transform.position, target.transform.position);
		Debug.Log("distance:" + distance);
		if(distance < distanceLimit) {
			yield break;
		}
		this.moveNow = true;
		var moveDistance = (distanceLimit - distance);
		var dirToEnemy = (transform.position - target.transform.position).normalized;
		var offset = 0f;
		var start = transform.position;
		var end = start + (dirToEnemy * moveDistance);
		var moveSeconds = (slowRemine / 10f);
		while(offset < moveSeconds) {
			var t = Time.time;
			yield return null;
			var diff = (Time.time - t) * Slow.Instance.GetPlayerSpeed();
			offset += diff;
			var parcent = Mathf.Clamp01(offset / moveSeconds);
			transform.position = start + (dirToEnemy * (moveDistance * parcent));
		}
		transform.position = end;
		this.moveNow = false;
	}

	private IEnumerator TurnToEnemyBack() {
		this.turnNow = true;
		var beforeSpeed = playerAnimation.speed;
		var turnSeconds = (slowRemine / 10f);
		//アニメーションの長さと実際に回転にかける速度が違いすぎる場合には
		//アニメーションの速度の方で補正をかける
		var animDiff = Mathf.Abs(STEP_LENGTH) - Mathf.Abs(turnSeconds);
		if(animDiff > 0.1) {
			playerAnimation.speed = STEP_LENGTH / turnSeconds;
			//playerAnimation.speed = STEP_LENGTH * (turnSeconds / STEP_LENGTH);
		}
		playerAnimation.StartRengekiAnimation();
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
			//壁に当たったら終了する
			RaycastHit hit;
			if(Physics.Raycast(transform.position + Vector3.up, dirv, out hit, 1f, LayerMask.GetMask(LayerName.Stage.String()))) {
				break;
			}
		}
		playerAnimation.speed = Slow.Instance.GetPlayerSpeed();
		this.turnNow = false;
		Debug.Log("end turn");
	}

	private IEnumerator AutoAction() {
		//アクションに使える残りの時間
		var actionTime = Slow.Instance.GetWaitSeconds() - Slow.Instance.elapsed;
		if(actionTime < 0) {
			playerAction.Avoid(Vector3.zero);
			yield break;
		}
		//残り時間 / 4 = アクション一回あたりの時間
		var actionOne = actionTime / 4f;
		var animDiff = Mathf.Abs(ATTACK_LENGTH) - Mathf.Abs(actionOne);
		if((ATTACK_LENGTH > actionOne) && animDiff > 0.1f) {
			playerAnimation.speed = ATTACK_LENGTH / actionOne;
		}
		Debug.Log("actionTime:" + actionTime);
		Debug.Log("actionOne:" + actionOne);
		Debug.Log("speed:" + playerAnimation.speed);
		this.actionNow = true;
		for(int i=0; i<4; i++) {
			//ダメージを受けたら中断
			if(playerAction.state == PlayerState.Damage) {
				break;
			}
			playerAnimation.StartAttackAnimation(i);
			yield return new WaitForSeconds(actionOne);
			//yield return playerAnimation.WaitAnimation("kaede", "attack" + (i + 1));
			//yield return new WaitForSeconds(ATTACK_LENGTH * (1 / Slow.Instance.GetPlayerSpeed()));
		}
		//yield return playerAnimation.WaitAnimation("kaede", "attack4_idle");
		yield return new WaitForSeconds(actionOne);
		playerAnimation.speed = Slow.Instance.GetPlayerSpeed();
		playerAction.Avoid(Vector3.zero);
		this.actionNow = false;
	}

	private void OnSlowStart(bool b) {
		this.pushCurrentCount = 0;
	}

	private void OnSlowEnd(bool b) {
		this.pushCurrentCount = 0;
	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(Rengeki))]
public class RengekiEditor : Editor {
	private Rengeki self;

	private void OnEnable() {
		this.self = target as Rengeki;
	}
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.HelpBox("`TurnSeconds`, `MoveSeconds`はスロー中はこの三倍になります", MessageType.Info);
	}
}
#endif