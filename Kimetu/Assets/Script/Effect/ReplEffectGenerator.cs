using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾きエフェクトを生成するためのクラス。
/// </summary>
public class ReplEffectGenerator : MonoBehaviour {
	[SerializeField]
	private int repeatCount = 1;

	[SerializeField]
	private float generateWait = 1.5f;

	[SerializeField]
	private GameObject particlePrefab;

	private bool generating;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	/// <summary>
	/// エフェクトの生成を開始します。
	/// </summary>
	/// <param name="position"></param>
	public void StartGenerate(Vector3 position) {
		if (generating) {
			return;
		}

		StartCoroutine(GenerateUpdate(position));
	}

	private IEnumerator GenerateUpdate(Vector3 position) {
		this.generating = true;

		for (int i = 0; i < repeatCount; i++) {
			var obj = GameObject.Instantiate(particlePrefab);
			var particleSystem = obj.GetComponent<ParticleSystem>();
			obj.transform.parent = transform;
			obj.transform.position = position;
			//パーティクルの1ループの長さだけ待機
			GameObject.Destroy(obj, particleSystem.main.duration);
			yield return new WaitForSeconds(generateWait);
		}

		this.generating = false;
	}
}
