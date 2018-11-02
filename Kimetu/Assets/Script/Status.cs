using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    protected int hp;

    [SerializeField]
    protected int maxHP;

    public virtual void Start() {
        this.hp = maxHP;
    }

    /// <summary>
    /// 残りHPを返します。
    /// </summary>
    /// <returns></returns>
    public int GetHP()
    {
        return hp;
    }

    /// <summary>
    /// 残りHPの割合を返します。
    /// </summary>
    /// <returns></returns>
    public float GetHPRatio()
    {
        return ((float)hp / (float)maxHP);
    }

    public void Damage(int power)
    {
        hp = hp - power;
    }
    public bool IsDead()
    {
        return hp <= 0;
    }

    /// <summary>
    /// ステータスをリセットします。
    /// プレイヤーが死亡した時に呼ばれます。
    /// </summary>
    public virtual void Reset() {
        this.hp = maxHP;
    }
}
