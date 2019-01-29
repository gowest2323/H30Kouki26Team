using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GotoUI : LongPressUI {
	[SerializeField]
	private ChangeStage changeStage;

	[SerializeField]
	private Image fillImage;

	private string sceneName;

	public override void Start() {
		this.sceneName = SceneManager.GetActiveScene().name;

		if (sceneName.Contains("Boss")) {
			Destroy(gameObject);
		} else {
			base.Start();
		}

		if (changeStage == null) {
			this.changeStage = FindStageComponent<ChangeStage>();
		}
	}

	protected override void UpdateProgress(float value) {
		//base.UpdateProgress(value);
		fillImage.fillAmount = value;
	}

	protected override LongPressDetector FindLongPressDetector() {
		return FindStageComponent<LongPressDetector>();
	}

	protected override bool CanShowUI() {
		if (name.Contains("Boss")) {
			return false;
		}

		return changeStage.CanGotoNextStage();
	}

	private T FindStageComponent<T>() where T : MonoBehaviour {
		var list = new GameObject[] {
			GameObject.Find("StageChangeArea01"),
			GameObject.Find("StageChangeArea02"),
			GameObject.Find("StageChangeArea03"),
		};

		foreach (var e in list) {
			if (e != null) {
				return e.GetComponent<T>();
			}
		}

		return null;
	}
}
