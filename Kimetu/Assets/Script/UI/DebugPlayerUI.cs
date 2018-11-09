using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DebugPlayerUI : MonoBehaviour {
	[SerializeField]
	private Text hpText;

	[SerializeField]
	private Text spText;

	[SerializeField]
	private PlayerStatus status;
    [SerializeField]
    private Slider slider1, slider2;

	// Use this for initialization
	void Start () {
		Assert.IsTrue(hpText != null);
		Assert.IsTrue(spText != null);
		Assert.IsTrue(status != null);
	}
	
	// Update is called once per frame
	void Update () {
		hpText.text = "HP: " + status.GetHP();
		spText.text = "SP: " + status.GetStamina();
        slider1.value = status.GetHP();
        slider2.value = status.GetStamina();
	}
}
