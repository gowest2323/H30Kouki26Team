using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アルファ版ボスのAI。
/// </summary>
public class AlphaBossAI : AI {
	private GameObject playerObj;

	// Use this for initialization
	public override void Start () {
		base.Start();
		this.playerObj = GameObject.FindWithTag(TagName.Player.String());
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		var playerPos = playerObj.transform.position;
		var selfPos = transform.position;
		var distance = Vector3.Distance(playerPos, selfPos);
		if(distance > 3) {
	
		}
	}
}
