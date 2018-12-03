using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowColorChanger : MonoBehaviour {
	[SerializeField, Range(0, 1)]
	private float maxAlpha; //色替えの最大アルファ値
	private Image image; //イメージオブジェクト
	private Coroutine coroutine; //色替えのコルーチン
	private float slowColorChangeTime = 0.2f; //スロー時の色替えの時間

	// Use this for initialization
	void Start() {
		image = GetComponent<Image>();
	}

	/// <summary>
	/// スロー開始
	/// </summary>
	public void SlowStart() {
		if (coroutine != null) {
			StopCoroutine(coroutine);
		}

		coroutine = StartCoroutine(SlowStartCoroutine(slowColorChangeTime));
	}

	/// <summary>
	/// スロー終了
	/// </summary>
	public void SlowEnd() {
		if (coroutine != null) {
			StopCoroutine(coroutine);
		}

		coroutine = StartCoroutine(SlowEndCoroutine(slowColorChangeTime));
	}

	private IEnumerator SlowStartCoroutine(float slowStartColorChangeTime) {
		float time = 0.0f;
		Color color = image.color;

		while (time < slowStartColorChangeTime) {
			color.a = Mathf.Lerp(0, maxAlpha, (time / slowStartColorChangeTime));
			image.color = color;
			time += Time.deltaTime;
			yield return null;
		}

		color.a = maxAlpha;
		image.color = color;
	}

	private IEnumerator SlowEndCoroutine(float slowEndColorChangeTime) {
		float time = 0.0f;
		Color color = image.color;

		while (time < slowEndColorChangeTime) {
			color.a = Mathf.Lerp(0, maxAlpha, 1 - (time / slowEndColorChangeTime));
			image.color = color;
			time += Time.deltaTime;
			yield return null;
		}

		color.a = 0;
		image.color = color;
	}

	private void OnDestroy() {
		if (coroutine != null) {
			StopCoroutine(coroutine);
			coroutine = null;
		}
	}
}
