using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の攻撃の種類の定義
/// </summary>
public enum EnemyAttackType {
	SwingDown, //振り下ろし
	NagiharaiAttack, //薙ぎ払い
	Combo, //コンボ攻撃
	Kiriage, //切り上げ
	Beveled, //斜め切り
	Strike, //叩きつけ
	Tackle, //突進
	ZigzagSlash, //ジグザグ斬撃
	AirSlash, //空中斬撃
	RotationSlash, //回転斬り
	TwiceSlash, //二度斬り
	HurimawashiAruki, //振り回し歩き
    SetsunaNagiharai, //刹那用薙ぎ払い
	Thunder, //雷撃
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
		{EnemyAttackType.Combo, "Combo" },
		{EnemyAttackType.Beveled, "Attack" },
		{EnemyAttackType.Strike, "Attack" },
		{EnemyAttackType.Tackle, "Attack" },
		{EnemyAttackType.ZigzagSlash, "Attack" },
		{EnemyAttackType.AirSlash, "Attack" },
		{EnemyAttackType.RotationSlash, "RotationSlash" },
		{EnemyAttackType.TwiceSlash, "TwiceSlash" },
		{EnemyAttackType.HurimawashiAruki, "HurimawashiAruki" },
        {EnemyAttackType.Thunder,"Thunder" },
        {EnemyAttackType.SetsunaNagiharai,"Nagiharai" },

	};

	/// <summary>
	/// 攻撃の種類とそれに対応したIEventHook
	/// </summary>
	public static Dictionary<EnemyAttackType, System.Type> typeDictionary =
	new Dictionary<EnemyAttackType, System.Type>() {
		{EnemyAttackType.SwingDown, typeof(OniAttackHook) },
		{EnemyAttackType.NagiharaiAttack, typeof(Oni360_mHook) },
		{EnemyAttackType.TwiceSlash, typeof(OniComboHook) },
		{EnemyAttackType.Kiriage, typeof(OniKiriageHook) },
		{EnemyAttackType.Combo, typeof(OniComboHook) },
		{EnemyAttackType.RotationSlash, typeof(OniKiriageHook) },
		{EnemyAttackType.HurimawashiAruki, typeof(OniNagiHook) },
        {EnemyAttackType.SetsunaNagiharai,typeof(SetunaStand_torchHook) },
        {EnemyAttackType.Thunder,typeof(SetunaGreat_swordHook) },
    };
}