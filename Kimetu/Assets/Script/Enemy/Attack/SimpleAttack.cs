using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の単純な攻撃（コライダーのオンオフで表現できる）
/// </summary>
public class SimpleAttack : EnemyAttack, IAttackEventHandler
{
    [SerializeField, Header("攻撃の種類")]
    private EnemyAttackType attackType;
    private int hitCount;

    public override IEnumerator Attack()
    {
        enemyAnimation.StartAttackAnimation(attackType);
        yield return new WaitWhile(() =>
        {
            AnimatorStateInfo info = enemyAnimation.anim.GetCurrentAnimatorStateInfo(0);
            return info.fullPathHash != Animator.StringToHash("Base Layer.oni@attack");
        });
        yield return new WaitWhile(() => !enemyAnimation.IsEndAnimation(0.02f));
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if(hitCount > 0) {
            return;
        }
        if (TagNameManager.Equals(collider.tag, TagName.Player))
        {
            hitCount++;
            DamageSource damage = new DamageSource(collider.ClosestPoint(this.transform.position),
                power, holderEnemy);
            collider.GetComponent<PlayerAction>().OnHit(damage);
        }
    }

    public void AttackStart() {
        Debug.Log("ST");
        this.hitCount = 0;
        GetComponent<Collider>().enabled = true;
    }
	public void AttackEnd() {
        Debug.Log("ED");
        GetComponent<Collider>().enabled = false;
    }
}
