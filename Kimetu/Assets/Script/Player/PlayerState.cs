using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの現在の状態。
/// </summary>
public enum PlayerState {
	Idle,
	Move,
	Attack,
	Avoid,
	Defence,
	Dash
}
