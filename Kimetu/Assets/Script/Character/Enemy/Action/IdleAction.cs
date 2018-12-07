using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IdleAction : MonoBehaviour, IEnemyActionable {
	[SerializeField]
	private float waitTime;
	private EnemyAnimation enemyAnimation;
	[SerializeField]
	private EnemySearchableAreaBase searchArea;
	public bool isFindPlayer { get; private set; }
	private GameObject player;

	private void Start() {
		enemyAnimation = GetComponentInParent<EnemyAnimation>();
		player = GameObject.FindGameObjectWithTag(TagName.Player.String());
	}

	/// <summary>
	/// 待機行動
	/// </summary>
	/// <returns></returns>
	public IEnumerator Action(UnityAction callBack) {
		yield return Action(callBack, waitTime);
	}

	public IEnumerator Action(UnityAction callBack, float waitTime) {
		isFindPlayer = false;
		enemyAnimation.StopRunAnimation();
		float time = 0.0f;

		while (time < waitTime) {
			if (searchArea.IsPlayerInArea(player, true)) {
				isFindPlayer = true;
				callBack.Invoke();
				yield break;
			}

			time += Slow.Instance.DeltaTime();
			yield return new WaitForSeconds(Slow.Instance.DeltaTime());
		}

		yield return new WaitForSeconds(waitTime);
		callBack.Invoke();
	}
}
