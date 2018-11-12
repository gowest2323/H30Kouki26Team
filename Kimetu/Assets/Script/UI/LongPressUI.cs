using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// 何のために長押しが必要かを返すインターフェイス。
/// </summary>
public interface ILongPressInformation {
	string longPressMessage { get; }
}

/// <summary>
/// 長押しの状態を視覚的に確認するUI。
/// </summary>
public class LongPressUI : MonoBehaviour {
	[SerializeField]
	private Text text;
	[SerializeField]
	private Slider slider;
	[SerializeField]
	private LongPressDetector[] detectors;
	private string defaultText;

	// Use this for initialization
	void Start () {
		this.defaultText = text.text;
		Assert.IsTrue(text != null);
		Assert.IsTrue(slider != null);
		foreach(var detector in detectors) {
			var info = detector.gameObject.GetComponent<ILongPressInformation>();
			detector.OnLongPressBegin += () => {
				slider.value = 0;
				if(info != null) text.text = info.longPressMessage;
				else text.text = defaultText;
			};
			detector.OnLongPressing += (e) => {
				slider.value = detector.progress;
				if(info != null) text.text = info.longPressMessage;
				else text.text = defaultText;
			};
			detector.OnLongPressEnd += () => {
				slider.value = 0;
				if(info != null) text.text = info.longPressMessage;
				else text.text = defaultText;
			};
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
