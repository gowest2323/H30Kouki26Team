using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour {
	//　パーティクルシステム
	private ParticleSystem ps;
	//　ScaleUp用の経過時間
	private float elapsedScaleUpTime = 0f;

	//　Scaleを大きくする間隔時間
	[SerializeField, Header("Scaleを大きくする間隔時間")]
	private float scaleUpTime = 0.03f;

	//　ScaleUpする割合
	[SerializeField, Header("ScaleUpする割合")]
	private float scaleUpParam = 0.1f;

	//　パーティクル削除用の経過時間
	private float elapsedDeleteTime = 0f;

	//　パーティクルを削除するまでの時間
	[SerializeField, Header("パーティクルを削除するまでの時間")]
	private float deleteTime = 5f;

	void Start() {
		ps = GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update() {
		elapsedScaleUpTime += Time.deltaTime;
		elapsedDeleteTime += Time.deltaTime;

		if (elapsedDeleteTime >= deleteTime) {
			Destroy(gameObject);
		}

		if (elapsedScaleUpTime > scaleUpTime) {
			transform.localScale += new Vector3(scaleUpParam, scaleUpParam, scaleUpParam);
			elapsedScaleUpTime = 0f;
		}
	}
}
