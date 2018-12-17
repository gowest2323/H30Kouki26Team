using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// 長押しの状態を視覚的に確認するUI。
/// </summary>
public class LongPressUI : MonoBehaviour {
	[SerializeField]
	private Image root;

	[SerializeField]
	private Slider slider;

	[SerializeField]
	private LongPressDetector longPressDetector;

	private bool near;
	private bool triggered;

	// Use this for initialization
	public virtual void Start () {
		if(root == null) {
			this.root = transform.FindRec("LBack").GetComponent<Image>();
		}
		if(slider == null) {
			this.slider = transform.FindRec("LSlider").GetComponent<Slider>();
		}
		if(longPressDetector==null) {
			this.longPressDetector = FindLongPressDetector();
		}
		//長押しの状態に応じてスライダーの値を更新
		root.gameObject.SetActive(false);
		longPressDetector.OnLongPressBegin += () => {
			slider.value = 0;
		};
		longPressDetector.OnLongPressing += (e) => {
			slider.value = longPressDetector.progress;
		};
		longPressDetector.OnLongPressEnd += () => {
			if(longPressDetector.progress < 1) {
				this.triggered = false;
			}
			Hide();
		};
	}

	private void Show() {
		root.gameObject.SetActive(true);
	}

	private void Hide() {
		root.gameObject.SetActive(false);
		slider.value = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(CanShowUI()) {
			if(!triggered) {
				this.triggered = true;
				Show();
			}
		} else {
			this.triggered = false;
			longPressDetector.Cancel();
			Hide();
		}
	}

	protected virtual LongPressDetector FindLongPressDetector() {
		return null;
	}

	protected virtual bool CanShowUI() {
		return false;
	}
}
