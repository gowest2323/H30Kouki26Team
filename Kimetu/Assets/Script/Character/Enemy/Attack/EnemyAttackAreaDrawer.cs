using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyAttackAreaDrawer : MonoBehaviour {
	[SerializeField, Tooltip("表示するプレハブ")]
	protected GameObject drawAreaObject;
	[SerializeField, Tooltip("フェードアウトにかける時間")]
    protected float fadeOutTime = 0.1f;
	[SerializeField, Tooltip("フェードインにかける時間")]
    protected float fadeInTime = 0.1f;
    protected int floorLayerMask; //床レイヤーのマスク
    protected Material areaPrefabMaterial;
	[SerializeField, Range(0.0f, 1.0f), Tooltip("最大アルファ値")]
    protected float maxMaterialAlpha;
	[SerializeField, Header("攻撃時に範囲をその場所に残しておくか")]
    protected bool toReleaseParent = false;
    protected int materialAlphaPropertyID;
    protected Coroutine coroutine;

    protected virtual void Start() {
		drawAreaObject = Instantiate(drawAreaObject, this.transform);
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
	public virtual void DrawStart() {
		//床にレイを飛ばして床の場所を取得
		Ray ray = new Ray(transform.position, Vector3.down);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayerMask)) {
			//床の上にそのまま置くと表示がちらつくので少し上に配置
			drawAreaObject.transform.position = hit.point + Vector3.up * 0.03f;

			if (toReleaseParent) {
				drawAreaObject.transform.SetParent(null);
			}
		}

		if (coroutine != null) {
			CoroutineManager.Instance.StopCoroutineEx(coroutine);
		}

		coroutine = CoroutineManager.Instance.StartCoroutineEx(DrawStartFade());
	}

    /// <summary>
    /// 描画開始フェードイン
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DrawStartFade() {
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
	public virtual void DrawEnd() {
		if (coroutine != null) {
			CoroutineManager.Instance.StopCoroutineEx(coroutine);
		}

		coroutine = CoroutineManager.Instance.StartCoroutineEx(DrawEndFade());
	}

    /// <summary>
    /// 描画終了フェードアウト
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DrawEndFade() {
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

		drawAreaObject.transform.SetParent(this.transform);
		drawAreaObject.SetActive(false);
	}
}
