using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    /// <summary>
    /// 攻撃される
    /// </summary>
    /// <param name="damageSource">ダメージ情報</param>
	void OnHit(DamageSource damageSource);

    /// <summary>
    /// カウンターされる
    /// </summary>
    void Countered();
}
