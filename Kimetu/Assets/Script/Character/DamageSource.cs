using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource {
	public Vector3 attackPosition { private set; get; }
	public int damage { private set; get; }
    public bool canCounter { private set; get; }
	public IDamageable attackCharacter { private set; get; } //攻撃をしたキャラクター

	public DamageSource(Vector3 attackPosition, int damage, IDamageable attackCharacter,bool canCounter =true) {
		this.attackPosition = attackPosition;
		this.damage = damage;
		this.attackCharacter = attackCharacter;
        this.canCounter = canCounter;
	}
}
