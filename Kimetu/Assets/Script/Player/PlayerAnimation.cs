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


}
