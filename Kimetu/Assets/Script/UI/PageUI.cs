using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageUI : MonoBehaviour {
	[SerializeField]
	private GameObject[] pages;

	[SerializeField]
	private int startPage = 0;

	[SerializeField]
	private bool circleSelect = false;

	private int selected;
	private float lasttime;

    [SerializeField]
    private bool isShowPageNum = false;

    [SerializeField]
    private Text pageNumText;

    // Use this for initialization
    void Start () {
		this.lasttime = -2;
		Select(startPage);
        ShowPageNum(startPage);
	}

	// Update is called once per frame
	void Update () {
		if (InputMap.Direction.Left.IsDetectedInput()) {
			Select(selected - 1);
		} else if (InputMap.Direction.Right.IsDetectedInput()) {
			Select(selected + 1);
		}
        ShowPageNum(selected);
	}

	public void Select(int pageIndex) {
		if((Time.unscaledTime - lasttime) < 0.2f) {
			return;
		}
		this.lasttime = Time.unscaledTime;
		if (circleSelect) {
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

    private void ShowPageNum(int index)
    {
        if (isShowPageNum) pageNumText.text = (index + 1).ToString();
    }
}
