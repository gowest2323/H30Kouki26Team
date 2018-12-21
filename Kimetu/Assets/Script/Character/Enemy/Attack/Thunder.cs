using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Thunder : MonoBehaviour, IDamageable {
	private Quaternion defaultRotation;
	private List<ThunderExtend> thunders;
	private float thunderLength;
	[SerializeField]
	private float scale = 1.0f;
	private int layerMask;
	private int power;
	private bool isHit;
	private void Awake() {
		defaultRotation = transform.rotation;
		thunders = new List<ThunderExtend>();
		//子オブジェクトからThunderExtendを取得
		ThunderExtend[] thunderChildren = transform.GetComponentsInChildren<ThunderExtend>();

		foreach (var child in thunderChildren) {
			thunders.Add(child);
		}

		layerMask = LayerMask.GetMask(LayerName.PlayerDamageable.String());
		isHit = false;
	}
	public void Init(int power) {
		isHit = false;
		this.power = power;
		this.transform.rotation = defaultRotation;
		thunders.ForEach(t => t.Init());
	}

	private void Update() {
		if (isHit) return;

		RaycastHit hit;

		if (Physics.BoxCast(this.transform.position, new Vector3(scale, 1, 1), this.transform.right, out hit, Quaternion.identity, thunderLength, layerMask)) {
			Transform playerTransform = hit.transform;
			DamageSource damage = new DamageSource(hit.point, power, this, false);
			PlayerAction player = playerTransform.GetComponent<PlayerAction>();
			Assert.IsNotNull(player, "PlayerActionが取得できませんでした。");
			player.OnHit(damage);
			Debug.Log("hit player");
			isHit = true;
		} else {
			Debug.Log("NO hit");
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawRay(this.transform.position + new Vector3(0, 1, 0), this.transform.right * thunderLength);
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="length">長さ</param>
	public void UpdateThunder(float length) {
		thunderLength = length * 2;
		thunders.ForEach(t => t.Extend(length));
	}

	public void OnHit(DamageSource damageSource) {

	}

	public void Countered() {
	}
}
