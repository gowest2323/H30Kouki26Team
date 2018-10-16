using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// EnemyのHPとスライダーの表示を同期します。
/// </summary>
public class DebugEnemyUI : MonoBehaviour {
	[SerializeField]
	private Slider slider;
	[SerializeField]
	private EnemyStatus status;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		slider.value = status.GetHPRatio();
	}
}
