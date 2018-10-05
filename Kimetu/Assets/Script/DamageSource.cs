using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource {
	public Vector3 attackPosition { private set; get; }
	public int damage { private set; get; }

	public DamageSource(Vector3 attackPosition, int damage) {
		this.attackPosition = attackPosition;
		this.damage = damage;
	}

	public static DamageSource Create(Vector3 attackPosition, int damage) {
		//必要ないけど設計書に合わせて
		return new DamageSource(attackPosition, damage);
	}
}
