﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status {
	private float stamina;
	private float maxStamina;

	public override void Start() {
		base.Start();
		PlayerScriptableObject playerParameter = parameter as PlayerScriptableObject;
		UnityEngine.Assertions.Assert.IsNotNull(playerParameter, "Player用のパラメータが設定されていません。") ;
		this.stamina = maxStamina = playerParameter.maxStamina;
	}

	public void OnHit() {

	}

	/// <summary>
	/// 現在のスタミナを返します。
	/// </summary>
	/// <returns></returns>
	public float GetStamina() {
		return stamina;
	}

	/// <summary>
	/// スタミナの最大値を返します。
	/// </summary>
	/// <returns></returns>
	public float GetMaxStamina() {
		return maxStamina;
	}

	/// <summary>
	/// スタミナの割合を返します。
	/// </summary>
	/// <returns></returns>
	public float GetStaminaRatio() {
		return (float)GetStamina() / (float)GetMaxStamina();
	}

	public float RecoveryStamina() {
		if (stamina < maxStamina)
			return stamina += 1;
		else
			return stamina;
	}

	/// <summary>
	/// 回復する
	/// </summary>
	/// <param name="healHP">回復量</param>
	public void Heal(int healHP) {
		hp += healHP;
		hp = Mathf.Clamp(hp, 0, maxHP);
	}

	/// <summary>
	/// スタミナを減少する
	/// </summary>
	/// <param name="num">減少量</param>
	public void DecreaseStamina(float num) {
		this.stamina -= num;
		this.stamina = Mathf.Clamp(this.stamina, 0, maxStamina);
	}

	public override void Reset() {
		base.Reset();
		this.stamina = maxStamina;
	}
	#if UNITY_EDITOR
	public void __SetStamina(float value) {
		this.stamina = value;
	}

	public float __GetMaxStamina() {
		return maxStamina;
	}
	#endif
}
