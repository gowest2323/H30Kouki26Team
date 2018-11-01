using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingDown : EnemyAttack
{

    [SerializeField]
    private float attackTime;

    public override IEnumerator Attack()
    {
        Debug.Log("振り下ろし攻撃開始");
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        attackCollider.enabled = false;
        Debug.Log("振り下ろし攻撃終了");
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
