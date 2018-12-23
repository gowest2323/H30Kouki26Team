using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendColorRuntime : MonoBehaviour {
	private List<Material> targetMaterials;
	private int colorID, alphaID;

	[SerializeField]
	private bool debugMode = false;

	[SerializeField]
	private Color debugColor = Color.red;

	[SerializeField]
	private float debugAlpha = 0f;


	// Use this for initialization
	void Start () {
		this.targetMaterials = new List<Material>();
		var renderers = GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers) {
			Debug.Log("renderer: " + renderer.gameObject.name);
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
		if(debugMode) {
			SetColor(debugColor);
			SetAlpha(debugAlpha);
		}
		#endif
	}

	public void SetColor(Color color) {
		var flo = new float[]{color.r, color.g, color.b, 1};
		foreach(var mat in targetMaterials) {
			mat.SetFloatArray(colorID, flo);
		}
	}

	public void SetAlpha(float a) {
		foreach(var mat in targetMaterials) {
			mat.SetFloat(alphaID, a);
		}
	}
}
