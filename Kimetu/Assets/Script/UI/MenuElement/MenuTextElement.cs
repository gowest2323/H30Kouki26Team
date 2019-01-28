using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTextElement : MenuElement {
	[SerializeField]
	private Text text;

	private Color defaultColor;

	[SerializeField]
	private Color selectedColor = Color.red;

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		if(this.text == null) {
			this.text = GetComponent<Text>();
		}
		this.defaultColor = text.color;
	}

	public override void OnFocus() {
		text.color = selectedColor;
	}

	public override void OnLostFocus() {
		text.color = defaultColor;
	}
}
