using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField, Header("攻撃力")]
    protected int power;
    [SerializeField]
    protected EnemyAttackableArea attackableArea;
    [SerializeField]
    protected EnemyAI holderEnemy;
    protected Collider attackCollider;

    protected virtual void Start()
    {
        attackCollider = GetComponent<Collider>();
        attackCollider.enabled = false;
    }

    public abstract IEnumerator Attack();

    public bool CanAttack(GameObject target)
    {
        return attackableArea.IsPlayerInArea(target, EnemyAttackableArea.Area.Attackable);
    }

    protected abstract void OnTriggerEnter(Collider collider);
}
