using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalRegistry {
	/// <summary>
	/// 唯一のインスタンス。
	/// </summary>
	/// <value>The instance.</value>
	public static TerminalRegistry instance {
		get {
			if (mInstance == null) {
				mInstance = new TerminalRegistry();
			}

			return mInstance;
		}
	}
	private static TerminalRegistry mInstance;

	private Dictionary<string, System.Action<string[]>> commandDict;

	private TerminalRegistry() {
		this.commandDict = new Dictionary<string, System.Action<string[]>>();
	}

	/// <summary>
	/// 指定のコマンドを登録します。
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="func">Func.</param>
	public void Register(string name, System.Action<string[]> func) {
		commandDict[name] = func;
	}

	/// <summary>
	/// 指定のコマンドを削除します。
	/// </summary>
	/// <param name="name">Name.</param>
	public void Unregister(string name) {
		commandDict.Remove(name);
	}

	/// <summary>
	/// 指定のコマンドを実行します。
	/// </summary>
	/// <returns>The invoke.</returns>
	/// <param name="name">Name.</param>
	/// <param name="args">Arguments.</param>
	public bool Invoke(string name, string[] args) {
		#if UNITY_EDITOR

		if (commandDict.ContainsKey(name)) {
			commandDict[name](args);
			return true;
		}

		#endif
		return false;
	}
}
