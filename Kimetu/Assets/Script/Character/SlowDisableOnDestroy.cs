using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトが破棄される前に Slow から自身を削除します。
/// </summary>
public class SlowDisableOnDestroy : MonoBehaviour {
	private CharacterAnimation characterAnimation;

	// Use this for initialization
	void Start () {
		this.characterAnimation = GetComponent<CharacterAnimation>();
	}

	// Update is called once per frame
	void Update () {

	}

	private void OnDestroy() {
		if (Slow.Instance != null && characterAnimation != null) {
			Slow.Instance.Remove(characterAnimation);
		}
	}
}
