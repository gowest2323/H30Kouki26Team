using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//FIXME:ほとんどMenuUIのコピペ

public class ListUI : MonoBehaviour {
	[System.Serializable]
	public struct Icon {
		public Image unselected;
		public Image selected;
	}

	[SerializeField]
	private Icon[] icons;

	public int selected { private set; get; }

	private float time;

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
	}

	private void Select(int index) {
		//NOTE:ポーズ中は Time.time ではなくrealtimeSinceStartup を使う
		if (Time.realtimeSinceStartup - time < 0.2f) { return; }

		if (index < 0) { index = icons.Length - 1; }

		if (index >= icons.Length) { index = 0;}

		foreach (var icon in icons) {
			icon.unselected.gameObject.SetActive(true);
			icon.selected.gameObject.SetActive(false);
		}

		icons[index].selected.gameObject.SetActive(true);
		icons[index].unselected.gameObject.SetActive(false);
		this.selected = index;
		this.time = Time.realtimeSinceStartup;
		Debug.Log("select " + index);
	}
}
