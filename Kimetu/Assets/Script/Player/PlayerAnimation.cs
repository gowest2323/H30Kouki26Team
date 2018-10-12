using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation :CharacterAnimation {   
     private Animator anim;
   

    public float speed
    {
        get
        {
           return anim.speed;
        }

        set
        {
            anim.speed = value;
        }
    }

    public PlayerAnimation(Animator anim)
    {
        this.anim = anim;
        this.speed = 1.0f;
    }
	// Use this for initialization

   public void StartRunAnimation()
    {
        //anim.SetFloat("Speed", speed);
        anim.SetBool("Run",true);
    }

    public void StopRunAnimation()
    {
        anim.SetFloat("Speed", speed);
        anim.SetBool("Run", false);
    }

    public void StartAttackAnimation()
    {

        anim.SetTrigger("Attack");
    }

    public void StartGuardAnimation()
    {
        anim.SetBool("Guard", true);
    }

    public void StopGuardAnimation()
    {
        anim.SetBool("Guard", false);
    }

    public void StartAvoidAnimation()
    {
        anim.SetTrigger("Avoid");
    }




}
