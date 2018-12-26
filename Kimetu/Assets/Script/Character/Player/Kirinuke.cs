using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirinuke : MonoBehaviour {
	[SerializeField]
	private float moveSeconds = 0.5f;
	[SerializeField]
	private float turnSeconds = 0.2f;

	[SerializeField, Header("敵の向こう側へ行く時、どれだけ距離を空けるか")]
	private float moveDistanceLimit = 0.5f;
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
		var center = target.transform.position;
		//var dirx = Mathf.Cos(Mathf.Deg2Rad * 45f);
		//var dirz = Mathf.Cos(Mathf.Deg2Rad * 45f);
		//var dirv = new Vector3(dirx, 0, dirz);
		var start = transform.position;
		var end = start + (transform.forward * 1.5f);
		var offset = 0f;
		while(offset < turnSeconds) {
			var t = Time.time;
			yield return new WaitForEndOfFrame();
			var diff = (Time.time - t) * Slow.Instance.GetPlayerSpeed();
			offset += diff;
			var parcent = offset / turnSeconds;
			transform.LookAt(target.transform.position);
			transform.position = Vector3.Slerp(start, end, parcent);
		}
		transform.position = end;
	}
}
