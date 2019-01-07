using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleMask : MonoBehaviour {
	[SerializeField]
	private bool debugMode;

	[SerializeField]
	private float debugParcent;

	[SerializeField]
	private GameObject back;

	[SerializeField]
	private GameObject text;

	[SerializeField]
	private GameObject fore;

	[SerializeField]
	private float height = 100f;

	private RectTransform foreImage;


	// Use this for initialization
	void Start () {
		this.foreImage = fore.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
		if(debugMode) {
			SetParcent(debugParcent);
		}
#endif
	}

	public void SetParcent(float p) {
		p = 1f - Mathf.Clamp01(p);
		if(foreImage == null) {
			this.foreImage = fore.GetComponent<RectTransform>();
		}
		var curPos = foreImage.anchoredPosition;
		var newPos = curPos;
		newPos.y = (height * p * -1);
		foreImage.anchoredPosition = newPos;
		//fore.transform.position = newPos;
	}
}
