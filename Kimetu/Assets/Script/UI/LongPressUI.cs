using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// 何のために長押しが必要かを返すインターフェイス。
/// </summary>
public interface ILongPressInformation {
	/// <summary>
	/// 長押しすると何が起きるかを表す文字列を返す。
	/// </summary>
	/// <value></value>
	string longPressMessage { get; }
	/// <summary>
	/// 現在長押しをすることで何かアクションを実行できるなら true.
	/// </summary>
	/// <value></value>
	bool canLongPress { get; }
}

/// <summary>
/// 長押しの状態を視覚的に確認するUI。
/// </summary>
public class LongPressUI : MonoBehaviour {
	[SerializeField]
	private GameObject panel;
	[SerializeField]
	private Text text;
	[SerializeField]
	private Slider slider;
	[SerializeField]
	private GameObject[] targets;
	private ILongPressInformation[] informations;
	private LongPressDetector[] detectors;
	private string defaultText;
	private int current;

	/// <summary>
	/// ラムダの外側の整数をキャプチャするためのラッパー。
	/// </summary>
	private class MutableInt {
		public int value;
		public MutableInt(int value) { this.value = value; }
	}

	// Use this for initialization
	void Start () {
		this.defaultText = text.text;
		this.informations = new ILongPressInformation[targets.Length];
		this.detectors = new LongPressDetector[targets.Length];
		this.current = -1;
		for(int i=0; i<targets.Length; i++) informations[i] = targets[i].GetComponent<ILongPressInformation>();
		for(int i=0; i<targets.Length; i++) detectors[i] = targets[i].GetComponent<LongPressDetector>();
		Assert.IsTrue(text != null);
		Assert.IsTrue(slider != null);
		//FIXME:雑
		for(int i=0; i<targets.Length; i++) {
			var detector = detectors[i];
			var info = informations[i];
			var mi = new MutableInt(i);
			detector.OnLongPressBegin += () => {
				if(this.current != mi.value) { return; }
				slider.value = 0;
				if(info != null) text.text = info.longPressMessage;
				else text.text = defaultText;
			};
			detector.OnLongPressing += (e) => {
				if(this.current != mi.value) { return; }
				slider.value = detector.progress;
				if(info != null) text.text = info.longPressMessage;
				else text.text = defaultText;
			};
			detector.OnLongPressEnd += () => {
				if(this.current != mi.value) { return; }
				slider.value = 0;
				if(info != null) text.text = info.longPressMessage;
				else text.text = defaultText;
			};
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.current = -1;
		panel.SetActive(false);
		for(int i=0; i<informations.Length; i++) {
			var info = informations[i];
			if(info.canLongPress) {
				this.current = i;
				panel.SetActive(true);
				break;
			}
		}
	}
}
