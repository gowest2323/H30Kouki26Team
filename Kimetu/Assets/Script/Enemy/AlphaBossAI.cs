using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

/// <summary>
/// アルファ版ボスのAI。
/// </summary>
public class AlphaBossAI : AI {
	[SerializeField]
	private NavMeshAgent agent;

	private GameObject playerObj;

	private void Awake() {
		
	}
	// Use this for initialization
	public override void Start () {
		base.Start();
		if(agent == null) {
			this.agent = GetComponent<NavMeshAgent>();
		}
		this.playerObj = GameObject.FindWithTag(TagName.Player.String());
		Assert.IsTrue(agent != null);
		Assert.IsTrue(playerObj != null);
		StartCoroutine(Research());
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	private IEnumerator Research() {
		//1秒ごとに自分と敵の間の距離をみて、
		//ある程度離れていたなら追跡する
		var wait = new WaitForSeconds(1f);
		while(true) {
			yield return wait;
			var distance = Vector3.Distance(playerObj.transform.position, transform.position);
			if(distance > 3) {
				agent.SetDestination(playerObj.transform.position);
			}
		}
	}
}
