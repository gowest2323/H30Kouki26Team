using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションをスローにする時、
/// 現在のシーンのすべてのアニメーションを取得するために使います。
/// 実際には MonoBehaivour を継承するクラスでこれを実装してください。
/// </summary>
public interface ICharacterAnimationProvider {
	CharacterAnimation characterAnimation { get; }
}
