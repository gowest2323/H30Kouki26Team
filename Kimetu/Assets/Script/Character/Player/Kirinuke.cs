using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirinuke : MonoBehaviour {
	[SerializeField, Header("近くのにかかる時間")]
	private float moveSeconds = 0.5f;
	[SerializeField, Header("回りこむのにかかる時間")]
	private float turnSeconds = 1f;

	[SerializeField, Header("敵の向こう側へ行く時、どれだけ距離を空けるか")]
	private float moveDistanceLimit = 2f;

	[SerializeField, Header("回りこむ角度"), Range(0f, 360f)]
	private float turnDegree = 100f;

	public bool isRunning { private set; get; }
	public GameObject target { private set; get; }

	// Use this for initialization
	void Start () {
		
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
		outObj = null;
		var enemies = GameObject.FindGameObjectsWithTag(TagName.Enemy.String());
		if(enemies.Length == 0) {
			return false;
		}
		var distance = Mathf.Infinity;
		foreach(var enemy in enemies) {
			var temp = Utilities.DistanceXZ(transform.position, enemy.transform.position);
			if(temp < distance) {
				distance = temp;
				outObj = enemy;
			}
		}
		return distance < Mathf.Infinity;
	}

	private IEnumerator KirinukeUpdate() {
		this.isRunning = true;
		GameObject enemy;
		if(FindNearEnemy(out enemy)) {
			this.target = enemy;
			yield return MoveToEnemy();
			yield return TurnToEnemyBack();
			this.isRunning = false;
		} else {
			this.isRunning = false;
		}
	}

	private IEnumerator MoveToEnemy() {
		var dir = (target.transform.position - transform.position).normalized;
		var dist = Utilities.DistanceXZ(target.transform.position, transform.position) - moveDistanceLimit;
		if(dist < 0) {
			yield break;
		}
		var start = transform.position;
		var end = start + (dir * dist);
		var offset = 0f;
		while(offset < moveSeconds) {
			var t = Time.time;
			yield return new WaitForEndOfFrame();
			var diff = (Time.time - t) * Slow.Instance.GetPlayerSpeed();
			offset += diff;
			var parcent = offset / moveSeconds;
			transform.position = start + (dir * dist * parcent);
			var currDistance = Utilities.DistanceXZ(transform.position, target.transform.position);
			if(currDistance < moveDistanceLimit) {
				break;
			}
		}
		transform.position = end;
	}

	private IEnumerator TurnToEnemyBack() {
		//円の中心をプレイヤーとする
		var center = target.transform.position;
		//自分からプレイヤーへの線が難度であるかをここで取得する
		var dirToCenter =center- transform.position;
		var radian = Mathf.Atan2(dirToCenter.z, dirToCenter.x) + (180f * Mathf.Deg2Rad);
		//一定時間で補完する
		var start = transform.position;
		var offset = 0f;
		while(offset < turnSeconds) {
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
			transform.position = center + (dirv * moveDistanceLimit);
		}
	}
}
