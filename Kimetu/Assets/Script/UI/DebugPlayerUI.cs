using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DebugPlayerUI : MonoBehaviour {
	[SerializeField]
	private Slider hpSlider;

	[SerializeField]
	private Slider spSlider;

	[SerializeField]
	private PlayerStatus status;

	// Use this for initialization
	void Start () {
		Assert.IsTrue(hpSlider != null);
		Assert.IsTrue(spSlider != null);
		Assert.IsTrue(status != null);
	}

	// Update is called once per frame
	void Update () {
		hpSlider.value = status.GetHPRatio();
		spSlider.value = status.GetStaminaRatio();
	}
}
