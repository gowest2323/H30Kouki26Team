using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoUI : LongPressUI {
	[SerializeField]
	private ChangeStage changeStage;

	public override void Start() {
		base.Start();
		if(changeStage == null) {
			this.changeStage = FindStageComponent<ChangeStage>();
		}
	}

	protected override LongPressDetector FindLongPressDetector() {
		return FindStageComponent<LongPressDetector>();
	}

	protected override bool CanShowUI() {
		return changeStage.CanGotoNextStage();
	}

	private T FindStageComponent<T>() where T : MonoBehaviour {
		var list = new GameObject[] {
				GameObject.Find("StageChangeArea01"),
				GameObject.Find("StageChangeArea02"),
				GameObject.Find("StageChangeArea03"),
		};
		foreach(var e in list) {
			if(e != null) {
				return e.GetComponent<T>();
			}
		}
		return null;
	}
}
