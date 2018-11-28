﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private int attackNum; //現在の攻撃回数
    private PlayerAnimation playerAnimation; //プレイヤーのアニメーション管理
    private Dictionary<GameObject, int> countDict;

    protected override void Start()
    {
        base.Start();
        this.countDict = new Dictionary<GameObject, int>();
    }

    /// <summary>
    /// プレイヤーのアニメーション管理クラスの設定
    /// </summary>
    public void SetPlayerAnimation(PlayerAnimation animation)
    {
        this.playerAnimation = animation;
    }

    /// <summary>
    /// 攻撃開始時に呼ばれる
    /// </summary>
    public override void AttackStart()
    {
        countDict.Clear();
        attackNum++;
        weaponCollider.enabled = true;
        //playerAnimation.StartAttackAnimation();
    }

    /// <summary>
    /// 攻撃終了時に呼ばれる
    /// </summary>
    public override void AttackEnd()
    {
        attackNum = 0;
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //敵に当たったら通知する
        if (TagNameManager.Equals(other.tag, TagName.Enemy))
        {
            //攻撃が複数回ヒットしないように
            if(!countDict.ContainsKey(other.gameObject)) {
                countDict[other.gameObject] = 1;
            } else if(countDict[other.gameObject] >= 1) {
                return;
            }
            //衝突したときの最近点を衝突点とする
            Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
            DamageSource damage = new DamageSource(hitPos, power, holderObjectDamagable);
            //相手に当たったと通知
            other.gameObject.GetComponent<IDamageable>().OnHit(damage);
        }
    }
}