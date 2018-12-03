using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVibration : SingletonMonoBehaviour<PlayVibration> {

	public void StartVibration(float time) {
		StartCoroutine(Play(time));
	}

	/// <summary>
	/// バイブレーション機能
	/// </summary>
	/// <param name="vibrationTime">何秒間バイブレーションするか</param>
	/// <returns></returns>
	private IEnumerator Play(float vibrationTime)

	{
		XInputDotNetPure.GamePad.SetVibration(0, 1.0f, 1.0f);
		yield return new WaitForSeconds(vibrationTime);
		XInputDotNetPure.GamePad.SetVibration(0, 0.0f, 0.0f);

	}

}
