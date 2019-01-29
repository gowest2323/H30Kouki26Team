using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour {
	[SerializeField]
	private Slider slider;
	[SerializeField]
	private Text text;
	private EnemyStatus status;
	private IEnemyInfoProvider provider;

	private void Awake() {
		//本番環境では無効化
		#if !(UNITY_EDITOR)
		if (text != null) {
			text.gameObject.SetActive(false);
		}

		#endif
	}

	// Use this for initialization
	void Start () {
		status = gameObject.GetComponentInParent<EnemyStatus>();
		this.provider = GetComponentInParent<IEnemyInfoProvider>();
	}
	// Update is called once per frame
	void Update () {
		slider.value = status.GetHPRatio();
		slider.transform.LookAt(Camera.main.transform.transform, Vector3.up);
		slider.transform.Rotate(new Vector3(0, 180, 0));
		#if UNITY_EDITOR
		/*
				if (provider != null) {
					text.text = provider.informationText;
				}

				text.transform.LookAt(Camera.main.transform.transform, Vector3.up);
				text.transform.Rotate(new Vector3(0, 180, 0));
				*/
		#endif
	}
}
