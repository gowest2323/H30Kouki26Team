using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonGroupUI : MonoBehaviour {
	[SerializeField]
	private Button[] buttons;

	public int selected { private set; get; }
	private float time;
	private int click;

	// Use this for initialization
	void Start () {
		time = -2;
		Select(0);
	}

	// Update is called once per frame
	void Update () {
		if (InputMap.Direction.Up.IsDetectedInput()) {
			Select(this.selected - 1);
		} else if (InputMap.Direction.Down.IsDetectedInput()) {
			Select(this.selected + 1);
		}

		if (click == 0 && Input.GetButtonDown(InputMap.Type.AButton.GetInputName())) {
			StartCoroutine(DoClick());
		}
	}

	private IEnumerator DoClick() {
		//遅延させないとすぐに戻ってしまうので
		//その対策
		this.click++;
		Debug.Log("do click");
		yield return new WaitForSecondsRealtime(0.1f);
		buttons[selected].onClick.Invoke();
		this.click--;
	}

	private void Select(int index) {
		if (Time.realtimeSinceStartup - time < 0.2f) { return; }

		if (index < 0) { index = buttons.Length - 1; }

		if (index >= buttons.Length) { index = 0;}

		EventSystem.current.SetSelectedGameObject(null);
		buttons[index].Select();
		this.selected = index;
		this.time = Time.realtimeSinceStartup;
	}
}
