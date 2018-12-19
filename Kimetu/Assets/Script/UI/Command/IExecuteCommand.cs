using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドの戻り値
/// </summary>
public enum CommandResult {
	//引き続き他のメニューを実行可能です。
	Continue,
	//このコマンドを実行した時点で他のコマンドは使用不可能になります。
	//例えばシーン遷移を伴うコマンドではこれを返すのが適切です。
	Terminate,
}

/// <summary>
/// MenuUIにおいて要素を選択中に決定キーが押されると呼び出される。
/// </summary>
public interface IExecuteCommand {
	CommandResult OnExecute();
}
