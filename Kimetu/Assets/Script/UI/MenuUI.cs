﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {
	public enum Orientation {
		Horizontal,
		Vertical
	}

	[SerializeField]
	private MenuElement[] elements;

	[SerializeField]
	private Orientation orientation = Orientation.Vertical;

	[SerializeField]
	private InputMap.Type executeButton = InputMap.Type.AButton;

	[SerializeField]
	private bool executeCommand = true;

	public int selected { private set; get; }
	private float time;
	private bool freeze;

	// Use this for initialization
	void Start () {
		this.time = -2;
		this.selected = -1;
		Select(0);
	}

	// Update is called once per frame
	void Update () {
		InputUpdate();
	}

	/// <summary>
	/// MenuUIの `Update` の実装を公開します。
	/// </summary>
	public void InputUpdate() {
		if(freeze) {
			return;
		}
		if(this.orientation == Orientation.Vertical) {
			if (InputMap.Direction.Up.IsDetectedInput()) {
				Select(this.selected - 1);
			} else if (InputMap.Direction.Down.IsDetectedInput()) {
				Select(this.selected + 1);
			}
		} else {
			if (InputMap.Direction.Left.IsDetectedInput()) {
				Select(this.selected - 1);
			} else if (InputMap.Direction.Right.IsDetectedInput()) {
				Select(this.selected + 1);
			}
		}
		ExecuteCommand();
	}

	private void ExecuteCommand() {
		if(!executeCommand ||
		   !Input.GetButtonDown(executeButton.GetInputName()) ||
		   selected == -1) {
			   return;
		}
		StartCoroutine(ExecuteWait(elements[selected].gameObject.GetComponent<IExecuteCommand>()));
	}

	private IEnumerator ExecuteWait(IExecuteCommand cmd) {
		this.freeze = true;
		yield return cmd.OnExecute();
		this.freeze = false;
	}

	private void Select(int index) {
		//押しっぱなしにするとすごいことになるので
		if (Time.unscaledTime - time < 0.2f) { return; }
		//インデックスが循環するように
		if (index < 0) { index = elements.Length - 1; }
		if (index >= elements.Length) { index = 0;}
		//前回の選択要素を更新
		if(selected != -1) {
			elements[selected].OnLostFocus();
		}
		//今回の選択によって更新
		elements[index].OnFocus();
		this.selected = index;
		this.time = Time.unscaledTime;
	}
}
