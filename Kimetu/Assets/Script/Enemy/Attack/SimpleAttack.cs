using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵の単純な攻撃（コライダーのオンオフで表現できる）
/// </summary>
public class SimpleAttack : EnemyAttack, IAttackEventHandler
{
    [SerializeField, Header("攻撃の種類")]
    private EnemyAttackType attackType;
    private System.IDisposable observer;

    protected override void Start() {
        base.Start();
        var attackHook = GetComponentInParent<OniAttackHook>();
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
        enemyAnimation.StartAttackAnimation(attackType);
        yield return new WaitWhile(() =>
        {
            AnimatorStateInfo info = enemyAnimation.anim.GetCurrentAnimatorStateInfo(0);
            return info.fullPathHash != Animator.StringToHash("Base Layer.oni@attack");
        });
        yield return new WaitWhile(() => !enemyAnimation.IsEndAnimation(0.02f));
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
