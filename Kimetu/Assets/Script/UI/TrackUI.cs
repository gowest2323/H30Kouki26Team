using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// キャンバス, UI, ターゲットを指定して、
/// UIが常にターゲットと同じ位置にいるようにします。
/// </summary>
public class TrackUI : MonoBehaviour {
	[SerializeField]
	private RectTransform canvasRect;

	[SerializeField]
	private RectTransform uiRect;

	[SerializeField]
	private GameObject target;
	public bool track {
		set; get;
	}

	// Use this for initialization
	void Start () {
		Assert.IsTrue(target != null);

		if (this.canvasRect == null) {
			this.canvasRect = GetComponent<RectTransform>();
		}

		if (this.uiRect == null) {
			this.uiRect = GetComponent<RectTransform>();
		}

		Assert.IsTrue(uiRect != null);
		Assert.IsTrue(canvasRect != null);
		this.track = true;
	}

	// Update is called once per frame
	void Update () {
		if (!track) { return; }

		if (target == null) {
			this.track = false;
			return;
		}

		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(target.transform.position);
		Vector2 WorldObject_ScreenPosition = new Vector2(
			((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
			((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

		//now you can set the position of the ui element
		uiRect.anchoredPosition = WorldObject_ScreenPosition;
	}

	/// <summary>
	/// 追跡対象を設定します。
	/// </summary>
	/// <param name="target"></param>
	public void SetTarget(GameObject target) {
		this.target = target;
	}

	/// <summary>
	/// 基準となるキャンバスを変更します。
	/// </summary>
	/// <param name="canvasRect"></param>
	public void SetCanvasRect(RectTransform canvasRect) {
		this.canvasRect = canvasRect;
	}
}
