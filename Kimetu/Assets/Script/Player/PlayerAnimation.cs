﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation :CharacterAnimation {      

	// Use this for initialization

   public void StartRunAnimation()
    {
        //anim.SetFloat("Speed", speed);
        animator.SetBool("Run",true);
    }

    public void StopRunAnimation()
    {
        animator.SetFloat("Speed", speed);
        animator.SetBool("Run", false);
    }

    public void StartWalkAnimation() {
        animator.SetBool("Walk", true);
    }

    public void StopWalkAnimation() {
        animator.SetBool("Walk", false);
    }

    public void StartCounterAnimation() {
        animator.SetTrigger("Counter");
    }

    public void StartAttackAnimation(int phase)
    {
        switch(phase) {
            case 0:
                animator.SetTrigger("Attack");
                break;
            case 1:
                animator.SetTrigger("Attack2");
                break;
            case 2:
                animator.SetTrigger("Attack3");
                break;
            case 3:
                animator.SetTrigger("Attack4");
                break;
            default:
                throw new System.ArgumentException();
        }
    }
    public void StartGuardAnimation()
    {
        animator.SetBool("Guard", true);
    }

    public void StopGuardAnimation()
    {
        animator.SetBool("Guard", false);
    }
    public void StartGuardWalkAnimation()
    {
        animator.SetBool("GuardWalk", true);
    }
    public void StopGuardWalkAnimation()
    {
        animator.SetBool("GuardWalk", false);
    }

    public void SetGuardSpeed(int speed)
    {
        animator.SetFloat("GuardSpeed", speed);
    }

    public void StartForwardAvoidAnimation()
    {
        animator.SetTrigger("ForwardAvoid");
    }

    public void StartRightAvoidAnimation()
    {
        animator.SetTrigger("RightAvoid");
    }

    public void StartLeftAvoidAnimation()
    {
        animator.SetTrigger("LeftAvoid");
    }

    public void StartBackAvoidAnimation()
    {
        animator.SetTrigger("BackAvoid");
    }

    public void StartKnockBackAnimation()
    {
        animator.SetTrigger("KnockBack");
    }

    public void StartKyuuseiAnimation() {
        animator.SetBool("Kyuusei", true);
    }

    public void StopKyuuseiAnimation() {
        animator.SetBool("Kyuusei", false);
    }
}
