using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField, Header("攻撃力")]
    protected int power;
    [SerializeField]
    protected EnemySearchableAreaBase attackableArea;
    [SerializeField]
    protected EnemyAI holderEnemy;
    protected Collider attackCollider;
    protected EnemyAnimation enemyAnimation;

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

    protected abstract void OnTriggerEnter(Collider collider);
}
