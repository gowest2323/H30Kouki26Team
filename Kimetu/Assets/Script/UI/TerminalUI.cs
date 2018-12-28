using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalUI : MonoBehaviour {
	[SerializeField]
	private Image root;

	[SerializeField]
	private Text header;

	[SerializeField]
	private Text editor;

	private bool show;
	private string defaultHeaderText, defaultEditorText;

	// Use this for initialization
	void Start () {
		this.defaultHeaderText = header.text;
		this.defaultEditorText = editor.text;
		ToggleEditor(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Slash)) {
			ToggleEditor();
		}
		if (!show) {
			return;
		}
	}

	private void ToggleEditor() {
		ToggleEditor(!show);
	}

	private void ToggleEditor(bool enable) {
		if(enable) {
			this.header.text = defaultHeaderText;
			this.editor.text = defaultEditorText;
		}
		root.gameObject.SetActive(enable);
		this.show = enable;
	}
}
