using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// シーンをまたいで存在可能なオブジェクト。
/// </summary>
/// <typeparam name="T"></typeparam>
public class CachedMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
	public static T Instance { private set; get; }

	void Awake() {
		if(Instance == null) {
			Instance = (T)FindObjectOfType (typeof(T));
			GameObject.DontDestroyOnLoad(Instance);
			StartUp();
		} else {
			GameObject.Destroy(gameObject);
		}
	}

	protected virtual void StartUp() {
	}
}
