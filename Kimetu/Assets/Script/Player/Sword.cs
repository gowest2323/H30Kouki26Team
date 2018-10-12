using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private int attackNum; //現在の攻撃回数
    private PlayerAnimation playerAnimation;

    public void SetPlayerAnimation(PlayerAnimation animation)
    {
        this.playerAnimation = animation;
    }

    /// <summary>
    /// 攻撃開始時に呼ばれる
    /// </summary>
    public override void AttackStart()
    {
        attackNum++;
    }

    /// <summary>
    /// 攻撃終了時に呼ばれる
    /// </summary>
    public override void AttackEnd()
    {
        attackNum = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        //敵に当たったら通知する
        if (TagNameManager.Equals(other.tag, TagName.Enemy))
        {
            Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
            DamageSource damage = new DamageSource(hitPos, power, holder);
            other.gameObject.GetComponent<IDamageable>().OnHit(damage);
        }
    }
}
