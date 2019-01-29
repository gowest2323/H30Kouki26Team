using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayVibration : SingletonMonoBehaviour<PlayVibration> {
	private static bool disableVibrationAtExit = false;

	public void StartVibration(float time) {
		#if UNITY_STANDALONE_WIN

		//アプリケーション終了時にバイブレーションを停止
		if (!disableVibrationAtExit) {
			disableVibrationAtExit = true;
			AppDomain.CurrentDomain.ProcessExit += (sender, e) => {
				XInputDotNetPure.GamePad.SetVibration(0, 0.0f, 0.0f);
			};
		}

		#endif

		StartCoroutine(Play(time));
	}

	/// <summary>
	/// バイブレーション機能
	/// </summary>
	/// <param name="vibrationTime">何秒間バイブレーションするか</param>
	/// <returns></returns>
	private IEnumerator Play(float vibrationTime)

	{
		#if UNITY_STANDALONE_WIN
		XInputDotNetPure.GamePad.SetVibration(0, 1.0f, 1.0f);
		yield return new WaitForSeconds(vibrationTime);
		XInputDotNetPure.GamePad.SetVibration(0, 0.0f, 0.0f);
		#else
		//macでも動かせるはずだけど
		//まだ環境設定できてないのでとりあえず
		yield return new WaitForSeconds(vibrationTime);
		#endif

	}

}
