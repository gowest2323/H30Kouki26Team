using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KyuseiUI : MonoBehaviour {
	[SerializeField]
	private Image root;

	[SerializeField]
	private Slider slider;

	[SerializeField]
	private PlayerAction action;

	[SerializeField]
	private LongPressDetector longPressDetector;

	private bool near;
	private bool canceled;

	// Use this for initialization
	void Start () {
		if(root == null) {
			this.root = transform.FindRec("KBBack").GetComponent<Image>();
		}
		if(slider == null) {
			this.slider = transform.FindRec("KSlider").GetComponent<Slider>();
		}
		if(action==null) {
			this.action = GameObject.FindGameObjectWithTag(TagName.Player.String()).GetComponent<PlayerAction>();
		}
		if(longPressDetector==null) {
			this.longPressDetector = GameObject.FindGameObjectWithTag(TagName.Player.String()).GetComponent<LongPressDetector>();
		}
		//長押しの状態に応じてスライダーの値を更新
		root.gameObject.SetActive(false);
		longPressDetector.OnLongPressBegin += () => {
			canceled = false;
		};
		longPressDetector.OnLongPressing += (e) => {
			if(!canceled) {
				slider.value = longPressDetector.progress;
			}
		};
		longPressDetector.OnLongPressEnd += () => {
			if(longPressDetector.progress < 1) {
				this.canceled = true;
			}
			Hide();
		};
	}

	private void Show() {
		root.gameObject.SetActive(true);
		slider.value = 0;
	}

	private void Hide() {
		root.gameObject.SetActive(false);
		slider.value = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(action.CanPierce()) {
			Show();
		} else {
			this.canceled = true;
			Hide();
		}
	}
}
