﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ダメージの種類。
/// </summary>
public enum DamageMode {
	Kill,
	NotKill
}


public class Status : MonoBehaviour {
	/// <summary>
	/// ダメージを受けたら呼ばれます。
	/// </summary>
	/// <value></value>
	public IObservable<int> onDamage {
		get { return mOnDamage; }
	}
	private Subject<int> mOnDamage;

	/// <summary>
	/// 死亡を受けたら呼ばれます。
	/// </summary>
	/// <value></value>
	public IObservable<int> onDie {
		get { return mOnDie; }
	}
	private Subject<int> mOnDie;

	protected int hp;

	[SerializeField]
	protected int maxHP;
	public virtual void Awake() {
		//ここで初期化しないと動かない場合がある
		this.hp = maxHP;
	}

	public virtual void Start() {
		this.hp = maxHP;
	}

	/// <summary>
	/// 残りHPを返します。
	/// </summary>
	/// <returns></returns>
	public int GetHP() {
		return hp;
	}

	/// <summary>
	/// 残りHPの割合を返します。
	/// </summary>
	/// <returns></returns>
	public float GetHPRatio() {
		return ((float)hp / (float)maxHP);
	}

	public void Damage(int power, DamageMode mode = DamageMode.NotKill) {
		if (this.hp <= 0) {
			return;
		}

		hp = hp - power;
		mOnDamage.OnNext(power);

		if (this.hp <= 0) {
			this.hp = 0;
		}

		if (mode == DamageMode.NotKill && this.hp <= 0) {
			this.hp = 1;
		}

		if (this.hp <= 0) {
			mOnDie.OnNext(power);
		}
	}
	public bool IsDead() {
		return hp <= 0;
	}

	public bool IsAlive() {
		return !IsDead();
	}

	/// <summary>
	/// ステータスをリセットします。
	/// プレイヤーが死亡した時に呼ばれます。
	/// </summary>
	public virtual void Reset() {
		this.hp = maxHP;
	}
}
