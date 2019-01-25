using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドローコールを減らすため、遠いオブジェクトのレンダラを無効にする。
/// </summary>
public class HideObject : MonoBehaviour {
	private GameObject cameraObject;
	private Vector3 lastPos;
	private Quaternion lastRot;
	private List<MeshRenderer> stageObjectList;
	private float viewDistance;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private float limitDistance = 30f;

	// Use this for initialization
	void Start () {
		this.cameraObject = Camera.main.gameObject;
		this.lastPos = transform.position;
		this.stageObjectList = new List<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		//*
		//ほとんど動いていないので更新しない
		if(Vector3.Distance(lastPos, transform.position) < 1f &&
		   Quaternion.Angle(lastRot, transform.rotation) < 1f) {
			return;
		}
		GetStageObjectList();
		var clone = new List<MeshRenderer>(stageObjectList);
		//最初に全て有効にする
		foreach(var obj in stageObjectList) {
			obj.enabled = true;
		}
		//プレイヤーと近いやつは全てのこす
		this.viewDistance = GetMaxRayHitDistance();
		clone.RemoveAll((e) => Vector3.Distance(e.transform.position, transform.position) < viewDistance);
		foreach(var obj in clone) {
			obj.enabled = false;
		}
		this.lastPos = transform.position;
		this.lastRot = transform.rotation;
		//*/
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, viewDistance);
	}

	/// <summary>
	/// 一周するようにレイを撃ってもっとも遠くで衝突した時の距離を返す。
	/// </summary>
	/// <returns>The max ray hit distance.</returns>
	private float GetMaxRayHitDistance() {
		var dist = -1f;
		for(float f=0; f<360; f+=20f) {
			var r = Mathf.Deg2Rad * f;
			var vx = Mathf.Cos(r);
			var vz = Mathf.Sin(r);
			var v = new Vector3(vx, 0, vz);
			var hitDist = GetRayHitDistance(v);
			if (dist < hitDist) {
				dist = hitDist;
			}
			Debug.Log("ray " + f);
		}
		if(dist < limitDistance) {
			dist = limitDistance;
		}
		return dist;
	}

	/// <summary>
	/// 指定の方向にレイを撃った場合どれぐらい先で衝突するか。
	/// </summary>
	/// <returns>The ray hit distance.</returns>
	/// <param name="dir">Dir.</param>
	private float GetRayHitDistance(Vector3 dir) {
		RaycastHit hit;
		if(Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, LayerMask.GetMask(LayerName.Stage.String()))) {
			return Vector3.Distance(transform.position, hit.collider.transform.position);
		}
		return 100f;
	}

	private void GetStageObjectList() {
		if(stageObjectList.Count > 0) { return; }
		//Stageレイヤーのオブジェクトを収集
		this.stageObjectList = Utilities.GetComponentsFromAllObject<MeshRenderer>();
		Debug.Log("stage objects a:" + stageObjectList.Count);
		stageObjectList.RemoveAll((e) => e.gameObject.tag == TagName.Player.String());
		stageObjectList.RemoveAll((e) => !(mask.value == (mask.value | (1 << e.gameObject.layer))));
		Debug.Log("stage objects b:" + stageObjectList.Count);
	}
}
