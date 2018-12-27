﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirinuke : MonoBehaviour {
	[SerializeField, Header("近付くのにかかる時間")]
	private float moveSeconds = 0.5f;

	[SerializeField, Header("プレイヤーから敵へのベクトルをどれだけずらすか"), Range(0f, 360f)]
	private float adjustDegree = 45f;

	[SerializeField, Header("敵をどれだけ通り過ぎるか")]
	private float distanceToEnemy = 1.5f;

	[SerializeField]
	private PlayerAnimation playerAnimation;

	public bool isRunning { private set; get; }
	public GameObject target { private set; get; }

	// Use this for initialization
	void Start () {
		if(playerAnimation == null) {
			this.playerAnimation = GetComponent<PlayerAnimation>();
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
			playerAnimation.StartKirinukeAnimation();
			this.target = enemy;
			yield return MoveToEnemy();
			//yield return TurnToEnemyBack();
			playerAnimation.StopKirinukeAnimation();
			this.isRunning = false;
		} else {
			this.isRunning = false;
		}
	}

	private IEnumerator MoveToEnemy() {
		var dir = Quaternion.Euler(0, adjustDegree, 0) * (target.transform.position - transform.position).normalized;
		var dist = Utilities.DistanceXZ(target.transform.position, transform.position) + distanceToEnemy;
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
		}
		transform.position = end;
	}
}