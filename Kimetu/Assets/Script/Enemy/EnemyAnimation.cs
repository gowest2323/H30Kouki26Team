using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyAnimation : CharacterAnimation
{

    public void StartRunAnimation()
    {
        animator.SetBool("Run", true);
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
        Assert.IsTrue(Array.Exists(animator.parameters, param => param.name == parameterName),parameterName+"が存在しません");
        animator.SetTrigger(parameterName);
    }

    public void StratDamageAnimation()
    {
        animator.SetTrigger("Damage");
    }

}
