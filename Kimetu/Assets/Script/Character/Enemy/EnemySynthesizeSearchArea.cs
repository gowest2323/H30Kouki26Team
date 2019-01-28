using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の攻撃・索敵範囲の合成範囲管理
/// </summary>
public class EnemySynthesizeSearchArea : EnemySearchableAreaBase {
	[SerializeField]
	private List<EnemySearchableAreaBase> searchAreas;

	/// <summary>
	/// 範囲内にプレイヤーがいるか？
	/// </summary>
	/// <param name="player"></param>
	/// <param name="toRayCast"></param>
	/// <returns></returns>
	public override bool IsPlayerInArea(GameObject player, bool toRayCast) {
		foreach (var searchArea in searchAreas) {
			if (searchArea.IsPlayerInArea(player, toRayCast)) {
				return true;
			}
		}

		return false;
	}
}
