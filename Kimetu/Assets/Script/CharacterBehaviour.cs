using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CharacterBehaviour : MonoBehaviour
{
    protected float speed; //移動速度

    /// <summary>
    /// 移動処理
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// 攻撃処理
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// カウンターされたときの挙動
    /// </summary>
    public abstract void Countered();
}
