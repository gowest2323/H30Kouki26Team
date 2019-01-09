using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirinuke : MonoBehaviour {
	[SerializeField, Header("攻撃力")]
	private int power = 20;

	[SerializeField, Header("敵の方にむくのにかかる時間")]
	private float rotateSeconds = 0.25f;

	[SerializeField, Header("近付くのにかかる時間")]
	private float moveSeconds = 0.5f;

	[SerializeField, Header("プレイヤーから敵へのベクトルをどれだけずらすか"), Range(0f, 360f)]
	private float adjustDegree = 45f;

	[SerializeField, Header("敵をどれだけ通り過ぎるか")]
	private float distanceToEnemy = 1.5f;

	[SerializeField]
	private PlayerAnimation playerAnimation;

	[SerializeField]
	private Sword sword;

	public bool isRunning { private set; get; }
	public GameObject target { private set; get; }

	// Use this for initialization
	void Start () {
		if(playerAnimation == null) {
			this.playerAnimation = GetComponent<PlayerAnimation>();
		}
		if(sword == null) {
			this.sword = GetComponentInChildren<Sword>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartKirinuke() {
		if(isRunning) {
			return;
		}
		StartCoroutine(KirinukeUpdate());
	}

	private bool FindNearEnemy(out GameObject outObj) {
		var ret = Utilities.SearchMostNearEnemyInTheRange(transform.position, 5.0f, false);
		outObj = ret;
		return ret != null;
	}

	private IEnumerator KirinukeUpdate() {
		this.isRunning = true;
		GameObject enemy;
		if(FindNearEnemy(out enemy)) {
			this.target = enemy;
			yield return Back();
			sword.ChangePower(power);
			playerAnimation.StartKirinukeAnimation();
			sword.AttackStart();
			yield return MoveToEnemy();
			sword.AttackEnd();
			playerAnimation.StopKirinukeAnimation();
			sword.ResetPower();
			this.isRunning = false;
		} else {
			this.isRunning = false;
		}
	}

	private IEnumerator Back() {
		var targetPos = target.transform.position;
		var selfPos = transform.position;
		var dir = (targetPos - selfPos);
		var distance = Utilities.DistanceXZ(targetPos, selfPos);
		var LIMIT_DISTANCE = 2.5f;
		//一定時間で敵の方をむく
		var moveDistance = LIMIT_DISTANCE - distance;
		var offset = 0f;
		var startPos = transform.position;
		var endPos = startPos + (-dir * moveDistance);
		var startRot = transform.rotation;
		var endRot = Quaternion.LookRotation(dir);
		if(Quaternion.Angle(startRot, endRot) < 1f) {
			yield break;
		}
		while (offset < rotateSeconds) {
			var t = Time.time;
			yield return new WaitForEndOfFrame();
			var diff = (Time.time - t) * Slow.Instance.GetPlayerSpeed();
			offset += diff;
			var parc = (offset / rotateSeconds);
			transform.rotation = Quaternion.Slerp(startRot, endRot, parc);
			if(distance < LIMIT_DISTANCE) {
				//transform.position = Vector3.Lerp(startPos, endPos, parc);
			}
		}
		//transform.position = endPos;
	}

	private IEnumerator MoveToEnemy() {
		var dir = Quaternion.Euler(0, -adjustDegree, 0) * (target.transform.position - transform.position).normalized;
		var dist = Utilities.DistanceXZ(target.transform.position, transform.position) + distanceToEnemy;
		var start = transform.position;
		var end = start + (dir * dist);
		var offset = 0f;
		var hitToWall = false;
		while(offset < moveSeconds) {
			var t = Time.time;
			yield return new WaitForEndOfFrame();
			var diff = (Time.time - t) * Slow.Instance.GetPlayerSpeed();
			offset += diff;
			var parcent = offset / moveSeconds;
			transform.position = start + (dir * dist * parcent);
			//前方にレイを撃って壁に当たったなら移動を終了する
			RaycastHit hit;
			if(Physics.Raycast(transform.position + Vector3.up, dir, out hit, 1f, LayerMask.GetMask(LayerName.Stage.String()))) {
				hitToWall = true;
				break;
			}
			var currDistance = Utilities.DistanceXZ(transform.position, target.transform.position);
		}
		if (!hitToWall) {
			transform.position = end;
		}
	}
}
