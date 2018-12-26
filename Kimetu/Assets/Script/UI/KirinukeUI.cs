using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KirinukeUI : LongPressUI {
	private Kirinuke kirinuke;

	// Use this for initialization
	public override void Start () {
		base.Start();
		var player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		this.kirinuke = player.GetComponent<Kirinuke>();
		GetLongPressDetector().OnLongPressTrigger += (e) => {
			Debug.Log("Trigger");
			kirinuke.StartKirinuke();
		};
	}

	protected override LongPressDetector FindLongPressDetector() {
		return null;
	}

	protected override bool CanShowUI() {
		return !kirinuke.isRunning;
	}
}
