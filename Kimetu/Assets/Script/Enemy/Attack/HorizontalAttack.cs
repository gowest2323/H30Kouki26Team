using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAttack : EnemyAttack
{
    [SerializeField]
    private float attackTime;

    public override IEnumerator Attack()
    {
        Debug.Log("薙ぎ払い攻撃開始");
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        attackCollider.enabled = false;
        Debug.Log("薙ぎ払い攻撃終了");
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if (TagNameManager.Equals(collider.tag, TagName.Player))
        {
            DamageSource damage = new DamageSource(collider.ClosestPoint(this.transform.position),
                power, holderEnemy);
            collider.GetComponent<PlayerAction>().OnHit(damage);
        }
    }

}
