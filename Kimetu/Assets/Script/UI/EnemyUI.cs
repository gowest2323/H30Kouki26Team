using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour {
	[SerializeField]
	private Slider slider;
	private EnemyStatus status;

	// Use this for initialization
	void Start () {
		status = gameObject.GetComponentInParent<EnemyStatus>();
	}
	// Update is called once per frame
	void Update () {
		slider.value = status.GetHPRatio();
	}
}
