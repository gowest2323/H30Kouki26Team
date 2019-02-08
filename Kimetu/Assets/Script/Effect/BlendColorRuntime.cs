using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// `BlendColor`シェーダーの色を実行時に変更するためのスクリプト。
/// </summary>
public class BlendColorRuntime : MonoBehaviour {
	private List<Material> targetMaterials;
	private int colorID, alphaID;

	[SerializeField]
	private bool debugMode = false;

	[SerializeField]
	private Color debugColor = Color.red;

	[SerializeField, Range(0, 1)]
	private float debugAlpha = 0f;

	private bool animationNow;


	// Use this for initialization
	void Start () {
		this.targetMaterials = new List<Material>();
		var renderers = GetComponentsInChildren<Renderer>();

		foreach (var renderer in renderers) {
			//Debug.Log("renderer: " + renderer.gameObject.name);
			var materials = renderer.materials;
			var baseName = renderer.gameObject.name;

			foreach (var material in materials) {
				targetMaterials.Add(material);
			}
		}

		this.colorID = Shader.PropertyToID("_BlendColor");
		this.alphaID = Shader.PropertyToID("_BlendAlpha");
	}

	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR

		if (debugMode) {
			SetColor(debugColor);
			SetAlpha(debugAlpha);
		}

		#endif
	}

	public void SetColor(Color color) {
		var flo = new float[] {color.r, color.g, color.b, 1};

		foreach (var mat in targetMaterials) {
			mat.SetFloatArray(colorID, flo);
		}
	}

	public void SetAlpha(float a) {
		foreach (var mat in targetMaterials) {
			mat.SetFloat(alphaID, a);
		}
	}

	public void StartAnimation(Color color, float minAlpha, float maxAlpha, float seconds) {
		if (animationNow) {
			return;
		}

		StartCoroutine(AnimationUpdate(color, minAlpha, maxAlpha, seconds));
	}

	private IEnumerator AnimationUpdate(Color color, float minAlpha, float maxAlpha, float seconds) {
		this.animationNow = true;
		SetColor(color);
		SetAlpha(minAlpha);
		var offset = 0f;

		while (offset < seconds) {
			var t = Time.time;
			yield return new WaitForEndOfFrame();
			offset += (Time.time - t);
			var parcent = offset / seconds;
			var alpha = minAlpha + ((maxAlpha - minAlpha) * parcent);
			SetAlpha(alpha);
			//Debug.Log("offset:" + offset);
		}

		SetAlpha(maxAlpha);
		yield return null;
		SetAlpha(0);
		this.animationNow = false;
	}
}
