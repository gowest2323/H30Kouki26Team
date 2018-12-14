using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の攻撃の種類の定義
/// </summary>
public enum EnemyAttackType {
	SwingDown, //振り下ろし
	NagiharaiAttack, //薙ぎ払い
	Kiriage, //切り上げ
	Beveled, //斜め切り
	Strike, //叩きつけ
	Tackle, //突進
	Thunder, //雷撃
	ZigzagSlash, //ジグザグ斬撃
	AirSlash, //空中斬撃
	RotationSlash, //回転斬り
	TwiceSlash, //二度斬り
	HurimawashiAruki, //振り回し歩き
}

/// <summary>
/// 攻撃の種類とアニメーションのパラメータ名のディクショナリの定義
/// </summary>
public class EnemyAttackTypeDictionary {
	/// <summary>
	/// 攻撃の種類とアニメーションのパラメータ名のディクショナリ
	/// </summary>
	public static Dictionary<EnemyAttackType, string> dictionary =
	new Dictionary<EnemyAttackType, string>() {
		{EnemyAttackType.SwingDown, "Hurioroshi" },
		{EnemyAttackType.NagiharaiAttack, "Nagiharai" },
		{EnemyAttackType.Kiriage, "Kiriage" },
		{EnemyAttackType.Beveled, "Attack" },
		{EnemyAttackType.Strike, "Attack" },
		{EnemyAttackType.Tackle, "Attack" },
		{EnemyAttackType.ZigzagSlash, "Attack" },
		{EnemyAttackType.AirSlash, "Attack" },
		{EnemyAttackType.RotationSlash, "RotationSlash" },
		{EnemyAttackType.TwiceSlash, "TwiceSlash" },
		{EnemyAttackType.HurimawashiAruki, "HurimawashiAruki" },

	};

	/// <summary>
	/// 攻撃の種類とそれに対応したIEventHook
	/// </summary>
	public static Dictionary<EnemyAttackType, System.Type> typeDictionary =
	new Dictionary<EnemyAttackType, System.Type>() {
		{EnemyAttackType.SwingDown, typeof(OniAttackHook) },
		{EnemyAttackType.NagiharaiAttack, typeof(OniNagiHook) },
		{EnemyAttackType.TwiceSlash, typeof(OniComboHook) },
		{EnemyAttackType.Kiriage, typeof(OniKiriageHook) },
		{EnemyAttackType.RotationSlash, typeof(OniKiriageHook) },
		{EnemyAttackType.HurimawashiAruki, typeof(OniNagiHook) },
	};
}