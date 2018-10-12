using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {
	[SerializeField]
	private Text hpText;

	[SerializeField]
	private Text spText;

	[SerializeField]
	private PlayerStatus status;

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
	}
}
