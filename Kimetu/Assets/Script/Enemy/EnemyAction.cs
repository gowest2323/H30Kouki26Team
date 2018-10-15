﻿using System;
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
    /// <summary>
    /// 回生に使われられるか
    /// </summary>
    public bool CanUseHeal { private set; get; }

    // Use this for initialization
    void Start()
    {
        this.enemyAnimation = new EnemyAnimation(GetComponent<Animator>());
        status = GetComponent<Status>();
        CanUseHeal = false;
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
    /// 攻撃された
    /// </summary>
    /// <param name="damageSource">ダメージ情報</param>
    public void OnHit(DamageSource damageSource)
    {
        status.Damage(damageSource.damage);

        //死亡したら倒れるモーション
        if (status.IsDead())
        {
            StartCoroutine(PassOut());
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

    /// <summary>
    /// カウンターされる
    /// </summary>
    public void Countered()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 倒れる
    /// </summary>
    /// <returns></returns>
    private IEnumerator PassOut()
    {
        //倒れるアニメーションが終了するまで待機
        this.transform.Rotate(new Vector3(0, 0, 90));
        yield return new WaitForSeconds(3.0f);
        CanUseHeal = true;
    }

    /// <summary>
    /// 回生に使われた
    /// </summary>
    public void UsedHeal()
    {
        if (!CanUseHeal) Debug.LogError("2回以上UsedHealが使われました。");
        CanUseHeal = false;
        Destroy(this.gameObject, 0.5f);
    }
}
