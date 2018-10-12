using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour, IDamageable
{
    private EnemyAnimation enemyAnimation;
    private Status status;

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

    public void OnHit(DamageSource damageSource)
    {
        status.Damage(damageSource.damage);
        //死亡したら倒れるモーション
        if (status.IsDead())
        {
            //TODO: 倒れるモーションを再生
            //enemyAnimation.Start...();
        }
        //まだ生きていたらダメージモーション
        else
        {
            //enemyAnimation.StartDamageAnimation();
        }
    }
}
