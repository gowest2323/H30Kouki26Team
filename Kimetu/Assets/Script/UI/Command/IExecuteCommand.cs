using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MenuUIにおいて要素を選択中に決定キーが押されると呼び出される。
/// </summary>
public interface IExecuteCommand {
	/// <summary>
	/// コマンドが終了するまで続行されるコルーチン。
	/// これが実行されている間メニューは選択/決定できません。
	/// </summary>
	/// <returns></returns>
	IEnumerator OnExecute();
}
