using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 同じ位置に表示される複数の `LongPressUI` を統括するクラス。
/// </summary>
public class LongPressUIServer : MonoBehaviour {
	public bool locked { private set; get; }
	private string currentKey;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// ロックを獲得可能なら
	/// 唯一のキーを生成して true を返します。
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public bool Hold(out string key) {
		key = "";
		if(locked) {
			return false;
		}
		key = Guid.NewGuid().ToString();
		this.currentKey = key;
		this.locked = true;
		return true;
	}

	/// <summary>
	/// ロックを獲得中のキーと引数のキーが同じならロックを解除します。
	/// ロックを解除できなかったなら引数のキーをそのまま返します。
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public string Release(string key) {
		if(key == null || key.Length == 0 || currentKey != key) {
			return key;
		}
		this.locked = false;
		return "";
	}
}
