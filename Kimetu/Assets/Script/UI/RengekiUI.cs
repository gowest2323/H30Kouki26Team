using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class RengekiUI : MonoBehaviour {
	[SerializeField]
	private GameObject root;

	[SerializeField]
	private Slider pushSlider;

	[SerializeField]
	private Rengeki rengeki;

	private System.IDisposable startObserver;
	private System.IDisposable endObserver;

	// Use this for initialization
	void Start () {
		if(root == null) {
			this.root = transform.FindRec("LBack");
		}
		if(pushSlider == null) {
			this.pushSlider = root.transform.FindRec("LSlider").GetComponent<Slider>();
		}
		if (rengeki == null) {
			this.rengeki = GameObject.FindGameObjectWithTag(TagName.Player.String()).GetComponent<Rengeki>();
		}
		root.SetActive(false);
		rengeki.onPush.Subscribe((e) => {
			pushSlider.value = e.parcent;
		});
		this.startObserver = Slow.Instance.onStart.Subscribe(OnSlowStart);
		this.endObserver = Slow.Instance.onEnd.Subscribe(OnSlowEnd);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDestroy() {
		startObserver.Dispose();
		endObserver.Dispose();
	}

	private void OnSlowStart(bool b) {
		root.SetActive(true);
		pushSlider.value = 0f;
	}

	private void OnSlowEnd(bool b) {
		root.SetActive(false);
	}
}
