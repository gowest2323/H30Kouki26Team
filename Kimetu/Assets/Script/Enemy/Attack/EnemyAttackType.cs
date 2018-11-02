using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の攻撃の種類の定義
/// </summary>
public enum EnemyAttackType
{
    SwingDown, //振り下ろし
    HorizontalAttack, //薙ぎ払い
    Beveled, //斜め切り
    Strike, //叩きつけ
    Tackle, //突進
    Thunder, //雷撃
    ZigzagSlash, //ジグザグ斬撃
}

/// <summary>
/// 攻撃の種類とアニメーションのパラメータ名のディクショナリの定義
/// </summary>
public class EnemyAttackTypeDictionary
{
    /// <summary>
    /// 攻撃の種類とアニメーションのパラメータ名のディクショナリ
    /// </summary>
    public static Dictionary<EnemyAttackType, string> dictionary =
        new Dictionary<EnemyAttackType, string>()
        {
            {EnemyAttackType.SwingDown,"Attack" },
            {EnemyAttackType.HorizontalAttack,"Attack" },

        };
}