using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyThunderAttackAreaDrawer : EnemyAttackAreaDrawer {
	[SerializeField]
	private List<GameObject> drawObjects;
	private float[] directions;
	public Vector3 rayCastPosition { get; set; }
	public void SetRaycastPosition(Vector3 position) {
		this.rayCastPosition = position;
		drawObjects.ForEach(obj => obj.transform.position = position);
	}
	// Use this for initialization
	protected override void Start() {
		for (int i = 0; i < drawObjects.Count; i++) {
			drawObjects[i] = Instantiate(drawObjects[i], this.transform);
			drawObjects[i].transform.SetParent(null);
		}

		//床レイヤーマスク取得
		floorLayerMask = LayerMask.GetMask(LayerName.Stage.String());
		//Shaderのプロパティ名から固有ID取得
		materialAlphaPropertyID = Shader.PropertyToID("_Alpha");
		areaPrefabMaterial = drawObjects[0].GetComponent<MeshRenderer>().material;
		Assert.IsNotNull(areaPrefabMaterial, "material is null");
		drawObjects.ForEach(obj => obj.SetActive(false));
	}

	/// <summary>
	/// 描画開始
	/// </summary>
	public override void DrawStart() {
		//床にレイを飛ばして床の場所を取得
		Ray ray = new Ray(rayCastPosition + Vector3.up, Vector3.down);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayerMask)) {
			float y = hit.point.y + 0.03f;

			foreach (var obj in drawObjects) {
				Vector3 pos = obj.transform.position;
				pos.y = y;
				obj.transform.position = pos;
				obj.transform.SetParent(null);
			}

			drawObjects.ForEach(obj => obj.transform.SetParent(null));
		}

		if (coroutine != null) {
			CoroutineManager.Instance.StopCoroutineEx(coroutine);
		}

		drawObjects.ForEach(obj => obj.SetActive(true));
		coroutine = CoroutineManager.Instance.StartCoroutineEx(DrawStartFade());
	}

	/// <summary>
	/// 描画終了フェードアウト
	/// </summary>
	/// <returns></returns>
	protected override IEnumerator DrawEndFade() {
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

		drawObjects.ForEach(obj => {
			//obj.transform.SetParent(this.transform);
			obj.SetActive(false);

		});
	}
}
