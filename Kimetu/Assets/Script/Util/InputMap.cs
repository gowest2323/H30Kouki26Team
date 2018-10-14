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
		RTrigger
	}
	private static readonly Dictionary<Type, string> macBinding;
	private static readonly Dictionary<Type, string> winBinding;
	static InputMap() {
		macBinding = new Dictionary<Type, string>();
		winBinding = new Dictionary<Type, string>();
		foreach(var e in System.Enum.GetValues(typeof(Type))) {
			Type type = (Type)e;
			macBinding[type] = "NULL";
			winBinding[type] = "NULL";
		}
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
		};
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
}
