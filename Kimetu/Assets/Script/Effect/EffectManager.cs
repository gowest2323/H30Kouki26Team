﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager> {

	public GameObject playerDamageEffect;
	public GameObject enemyDamageEffect;
	public GameObject playerMoveEffect;
	public GameObject enemyMoveEffect;
	public GameObject playerViolentMoveEffect;
	public GameObject shieldEffect;
	public GameObject auraEffect;
    public GameObject auraEffect_Slow;
	GameObject effect;
	[SerializeField]
	private float limitTime = 1.0f;
	private float startTime;


	void Awake() {
		startTime = 0.1f;
	}

	public void PlayerDamageEffectCreate(GameObject target) {
		//今までのダメージエフェクトの代わりにプレイヤーのモデルを赤くする処理を入れる
	}

	public void EnemyDamageEffectCreate(GameObject target, bool isShield) {
		if (isShield) {
			effect = shieldEffect;
		} else {
			effect = enemyDamageEffect;
		}

		if (Slow.Instance.isSlowNow) {
			enemyDamageEffect.GetComponent<ParticleSystem>().startLifetime = limitTime;
		} else {
			enemyDamageEffect.GetComponent<ParticleSystem>().startLifetime = startTime;
		}


		var obj = GameObject.Instantiate(effect) as GameObject;
		obj.transform.position = target.transform.position + new Vector3(0, 2, 0);
		Destroy(obj, 1f);
	}

	public void PlayerMoveEffectCreate(GameObject player, bool isViolent) {

		if (isViolent) {
			effect = playerViolentMoveEffect;
		} else {
			effect = playerMoveEffect;
		}

		var obj = GameObject.Instantiate(effect, Vector3.zero, Quaternion.identity, this.transform.parent) as GameObject;
		obj.transform.position = player.transform.position;
		Destroy(obj, 0.5f);

	}

	public void EnemyMoveEffectCreate(GameObject enemy) {

		var obj = GameObject.Instantiate(enemyMoveEffect, Vector3.zero, Quaternion.identity, this.transform.parent) as GameObject;
		obj.transform.position = enemy.transform.position;
		Destroy(obj, 0.5f);

	}
	public void EnemyAuraCreate(GameObject enemy) {

        if (Slow.Instance.isSlowNow) return;      
		var obj = GameObject.Instantiate(auraEffect) as GameObject;
		obj.transform.position = enemy.transform.position ;
		Destroy(obj, 0.1f);

	}

    public void EnemySlowAuraCreate(GameObject enemy)
    {

        var obj = GameObject.Instantiate(auraEffect_Slow, enemy.transform) as GameObject;
        obj.transform.position = enemy.transform.position;
        Destroy(obj, 2f);

    }




}
