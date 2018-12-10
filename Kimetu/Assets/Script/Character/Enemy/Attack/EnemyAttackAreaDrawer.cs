using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyAttackAreaDrawer : MonoBehaviour {
	[SerializeField, Tooltip("表示するプレハブ")]
	private GameObject drawAreaObject;
	[SerializeField, Tooltip("フェードアウトにかける時間")]
	private float fadeOutTime = 0.1f;
	[SerializeField, Tooltip("フェードインにかける時間")]
	private float fadeInTime = 0.1f;
	private int floorLayerMask; //床レイヤーのマスク
	private Material areaPrefabMaterial;
	[SerializeField, Range(0.0f, 1.0f), Tooltip("最大アルファ値")]
	private float maxMaterialAlpha;
	private int materialAlphaPropertyID;
	private System.IDisposable coroutine;

	private void Start() {
		drawAreaObject = Instantiate(drawAreaObject);
		drawAreaObject.transform.SetParent(this.transform);
		drawAreaObject.SetActive(false);
		//床レイヤーマスク取得
		floorLayerMask = LayerMask.GetMask(LayerName.Stage.String());
		//Shaderのプロパティ名から固有ID取得
		materialAlphaPropertyID = Shader.PropertyToID("_Alpha");
		areaPrefabMaterial = drawAreaObject.GetComponent<Renderer>().material;
		Assert.IsNotNull(areaPrefabMaterial, "material is null");
	}

	/// <summary>
	/// 描画開始
	/// </summary>
	public void DrawStart() {
		//床にレイを飛ばして床の場所を取得
		Ray ray = new Ray(transform.position, Vector3.down);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayerMask)) {
			//床の上にそのまま置くと表示がちらつくので少し上に配置
			drawAreaObject.transform.position = hit.point + Vector3.up * 0.03f;
		}

		if (coroutine != null) {
			coroutine.Dispose();
		}

		coroutine = Observable.FromCoroutine(() => DrawStartFade())
					.TakeUntilDestroy(this.gameObject).Subscribe();
		//StartCoroutine(DrawStartFade());
	}

	/// <summary>
	/// 描画開始フェードイン
	/// </summary>
	/// <returns></returns>
	private IEnumerator DrawStartFade() {
		float time = 0.0f;
		float alpha = 0.0f;
		float slowDelta = 0.0f;
		drawAreaObject.SetActive(true);
		areaPrefabMaterial.SetFloat(materialAlphaPropertyID, alpha);

		while (time < fadeInTime) {
			slowDelta = Slow.Instance.DeltaTime();
			time += slowDelta;
			float t = time / fadeInTime;
			alpha = Mathf.Lerp(0, maxMaterialAlpha, t);
			areaPrefabMaterial.SetFloat(materialAlphaPropertyID, alpha);
			yield return new WaitForSeconds(slowDelta);
		}
	}

	/// <summary>
	/// 描画終了
	/// </summary>
	public void DrawEnd() {
		if (coroutine != null) {
			coroutine.Dispose();
		}

		coroutine = Observable.FromCoroutine(() => DrawEndFade())
					.TakeUntilDestroy(this.gameObject).Subscribe();
	}

	/// <summary>
	/// 描画終了フェードアウト
	/// </summary>
	/// <returns></returns>
	private IEnumerator DrawEndFade() {
		float time = 0.0f;
		float alpha = maxMaterialAlpha;
		float slowDelta = 0.0f;
		areaPrefabMaterial.SetFloat(materialAlphaPropertyID, alpha);

		while (time < fadeOutTime) {
			slowDelta = Slow.Instance.DeltaTime();
			time += slowDelta;
			float t = 1 - (time / fadeInTime);
			alpha = Mathf.Lerp(0, maxMaterialAlpha, t);
			areaPrefabMaterial.SetFloat(materialAlphaPropertyID, alpha);
			yield return new WaitForSeconds(slowDelta);
		}

		drawAreaObject.SetActive(false);
	}
}
