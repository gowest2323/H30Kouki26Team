using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int hp { protected set; get; }
    public int maxHP { protected set; get; }


    public void Damage(int power)
    {
        hp = hp - power;
    }
    public bool IsDead()
    {
        return hp <= 0;
    }
}
