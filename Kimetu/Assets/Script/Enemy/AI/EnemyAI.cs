using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour, IDamageable
{
    protected EnemyState currentState; //現在の状態
    protected EnemyState reserveState; //次の状態の予約
    protected Coroutine currentActionCoroutine; //現在の行動コルーチン
    /// <summary>
    /// 吸生に使われられるか？
    /// </summary>
    public bool canUseHeal { protected set; get; }

    public virtual void UsedHeal()
    {
        canUseHeal = false;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 次の行動を決定する
    /// </summary>
    /// <returns></returns>
    protected abstract Coroutine Think();
    public abstract void OnHit(DamageSource damageSource);
    public abstract void Countered();

    /// <summary>
    /// ダメージを適用します。
    /// </summary>
    /// <param name="damageSource"></param>
    protected void ApplyDamage(DamageSource damageSource) {
        var status = GetComponent<Status>();
        var isBoss = GetComponent<BossMarker>() != null;
        //ボスははじかないと死なない
        if(isBoss) {
            status.Damage(damageSource.damage, (Slow.Instance.isSlowNow ? DamageMode.Kill : DamageMode.NotKill));
        } else {
            status.Damage(damageSource.damage, DamageMode.Kill);
        }
    }
}
