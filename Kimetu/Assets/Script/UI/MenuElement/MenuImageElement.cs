using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuImageElement : MenuElement {
	[SerializeField]
	private Image image;

	[SerializeField]
	private Sprite defaultSprite;

	[SerializeField]
	private Sprite selectedSprite;

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		if(this.image == null) {
			this.image = GetComponent<Image>();
		}
		if(this.defaultSprite == null) {
			this.defaultSprite = image.sprite;
		}
		image.sprite = defaultSprite;
	}

	public override void OnFocus() {
		image.sprite = selectedSprite;
	}

	public override void OnLostFocus() {
		image.sprite = defaultSprite;
	}
}
