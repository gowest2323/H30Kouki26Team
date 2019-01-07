using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//今はもう使われていません
//代わりにSkillUIを使用します。

public class KirinukeUI : LongPressUI {
	[SerializeField]
	private Kirinuke kirinuke;

	[SerializeField]
	private Rengeki rengeki;

	// Use this for initialization
	public override void Start () {
		base.Start();
		var player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		if(kirinuke == null) {
			this.kirinuke = player.GetComponent<Kirinuke>();
		}
		if(rengeki == null) {
			this.rengeki = player.GetComponent<Rengeki>();
		}
		GetLongPressDetector().OnLongPressTrigger += (e) => {
			//スロー中に長押しが完了
			//切り抜け中ではない
			//回り込み中ではない
			//攻撃中ではない
			//ならば切り抜けを実行する
			if(Slow.Instance.isSlowNow && 
			  !kirinuke.isRunning &&
			  !rengeki.moveNow &&
			  !rengeki.actionNow) {
				kirinuke.StartKirinuke();
			}
		};
	}

	protected override LongPressDetector FindLongPressDetector() {
		return null;
	}

	protected override bool CanShowUI() {
		return (!kirinuke.isRunning && Slow.Instance.isSlowNow);
	}
}
