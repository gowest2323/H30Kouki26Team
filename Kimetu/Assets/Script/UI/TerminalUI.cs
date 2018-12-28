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

	[SerializeField]
	private float deleteWait = 0.1f;

	private float deleteElapsed;
	private bool deleteAccept;

	private bool show;
	private string defaultHeaderText, defaultEditorText;
	private Color defaultHeaderColor, defaultEditorColor;

	// Use this for initialization
	void Start () {
		this.deleteAccept = true;
		this.defaultHeaderText = header.text;
		this.defaultEditorText = editor.text;
		this.defaultHeaderColor = header.color;
		this.defaultEditorColor = editor.color;
		ToggleEditor(false);
		StartCoroutine(BlinkText(header, defaultHeaderColor));
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Slash)) {
			ToggleEditor();
		}
		if (!show) {
			return;
		}
		InputText();
	}

	private void InputText() {
		InputKeyArray(KeyCode.A, KeyCode.Z, (key) => {
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
				return key.ToString();
			} else {
				return key.ToString().ToLower();
			}
		});
		InputKey(KeyCode.Alpha0, "0");
		InputKey(KeyCode.Alpha1, "1");
		InputKey(KeyCode.Alpha2, "2");
		InputKey(KeyCode.Alpha3, "3");
		InputKey(KeyCode.Alpha4, "4");
		InputKey(KeyCode.Alpha5, "5");
		InputKey(KeyCode.Alpha6, "6");
		InputKey(KeyCode.Alpha7, "7");
		InputKey(KeyCode.Alpha8, "8");
		InputKey(KeyCode.Alpha9, "9");
		InputKey(KeyCode.Plus, "+");
		InputKey(KeyCode.Minus, "-");
		InputKey(KeyCode.Asterisk, "*");
		InputKey(KeyCode.Slash, "/");
		InputKey(KeyCode.Space, " ");
		//バックスペース押しっぱなしで消せるように
		if(editor.text.Length > 0 &&
		   Input.GetKey(KeyCode.Backspace) && deleteAccept) {
			editor.text = editor.text.Substring(0, editor.text.Length - 1);
			this.deleteAccept = false;
			this.deleteElapsed = 0;
		}
		if(!deleteAccept && deleteElapsed < deleteWait) {
			this.deleteElapsed += Time.deltaTime;
			this.deleteAccept = deleteElapsed >= deleteWait;
		}
		ExecuteCommand();
	}

	private void ExecuteCommand() {
		if(!Input.GetKey(KeyCode.Return) && !Input.GetKey(KeyCode.KeypadEnter)) {
			return;
		}
		//エンターでコマンドを実行
		var words = editor.text.Split(' ');
		if (words.Length == 1) {
			TerminalRegistry.instance.Invoke(words[0], new string[] { });
		} else {
			var args = new string[words.Length - 1];
			System.Array.Copy(words, 1, args, 0, args.Length);
			TerminalRegistry.instance.Invoke(words[0], args);
		}
		editor.text = "";
	}

	private void InputKeyArray(KeyCode start, KeyCode end, System.Func<KeyCode,string> outKeyString) {
		var iter = start;
		while (iter <= end) {
			var key = (KeyCode)iter;
			InputKey(key, outKeyString(key));
			iter++;
		}
	}

	private void InputKey(KeyCode key) {
		InputKey(key, key.ToString());
	}

	private void InputKey(KeyCode key, string str) {
		if (Input.GetKeyDown(key)) {
			editor.text += str;
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

	private IEnumerator BlinkText(Text text, Color baseColor) {
		var changeColor = baseColor;
		changeColor.a = 0f;
		while (true) {
			text.color = baseColor;
			yield return new WaitForSeconds(0.5f);
			text.color = changeColor;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
