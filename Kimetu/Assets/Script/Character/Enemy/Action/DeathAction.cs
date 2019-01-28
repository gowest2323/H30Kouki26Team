using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathAction : ActionBase {

	[SerializeField]
	private string deathStateName = "dead";
	public UnityAction deadEnd { get; set; }

	protected override void Start() {
		base.Start();
	}
	public override IEnumerator Action() {
		enemyAnimation.StartDeathAnimation();
		yield return enemyAnimation.WaitAnimation("oni", deathStateName);
		deadEnd.Invoke();
	}
}
