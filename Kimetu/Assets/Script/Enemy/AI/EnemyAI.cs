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

    /// <summary>
    /// はじきによって殺されたなら true.
    /// </summary>
    /// <value></value>
    public bool deathByRepl { private set; get; }

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
        if(Slow.Instance.isSlowNow && status.IsDead()) {
            this.deathByRepl = true;
        }
    }

    /// <summary>
    /// アニメーションが終了するまで待機したあとビームを発射します。
    /// </summary>
    /// <returns></returns>
    protected void ShowBeam() {
        StartCoroutine(StartShowBeam());
    }

    /// <summary>
    /// 鬼の体を五秒かけて黒くしたあと消します。
    /// </summary>
    protected void Extinction() {
        StartCoroutine(StartExtinction());
    }

    private IEnumerator StartExtinction() {
        var hook = GetComponentInParent<OniDeadHook>();
        yield return hook.Wait();
        var offset = 0f;
        var seconds = 5f;
        var separate = 100;
        var mat = GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
        var start = mat.color;
        var end = Color.black;
        while(offset < seconds) {
            yield return new WaitForSeconds(seconds / separate);
            offset += (seconds / separate);
            mat.color = Color.Lerp(start, end, offset / seconds);
        }
        mat.color = Color.black;
        yield return new WaitForEndOfFrame();
        GameObject.Destroy(gameObject);
    }

    private IEnumerator StartShowBeam() {
        var hook = GetComponentInParent<OniDeadHook>();
        var beam = GetComponentInChildren<BeamShot>();
        yield return hook.Wait();
        beam.StartShot();
    }
}
