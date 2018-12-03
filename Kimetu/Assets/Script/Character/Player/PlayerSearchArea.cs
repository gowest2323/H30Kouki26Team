using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーのサーチ範囲管理
/// </summary>
public class PlayerSearchArea : MonoBehaviour {
	[SerializeField]
	private PlayerAction player;

	private void OnTriggerStay(Collider other) {
		TagName otherTag = TagNameManager.GetKeyByValue(other.tag);

		if (otherTag == TagName.EnemyDeadArea) {
			EnemyAI enemy = other.GetComponentInParent<EnemyAI>();
			player.NearEnemy(enemy);
		}
	}

	private void OnTriggerExit(Collider other) {
		TagName otherTag = TagNameManager.GetKeyByValue(other.tag);

		if (otherTag == TagName.EnemyDeadArea) {
			EnemyAI enemy = other.GetComponentInParent<EnemyAI>();
			player.FarEnemy(enemy);
		}
	}
}
