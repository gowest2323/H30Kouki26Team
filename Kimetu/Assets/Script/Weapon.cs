using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 抽象武器クラス
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    [SerializeField, Header("この武器を扱っているCharacterBehaviour")]
    protected CharacterBehaviour holder;

    [SerializeField, Header("攻撃力")]
    protected int power;
    protected Collider weaponCollider; //武器のあたり判定

    protected virtual void Start()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
    }

    /// <summary>
    /// 攻撃開始時に呼ばれる
    /// </summary>
    public abstract void AttackStart();

    /// <summary>
    /// 攻撃終了時に呼ばれる
    /// </summary>
    public abstract void AttackEnd();

    /// <summary>
    /// この武器を扱っている持ち主を返す
    /// </summary>
    public CharacterBehaviour GetHolder() { return holder; }
}
