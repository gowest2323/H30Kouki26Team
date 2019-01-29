using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TableUI : MonoBehaviour {
	[SerializeField]
	private int rowCount = 0;

	[SerializeField]
	private int colCount = 0;

	[SerializeField]
	private MenuLine[] grid;

	[SerializeField]
	private InputMap.Type executeButton = InputMap.Type.AButton;

	[SerializeField]
	private bool executeCommand = true;

	public int selectedRow { private set; get; }

	public int selectedCol { private set; get; }
	private float time;
	private bool freeze;

	// Use this for initialization
	void Start () {
		this.time = -2;
		this.selectedRow = -1;
		this.selectedCol = -1;
		Select(0, 0);
	}

	// Update is called once per frame
	void Update () {
		InputUpdate();
	}

	/// <summary>
	/// MenuUIの `Update` の実装を公開します。
	/// </summary>
	public void InputUpdate() {
		if (freeze) {
			return;
		}

		int rv = 0;
		int cv = 0;

		//縦の移動
		if (InputMap.Direction.Up.IsDetectedInput()) {
			rv = -1;
		} else if (InputMap.Direction.Down.IsDetectedInput()) {
			rv = 1;
		}

		//横の移動
		if (InputMap.Direction.Left.IsDetectedInput()) {
			cv = -1;
		} else if (InputMap.Direction.Right.IsDetectedInput()) {
			cv = 1;
		}

		if (rv != 0 || cv != 0) {
			Select(selectedRow + rv, selectedCol + cv);
		}

		ExecuteCommand();
	}

	private void ExecuteCommand() {
		if (!executeCommand ||
				!Input.GetButtonDown(executeButton.GetInputName()) ||
				IsInvalidSelect()) {
			return;
		}

		StartCoroutine(ExecuteWait(grid[selectedRow].elements[selectedCol].gameObject.GetComponent<IExecuteCommand>()));
	}

	private bool IsInvalidSelect() {
		return selectedRow >= rowCount ||
			   selectedRow < 0 ||
			   selectedCol >= colCount ||
			   selectedCol < 0;
	}

	private IEnumerator ExecuteWait(IExecuteCommand cmd) {
		this.freeze = true;
		yield return cmd.OnExecute();
		this.freeze = false;
	}

	private void Select(int row, int col) {
		//押しっぱなしにするとすごいことになるので
		if (Time.unscaledTime - time < 0.2f) { return; }

		//インデックスが循環するように
		if (row < 0) row = rowCount - 1;
		else if (row >= rowCount) row = 0;

		if (col < 0) col = colCount - 1;
		else if (col >= colCount) col = 0;

		//前回の選択要素を更新
		if (selectedRow != -1 && selectedCol != -1) {
			grid[selectedRow].elements[selectedCol].OnLostFocus();
		}

		//今回の選択によって更新
		grid[row].elements[col].OnFocus();
		this.selectedRow = row;
		this.selectedCol = col;
		this.time = Time.unscaledTime;
	}
	#if UNITY_EDITOR
	public int GetRowCount() {
		return rowCount;
	}
	public int GetColCount() {
		return colCount;
	}
	public MenuLine[] GetGrid() {
		return grid;
	}
	#endif
}
#if UNITY_EDITOR
[CustomEditor(typeof(TableUI))]
public class TableUIEditor : Editor {
	private TableUI self = null;

	void OnEnable () {
		this.self = (TableUI) target;
	}

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		var rowc = self.GetRowCount();
		var colc = self.GetColCount();
		var grid = self.GetGrid();

		if (rowc != grid.Length) {
			EditorGUILayout.HelpBox("行列が正しく設定されていません", MessageType.Error);
		} else {
			for (int i = 0; i < grid.Length; i++) {
				var elems = grid[i].elements;

				if (colc != elems.Length) {
					EditorGUILayout.HelpBox("行列が正しく設定されていません", MessageType.Error);
					break;
				}
			}
		}
	}
}
#endif