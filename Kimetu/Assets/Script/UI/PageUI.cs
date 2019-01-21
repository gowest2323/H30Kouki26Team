﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageUI : MonoBehaviour {
	[SerializeField]
	private GameObject[] pages;

	[SerializeField]
	private int startPage = 0;

	[SerializeField]
	private bool circleSelect = false;

	private int selected;

	// Use this for initialization
	void Start () {
		Select(startPage);
	}

	// Update is called once per frame
	void Update () {
		if (InputMap.Direction.Left.IsDetectedInput()) {
			Select(selected - 1);
		} else if (InputMap.Direction.Right.IsDetectedInput()) {
			Select(selected + 1);
		}
	}

	public void Select(int pageIndex) {
		if(circleSelect) {
			if (pageIndex < 0) pageIndex = pages.Length - 1;
			if (pageIndex >= pages.Length) pageIndex = 0;
		} else {
			if (pageIndex < 0) pageIndex = 0;
			if (pageIndex >= pages.Length) pageIndex = pages.Length - 1;
		}

		foreach (var page in pages) {
			page.SetActive(false);
		}

		pages[pageIndex].SetActive(true);
		this.selected = pageIndex;
	}
}
