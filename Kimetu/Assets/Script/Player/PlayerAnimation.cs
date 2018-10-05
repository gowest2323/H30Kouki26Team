using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation  {   
     private Animator anim;
    
    public PlayerAnimation(Animator anim)
    {
        this.anim = anim;
    }
	// Use this for initialization

   public void StartRunAnimation()
    {
        anim.SetBool("Run",true);
    }

    public void StopRunAnimation()
    {
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
