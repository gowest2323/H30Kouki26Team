using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の単純な攻撃（コライダーのオンオフで表現できる）
/// </summary>
public class SimpleAttack : EnemyAttack
{
    [SerializeField, Header("攻撃の種類")]
    private EnemyAttackType attackType;

    public override IEnumerator Attack()
    {
        enemyAnimation.StartAttackAnimation(attackType);
        yield return null;
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
