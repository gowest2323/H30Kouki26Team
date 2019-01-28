using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour {

	[SerializeField]
	private MenuUI menu;
	[SerializeField]
	private Toggle cameraUpDown;
	[SerializeField]
	private Toggle cameraLeftRight;



	void Start() {
		cameraUpDown.isOn = OptionDataPrefs.GetUpDownBool(false);
		cameraLeftRight.isOn = OptionDataPrefs.GetLeftRightBool(false);
	}
	// Update is called once per frame
	void Update() {

		if (!Input.GetButton(InputMap.Type.BButton.GetInputName())) {
			return;
		}

		var selected = menu.selected;

		if (selected == 0) {
			//カメラ上下反転
			CameraUpDownInverted();
		} else if (selected == 1) {
			//カメラ左右反転
			CameraLeftRightInverted();
		}

	}

	//カメラ上下反転するか
	private void CameraUpDownInverted() {

		if (Input.GetButtonDown(InputMap.Type.BButton.GetInputName())) {
			cameraUpDown.isOn = !cameraUpDown.isOn;
			OptionDataPrefs.SetUpDownInvert(cameraUpDown.isOn);
		}


	}
	//カメラ左右反転するか
	private void CameraLeftRightInverted() {

		if (Input.GetButtonDown(InputMap.Type.BButton.GetInputName())) {
			cameraLeftRight.isOn = !cameraLeftRight.isOn;
			OptionDataPrefs.SetLeftRightInvert(cameraLeftRight.isOn);
		}
	}
}
