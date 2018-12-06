using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {
	[SerializeField]
	private Text[] menuItems;

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
		if (Time.time - time < 0.2f) { return; }

		if (index < 0) { index = menuItems.Length - 1; }

		if (index >= menuItems.Length) { index = 0;}

		foreach (var item in menuItems) {
			item.color = Color.black;
		}

		menuItems[index].color = Color.red;
		this.selected = index;
		this.time = Time.time;
	}
}
