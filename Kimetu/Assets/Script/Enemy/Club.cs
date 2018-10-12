using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : Weapon
{
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 攻撃開始
    /// </summary>
    public override void AttackStart()
    {
        weaponCollider.enabled = true;
    }

    /// <summary>
    /// 攻撃終了
    /// </summary>
    public override void AttackEnd()
    {
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーと当たったらプレイヤーにダメージ
        if (TagNameManager.Equals(other.tag, TagName.Player))
        {
            //衝突したときの最近点を衝突点とする
            Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
            DamageSource damage = new DamageSource(hitPos, power, holderObjectDamagable);
            //相手に当たったと通知
            other.gameObject.GetComponent<IDamageable>().OnHit(damage);
        }
    }
}
