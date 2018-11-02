using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour {
	[SerializeField]
	private Camera target;

	public bool isVisible { private set; get; }

	// Use this for initialization
	void Start () {
		if(target == null) {
			this.target = Camera.main;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//https://qiita.com/edo_m18/items/8a354d3099fc799c97ff
		Matrix4x4 V = target.worldToCameraMatrix;
		Matrix4x4 P = target.projectionMatrix;
		Matrix4x4 VP = P * V;
		var p = transform.position;
		Vector4 pos = VP * new Vector4(p.x, p.y, p.z, 1.0f);

		if (pos.w == 0)
		{
			isVisible = true;
			return;
		}

		float x = pos.x / pos.w;
		float y = pos.y / pos.w;
		float z = pos.z / pos.w;
		if (x < -1.0f)
		{
			isVisible = false;
			return;
		}
		if (x > 1.0f)
		{
			isVisible = false;
			return;
		}

		if (y < -1.0f)
		{
			isVisible = false;
			return;
		}
		if (y > 1.0f)
		{
			isVisible = false;
			return;
		}

		if (z < -1.0f)
		{
			isVisible = false;
			return;
		}

		if (z > 1.0f)
		{
			isVisible = false;
			return;
		}
		isVisible = true;
	}
}
