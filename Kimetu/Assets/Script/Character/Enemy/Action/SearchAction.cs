using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAction : ActionBase {
	[SerializeField]
	private EnemySearchableAreaBase searchArea;
	public bool canSearch { get; private set; }
	private GameObject player;

	// Use this for initialization
	protected override void Start() {
		base.Start();
		canSearch = false;
		player = GetPlayer();
	}

	public override IEnumerator Action() {
		cancelFlag = false;
		canSearch = false;

		while (!cancelFlag) {
			if (searchArea.IsPlayerInArea(player, true)) {
				canSearch = true;
				break;
			}

			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}
	}

}
