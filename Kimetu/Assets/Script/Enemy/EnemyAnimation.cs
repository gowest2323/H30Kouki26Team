using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation :CharacterAnimation {

    private Animator enemyAnim;
    public float speed
    {
        get
        {
            return enemyAnim.speed;
        }

        set
        {
            enemyAnim.speed = value;
        }
    }

    public EnemyAnimation(Animator enemyAnim)
    {
        this.enemyAnim = enemyAnim;
        this.speed = 1.0f;
    }
    public void StartRunAnimation()
    {
        enemyAnim.SetBool("Run", true);
    }

    public void StopRunAnimation()
    {
        enemyAnim.SetBool("Run", false);
    }

    public void StartAttackAnimation()
    {
        enemyAnim.SetTrigger("Attack");
    }




}
