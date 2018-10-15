﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    private int stamina;

    [SerializeField]
    private int maxStamina;

    public override void Start()
    {
        base.Start();
        this.stamina = maxStamina;
    }

    public void OnHit()
    {

    }

    /// <summary>
    /// 現在のスタミナを返します。
    /// </summary>
    /// <returns></returns>
    public int GetStamina()
    {
        return stamina;
    }

    /// <summary>
    /// 回復する
    /// </summary>
    /// <param name="healHP">回復量</param>
    public void Heal(int healHP)
    {
        hp += healHP;
        hp = Mathf.Clamp(hp, 0, maxHP);
    }

    /// <summary>
    /// スタミナを減少する
    /// </summary>
    /// <param name="num">減少量</param>
    public void DecreaseStamina(int num)
    {
        this.stamina -= num;
        this.stamina = Mathf.Clamp(this.stamina, 0, maxStamina);
    }
}
