using System;
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

    public void StartAttackAnimation()
    {

        animator.SetTrigger("Attack");
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
