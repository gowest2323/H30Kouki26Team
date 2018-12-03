using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageUI : MonoBehaviour {
	[SerializeField]
	private GameObject[] pages;

	[SerializeField]
	private int startPage = 0;

	private int selected;

	// Use this for initialization
	void Start () {
		Select(startPage);
	}

	// Update is called once per frame
	void Update () {
		var inputH = InputMap.GetDPadHorizontal();

		if (inputH < 0) {
			Select(selected - 1);
		} else if (inputH > 0) {
			Select(selected + 1);
		}
	}

	public void Select(int pageIndex) {
		if (pageIndex < 0) pageIndex = 0;

		if (pageIndex >= pages.Length) pageIndex = pages.Length - 1;

		foreach (var page in pages) {
			page.SetActive(false);
		}

		pages[pageIndex].SetActive(true);
		this.selected = pageIndex;
	}
}
