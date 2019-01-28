using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour {
	[SerializeField]
	private GameObject root;
	[SerializeField]
	private Image kirinukeImage;
	[SerializeField]
	private Image rengekiImage;

	[SerializeField]
	private Kirinuke kirinuke;

	[SerializeField]
	private LongPressDetector kirinukeDetector;

	[SerializeField]
	private Rengeki rengeki;


	private System.IDisposable startObserver;
	private System.IDisposable endObserver;

	// Use this for initialization
	void Start () {
		this.startObserver = Slow.Instance.onStart.Subscribe(OnSlowStart);
		this.endObserver = Slow.Instance.onEnd.Subscribe(OnSlowEnd);
		if(root == null) {
			this.root = transform.FindRec("Root");
		}
		//kirinuke, rengekiを設定
		var player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		if(kirinuke == null) {
			this.kirinuke = player.GetComponent<Kirinuke>();
		}
		if(rengeki == null) {
			this.rengeki = player.GetComponent<Rengeki>();
		}
		rengeki.onPush.Subscribe((e) => {
			rengekiImage.fillAmount = e.parcent;
			//rengekiMask.SetParcent(e.parcent);
		});
		rengeki.onEnd.Subscribe(OnRengekiEnd);
		//kirinukeの長押し判定
		kirinukeDetector.OnLongPressBegin += () => {
			SetKirinukeMask(0f);
		};
		kirinukeDetector.OnLongPressing += (e) => {
			SetKirinukeMask(kirinukeDetector.progress);
		};
		kirinukeDetector.OnLongPressTrigger += (e) => {
			//スロー中に長押しが完了
			//切り抜け中ではない
			//回り込み中ではない
			//攻撃中ではない
			//ならば切り抜けを実行する
			if (Slow.Instance.isSlowNow &&
			  !kirinuke.isRunning &&
			  !rengeki.moveNow &&
			  !rengeki.actionNow) {
				kirinuke.StartKirinuke();
			}
		};
		kirinukeDetector.OnLongPressEnd += () => {
			SetKirinukeMask(0f);
		};
		root.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void SetKirinukeMask(float p) {
		//kirinukeMask.SetParcent(p);
		kirinukeImage.fillAmount = p;
	}

	public void SetRengekiMask(float p) {
		//rengekiMask.SetParcent(p);
		rengekiImage.fillAmount = p;
	}

	private void OnDestroy() {
		startObserver.Dispose();
		endObserver.Dispose();
	}
	//スロー開始,終了で呼ばれる
	private void OnSlowStart(bool b) {
		root.SetActive(true);
		SetRengekiMask(0f);
		SetKirinukeMask(0f);
	}

	private void OnSlowEnd(bool b) {
		root.SetActive(false);
	}
	//Rengeki終了で呼ばれる
	private void OnRengekiEnd(bool b) {
		SetRengekiMask(0f);
	}
}
