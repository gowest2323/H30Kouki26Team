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

	[SerializeField]
	private EnemyAction action;
	[SerializeField]
	private EnemyAnimation animation;
	[SerializeField]
	private float attackRange = 1.5f;
	[SerializeField]
	private float routeCalculateInterval = 1.0f;

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
		if(action == null) {
			this.action = GetComponent<EnemyAction>();
		}
		if(animation == null) {
			this.animation = new EnemyAnimation(GetComponent<Animator>());
		}
		Assert.IsTrue(agent != null);
		Assert.IsTrue(playerObj != null);
		Assert.IsTrue(action != null);
		Assert.IsTrue(animation != null);
		StartCoroutine(Research());
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	private IEnumerator Research() {
		//1秒ごとに自分と敵の間の距離をみて、
		//ある程度離れていたなら追跡する
		var wait = new WaitForSeconds(routeCalculateInterval);
		while(true) {
			if(playerObj == null) {
				break;
			}
			yield return wait;
			var distance = Vector3.Distance(playerObj.transform.position, transform.position);
			if(distance > attackRange) {
				agent.SetDestination(playerObj.transform.position);
				action.Run();
			} else {
				//ある程度距離が近くなったので攻撃を実行する
				action.Attack();
			}
		}
	}
}
