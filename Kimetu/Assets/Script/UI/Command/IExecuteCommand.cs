using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MenuUIにおいて要素を選択中に決定キーが押されると呼び出される。
/// </summary>
public interface IExecuteCommand {
	void OnExecute();
}
