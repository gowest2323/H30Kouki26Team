using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特定のユーザにおいてのみ有効になるロガーです。
/// </summary>
public class PrivateLogger {
	/// <summary>
	/// このロガーを使用できるユーザの名前。
	/// </summary>
	/// <value></value>
	public string userName { private set; get; }
	/// <summary>
	/// このロガーが実行時の環境のユーザ名で有効かどうか。
	/// </summary>
	/// <value></value>
	public bool accessible { get { return System.Environment.UserName == this.userName; }}
	public static readonly PrivateLogger KOYA = new PrivateLogger("koya");
	public static readonly PrivateLogger TEST = new PrivateLogger("$test");

	private PrivateLogger(string userName) {
		this.userName = userName;
	}

	/// <summary>
	/// このロガーが有効なら Debug.Log を呼び出します。
	/// </summary>
	/// <param name="message"></param>
	public void Log(object message) {
		if(accessible) {
			Debug.Log(message);
		}
	}


	/// <summary>
	/// このロガーが有効なら Debug.LogWarning を呼び出します。
	/// </summary>
	/// <param name="message"></param>
	public void LogWarning(object message) {
		if(accessible) {
			Debug.LogWarning(message);
		}
	}

	/// <summary>
	/// このロガーが有効なら Debug.LogFormat を呼び出します。
	/// </summary>
	/// <param name="format"></param>
	/// <param name="args"></param>
	public void LogFormat(string format, params object[] args) {
		if(accessible) {
			Debug.LogFormat(format, args);
		}
	}

	/// <summary>
	/// このロガーが有効なら Debug.LogError を呼び出します。
	/// </summary>
	/// <param name="message"></param>
	public void LogError(object message) {
		if(accessible) {
			Debug.LogError(message);
		}
	}
}
