using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レンダラーの設定を実行時に変更します。
/// </summary>
public class FixRendererLayer : MonoBehaviour {
	[SerializeField]
	private Renderer target;

	[SerializeField]
	private SortingLayer sortingLayer;

	[SerializeField]
	private int orderInLayer = 0;

	// Use this for initialization
	void Start () {
		if (target == null) {
			this.target = GetComponent<Renderer>();
		}

		target.sortingLayerName = sortingLayer.name;
		target.sortingOrder = orderInLayer;
	}

	// Update is called once per frame
	void Update () {

	}
}
