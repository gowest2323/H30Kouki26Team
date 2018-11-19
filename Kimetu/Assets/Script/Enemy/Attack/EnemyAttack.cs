using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour, IAttackEventHandler
{
    [SerializeField, Header("攻撃力")]
    protected int power;
    [SerializeField]
    protected EnemySearchableAreaBase attackableArea;
    [SerializeField]
    protected EnemyAI holderEnemy;
    protected Collider attackCollider;
    protected EnemyAnimation enemyAnimation;
    /// <summary>
    /// 攻撃が何回ヒットしたかを記録する。
    /// 一度のアニメーションで意図せず多段ヒットすることがあるのでその防止のために用意しました。
    /// #AttackStart で 0 にリセットされます。
    /// </summary>
    protected int hitCount;

    /// <summary>
    /// この攻撃が開始しているなら true.
    /// このフラグは IsTriggerEnter のなかで使用します。
    /// 攻撃手段が複数あり、かつそれぞれが当たり判定を共有している場合があります。
    /// フラグを使用せずに IsTriggerEnter で必ずダメージを与えるように実装すると、
    ///
    /// 例えば複数の攻撃手段を持っているボスが振り下ろしという攻撃のためにコライダーを有効化した時、
    /// もう一つの攻撃手段であるなぎ払い(HorizontalAttack)も同様に IsTriggerEnter で攻撃を与えてしまいます。
    /// このフラグは #AttackStart で trueに、 #AttackEnd でfalseになります。
    /// </summary>
    /// <value></value>
    protected bool running { private set; get; }

    protected virtual void Start()
    {
        attackCollider = GetComponent<Collider>();
        attackCollider.enabled = false;
        enemyAnimation = GetComponentInParent<EnemyAnimation>();
    }

    public abstract IEnumerator Attack();

    public bool CanAttack(GameObject target)
    {
        return attackableArea.IsPlayerInArea(target, false);
    }

    public void OnTriggerEnter(Collider collider) {
        if(running) {
            OnHit(collider);
        }
    }

    /// <summary>
    /// 他のオブジェクトと衝突すると呼ばれます。
    /// </summary>
    /// <param name="collider"></param>
    protected abstract void OnHit(Collider collider);

    public void AttackStart() {
        this.hitCount = 0;
        this.running = true;
        GetComponent<Collider>().enabled = true;
    }

	public void AttackEnd() {
        this.running = false;
        GetComponent<Collider>().enabled = false;
    }
}
