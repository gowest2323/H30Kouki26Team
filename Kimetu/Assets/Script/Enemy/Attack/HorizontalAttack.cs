using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HorizontalAttack : EnemyAttack
{
    private System.IDisposable observer;
    protected override void Start() {
        base.Start();
        var attackHook = GetComponentInParent<OniNagiHook>();
        this.observer = attackHook.trigger.Subscribe((e) => {
            if(e) { AttackStart(); }
            else { AttackEnd(); }
        });
    }

    private void OnDestroy() {
        //イベントの購読を終了
        observer.Dispose();
    }

    public override IEnumerator Attack()
    {
        Debug.Log("薙ぎ払い攻撃開始");
        enemyAnimation.StartAttackAnimation(EnemyAttackType.NagiharaiAttack);
        attackCollider.enabled = true;
        //アニメーション終了まで待機
        yield return new WaitWhile(() => !enemyAnimation.IsEndAnimation(0.02f));
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
