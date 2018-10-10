using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status {
    private int stamina;

    [SerializeField]
    private int maxStamina;

    public override void Start() {
        base.Start();
        this.stamina = maxStamina;
    }

    public void OnHit()
    {

    }
}
