using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour {
	[SerializeField]
	private Camera target;

	[SerializeField]
	private bool debugMode = false;

	public bool isVisible { private set; get; }
	private GameObject player;

	// Use this for initialization
	void Start () {
		if (target == null) {
			this.target = Camera.main;
		}
	}

	// Update is called once per frame
	void Update () {
		CheckVisible();

		if (debugMode) {
			Debug.Log("isVisible: " + isVisible);
		}
	}

	/// <summary>
	/// カメラからこのオブジェクトへレイを打って当たるなら true.
	/// </summary>
	/// <returns></returns>
	public bool IsHitRay() {
		if (player == null) {
			this.player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		}

		//ちょっとだけプレイヤーのコライダーを無効にする
		var collider = player.GetComponent<Collider>();
		collider.enabled = false;

		try {
			var cameraPos = target.transform.position;
			var lookPos = transform.position;
			var dir = (lookPos - cameraPos).normalized;
			RaycastHit hit;

			if (Physics.Raycast(new Ray(cameraPos, dir), out hit)) {
				return hit.collider.gameObject == gameObject;
			}

			return false;
		} finally {
			collider.enabled = true;
		}
	}

	private void CheckVisible() {
		//https://qiita.com/edo_m18/items/8a354d3099fc799c97ff
		Matrix4x4 V = target.worldToCameraMatrix;
		Matrix4x4 P = target.projectionMatrix;
		Matrix4x4 VP = P * V;
		var p = transform.position;
		Vector4 pos = VP * new Vector4(p.x, p.y, p.z, 1.0f);

		if (pos.w == 0) {
			isVisible = true;
			return;
		}

		float x = pos.x / pos.w;
		float y = pos.y / pos.w;
		float z = pos.z / pos.w;

		if (x < -1.0f) {
			isVisible = false;
			return;
		}

		if (x > 1.0f) {
			isVisible = false;
			return;
		}

		if (y < -1.0f) {
			isVisible = false;
			return;
		}

		if (y > 1.0f) {
			isVisible = false;
			return;
		}

		if (z < -1.0f) {
			isVisible = false;
			return;
		}

		if (z > 1.0f) {
			isVisible = false;
			return;
		}

		isVisible = true;
	}
}
