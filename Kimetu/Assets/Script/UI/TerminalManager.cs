﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : MonoBehaviour {
	[SerializeField]
	private GameObject terminalPrefab;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		GameObject.Instantiate(terminalPrefab);
		TerminalRegistry.instance.Register("hello", (args) => {
			Debug.Log("hello");
		});
		TerminalRegistry.instance.Register("echo", (args) => {
			Debug.Log(args[0]);
		});
		TerminalRegistry.instance.Register("enemy-damage", (args) => {
			Debug.Log("enemy-damage");
			if(args.Length == 0 || args[0] == "near") {
				var player = GameObject.FindGameObjectWithTag(TagName.Player.String());
				var enemy = Utilities.SearchMostNearEnemyInTheRange(player.transform.position, 100.0f, false);
				if (enemy != null) {
					var status = enemy.GetComponent<Status>();
					status.__SetHP(1);
				}
			} else if(args[0] == "all") {
				foreach(var enemy in GameObject.FindGameObjectsWithTag(TagName.Enemy.String())) {
					var status = enemy.GetComponent<Status>();
					status.__SetHP(1);
				}
			}
		});
		TerminalRegistry.instance.Register("enemy-heal", (args) => {
			Debug.Log("enemy-heal");
			if (args.Length == 0 || args[0] == "near") {
				var player = GameObject.FindGameObjectWithTag(TagName.Player.String());
				var enemy = Utilities.SearchMostNearEnemyInTheRange(player.transform.position, 100.0f, false);
				if (enemy != null) {
					var status = enemy.GetComponent<Status>();
					status.__SetHP(status.__GetMaxHP());
				}
			} else if (args[0] == "all") {
				foreach (var enemy in GameObject.FindGameObjectsWithTag(TagName.Enemy.String())) {
					var status = enemy.GetComponent<Status>();
					status.__SetHP(status.__GetMaxHP());
				}
			}
		});
		TerminalRegistry.instance.Register("player-heal", (args) => {
			var player = GameObject.FindGameObjectWithTag(TagName.Player.String());
			var status = player.GetComponent<PlayerStatus>();
			status.__SetHP(status.__GetMaxHP());
			status.__SetStamina(status.__GetMaxStamina());
		});
		TerminalRegistry.instance.Register("warp", (args) => {
			var cp = Utilities.FindAny(
				"StageChangeArea01",
				"StageChangeArea02",
				"StageChangeArea03"
			);
			if(cp != null) {
				var player = GameObject.FindGameObjectWithTag(TagName.Player.String());
				player.transform.position = cp.transform.position;
			}
		});
		TerminalRegistry.instance.Register("movie-boss", (args) => {
			SceneChanger.Instance().Change(SceneName.LastBossMovie, new FadeData(1, 1, Color.black));
		});
#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
