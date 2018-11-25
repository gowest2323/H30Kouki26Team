using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WindowsとMacでは入力方法が異なるため、
/// このクラスではその二つを外部から同じ方法で扱えるように入力のマッピングを行います。
/// </summary>
public static class InputMap {
	/// <summary>
	/// 入力方法。
	/// </summary>
	public enum Type {
		//A,B,X,Y ボタン
		AButton,
		BButton,
		XButton,
		YButton,
		//左、右スティックの軸方向
		LStick_Horizontal,
		LStick_Vertical,
		RStick_Horizontal,
		RStick_Vertical,
		//LRボタン
		LButton,
		RButton,
		//LRTrigger
		LTrigger,
		RTrigger,
		//Start Back
		StartButton,
		BackButton,
		//スティックの垂直押し込み
		LStickClick,
		RStickClick
	}
	private static readonly Dictionary<Type, string> macBinding;
	private static readonly Dictionary<Type, string> winBinding;
	static InputMap() {
		macBinding = new Dictionary<Type, string>() {
			{Type.LStick_Horizontal, "MAC_Horizontal_L"},
			{Type.LStick_Vertical, "MAC_Vertical_L"},
			{Type.RStick_Horizontal, "MAC_Horizontal_R"},
			{Type.RStick_Vertical, "MAC_Vertical_R"},
			{Type.AButton, "MAC_AButton"},
			{Type.BButton, "MAC_BButton"},
			{Type.XButton, "MAC_XButton"},
			{Type.YButton, "MAC_YButton"},
			{Type.LButton, "MAC_LButton"},
			{Type.RButton, "MAC_RButton"},
			{Type.StartButton, "MAC_StartButton"},
			{Type.BackButton, "MAC_BackButton"},
			{Type.LStickClick, "MAC_LStickClick"},
			{Type.RStickClick, "MAC_RStickClick"},
		};
		winBinding = new Dictionary<Type, string>() {
			{Type.LStick_Horizontal, "WIN_Horizontal_L"},
			{Type.LStick_Vertical, "WIN_Vertical_L"},
			{Type.RStick_Horizontal, "WIN_Horizontal_R"},
			{Type.RStick_Vertical, "WIN_Vertical_R"},
			{Type.AButton, "WIN_AButton"},
			{Type.BButton, "WIN_BButton"},
			{Type.XButton, "WIN_XButton"},
			{Type.YButton, "WIN_YButton"},
			{Type.LButton, "WIN_LButton"},
			{Type.RButton, "WIN_RButton"},
			{Type.StartButton, "WIN_StartButton"},
			{Type.BackButton, "WIN_BackButton"},
			{Type.LStickClick, "WIN_LStickClick"},
			{Type.RStickClick, "WIN_RStickClick"},
		};
		foreach(var e in System.Enum.GetValues(typeof(Type))) {
			Type type = (Type)e;
			if(!macBinding.ContainsKey(type)) macBinding[type] = "NULL";
			if(!winBinding.ContainsKey(type)) winBinding[type] = "NULL";
		}
	}

	/// <summary>
	/// 指定の入力方法に対応する、実行時のプラットフォームで動作する入力キーを返します。
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static string GetInputName(this Type type) {
		#if UNITY_STANDALONE_WIN
			return winBinding[type];
		#elif UNITY_STANDALONE_OSX
			return macBinding[type];
		#else
			return winBinding[type];
		#endif
	}

	/// <summary>
	/// Dパッドの水平方向の入力状態を返します。
	/// </summary>
	/// <returns></returns>
	public static float GetDPadHorizontal() {
		#if UNITY_STANDALONE_OSX
			if(Input.GetButtonDown("MAC_DPAD_LEFT")) {
				return -1;
			}
			if(Input.GetButtonDown("MAC_DPAD_RIGHT")) {
				return 1;
			}
			return 0;
		#else
			return Input.GetAxis("WIN_DPAD_HORIZONTAL");
		#endif
	}

	/// <summary>
	/// Dパッドの垂直方向の入力状態を返します。
	/// </summary>
	/// <returns></returns>
	public static float GetDPadVertical() {
		#if UNITY_STANDALONE_OSX
			if(Input.GetButtonDown("MAC_DPAD_UP")) {
				return -1;
			}
			if(Input.GetButtonDown("MAC_DPAD_DOWN")) {
				return 1;
			}
			return 0;
		#else
			return Input.GetAxis("WIN_DPAD_VERTICAL");
		#endif
	}
}
