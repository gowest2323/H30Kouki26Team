using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

/// <summary>
/// ロックオン対象にマーカーをつけるスクリプト。
/// </summary>
public class PointerUI : MonoBehaviour {
	[SerializeField]
	private CameraController cameraController;
	[SerializeField]
	private Image image;
	private FieldInfo nearEnemyInfo;


	// Use this for initialization
	void Start () {
		if(this.image == null) {
			this.image = GetComponentInChildren<Image>();
		}
		if(this.cameraController == null) {
			this.cameraController = Camera.main.gameObject.GetComponentInChildren<CameraController>();
		}
		image.color = image.color * new Vector4(1, 1, 1, 0);
		this.nearEnemyInfo = typeof(CameraController).GetField("nearObj", BindingFlags.NonPublic | BindingFlags.Instance);
	}
	
	// Update is called once per frame
	void Update () {
		var obj = nearEnemyInfo.GetValue(cameraController);
		var color = image.color;
		color.a = (cameraController.IsLockOn() ? 1 : 0);
		image.color = color;

		if(obj != null) {
			var pos = Camera.main.WorldToScreenPoint(((GameObject)obj).transform.position + Vector3.up);
			image.transform.position = pos;
		}
	}
}
