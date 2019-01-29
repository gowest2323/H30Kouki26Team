using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KyuseiUI : LongPressUI {
	[SerializeField]
	private PlayerAction action;

	[SerializeField]
	private Image fillImage;

	public override void Start() {
		base.Start();

		if (action == null) {
			this.action = GameObject.FindGameObjectWithTag(TagName.Player.String()).GetComponent<PlayerAction>();
		}
	}

	protected override void UpdateProgress(float value) {
		//base.UpdateProgress(value);
		fillImage.fillAmount = value;
	}

	protected override LongPressDetector FindLongPressDetector() {
		return GameObject.FindGameObjectWithTag(TagName.Player.String()).GetComponent<LongPressDetector>();
	}

	protected override bool CanShowUI() {
		return action.CanPierce();
	}
}
