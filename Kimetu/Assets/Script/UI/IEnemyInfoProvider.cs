using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 現在のエネミーの状態を表すデバッグ用文字列を表示するためのインターフェイス。
/// </summary>
public interface IEnemyInfoProvider {
	string informationText { get; }
}
