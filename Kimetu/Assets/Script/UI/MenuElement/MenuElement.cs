using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メニューUIの要素
/// </summary>
public class MenuElement : MonoBehaviour {

	// Use this for initialization
	protected virtual void Awake () {

	}

	// Update is called once per frame
	protected virtual void Update () {

	}

	/// <summary>
	/// メニューからこの要素が選択されると呼ばれます。
	/// </summary>
	public virtual void OnFocus() {
	}

	/// <summary>
	/// 非選択状態になると呼ばれます。
	/// </summary>
	public virtual void OnLostFocus() {
	}
}
