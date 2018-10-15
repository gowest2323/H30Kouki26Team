using System.Collections;
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

    public void Heal(int healHP)
    {
        hp += healHP;
        hp = Mathf.Clamp(hp, 0, maxHP);
    }
}
