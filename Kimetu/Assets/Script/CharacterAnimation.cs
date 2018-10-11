using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public interface CharacterAnimation {
	/// <summary>
	/// アニメーションの速度を 0-1 の範囲で設定します。
	/// </summary>
	/// <value></value>
	float speed {
		set; get;
	}
}
