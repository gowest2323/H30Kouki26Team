using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAttack : EnemyAttack
{
    public override IEnumerator Attack()
    {
        Debug.Log("薙ぎ払い攻撃開始");
        enemyAnimation.StartAttackAnimation(EnemyAttackType.HorizontalAttack);
        attackCollider.enabled = true;
        //アニメーション終了まで待機
        yield return new WaitWhile(() => enemyAnimation.IsEndAnimation(0.02f));
        attackCollider.enabled = false;
        Debug.Log("薙ぎ払い攻撃終了");
    }

    protected override void OnHit(Collider collider)
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

}
