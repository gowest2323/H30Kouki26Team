using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyAnimation : CharacterAnimation
{
    public Animator anim { get { return animator; } }
    public void StartRunAnimation()
    {
        animator.SetBool("Run", true);
    }

    public void StartDeathAnimation()
    {
        animator.SetBool("Dead", true);
    }

    public void StopRunAnimation()
    {
        animator.SetBool("Run", false);
    }

    /// <summary>
    /// 攻撃アニメーションの開始
    /// </summary>
    /// <param name="attackType">攻撃の種類</param>
    public void StartAttackAnimation(EnemyAttackType attackType)
    {
        Assert.IsTrue(EnemyAttackTypeDictionary.dictionary.ContainsKey(attackType), attackType + "が定義されていません");
        //パラメータ名を取得
        string parameterName = EnemyAttackTypeDictionary.dictionary[attackType];
        Assert.IsTrue(Array.Exists(animator.parameters, param => param.name == parameterName), parameterName + "が存在しません");
        animator.SetTrigger(parameterName);
    }

    /// <summary>
    /// ダメージを受けた時のアニメーション再生
    /// </summary>
    public void StartDamageAnimation()
    {
        animator.SetTrigger("Damage");
    }

    /// <summary>
    /// はじかれたときのアニメーション再生
    /// </summary>
    public void StartReplAnimation()
    {
        animator.SetTrigger("Repl");
    }

}
