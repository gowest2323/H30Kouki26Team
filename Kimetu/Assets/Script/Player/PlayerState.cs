using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの現在の状態。
/// </summary>
public enum PlayerState {
	Idle, //待機　
	Move, //移動注
	Attack, //攻撃時
	Avoid, //回避時
	Defence, //防御時
	Dash, //ダッシュ時
    Counter, //カウンター攻撃時
    Pierce, //吸生中
}
