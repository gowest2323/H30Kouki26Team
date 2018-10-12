using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyStatus))]

public class EnemyAction : MonoBehaviour, IDamageable
{
    private EnemyAnimation enemyAnimation; //アニメーション管理
    private Status status; //ステータス管理
    [SerializeField]
    private Weapon weapon;

    // Use this for initialization
    void Start()
    {
        this.enemyAnimation = new EnemyAnimation(GetComponent<Animator>());
        status = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Attack()
    {
        StartCoroutine(AttackStart());
    }

    /// <summary>
    /// 攻撃されたときに呼ばれる
    /// </summary>
    /// <param name="damageSource">ダメージ情報</param>
    public void OnHit(DamageSource damageSource)
    {
        status.Damage(damageSource.damage);

        //死亡したら倒れるモーション
        if (status.IsDead())
        {
            Destroy(this.gameObject);
            //enemyAnimation.Start...();
        }
        //まだ生きていたらダメージモーション
        else
        {
            //enemyAnimation.StartDamageAnimation();
        }
    }

    private IEnumerator AttackStart()
    {
        weapon.AttackStart();
        yield return new WaitForSeconds(1.0f);
        weapon.AttackEnd();
    }

    public void Countered()
    {
        Destroy(this.gameObject);
    }
}
