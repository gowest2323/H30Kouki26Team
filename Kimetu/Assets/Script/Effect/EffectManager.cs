using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager> {

	public GameObject playerDamageEffect;
	public GameObject enemyDamageEffect;
	public GameObject playerMoveEffect;
    public GameObject enemyMoveEffect;
    public GameObject playerViolentMoveEffect;
    GameObject effect;
    [SerializeField]
	private float limitTime = 1.0f;
	private float startTime;


	void Awake() {
		startTime = 0.1f;
	}

	public void PlayerDamageEffectCreate(GameObject target) {
		var obj = GameObject.Instantiate(playerDamageEffect) as GameObject;
		obj.transform.position = target.transform.position + new Vector3(0, 1, 0);
		Destroy(obj, 1f);
	}

	public void EnemyDamageEffectCreate(GameObject target) {
		if (Slow.Instance.isSlowNow) {
			enemyDamageEffect.GetComponent<ParticleSystem>().startLifetime = limitTime;
		} else {
			enemyDamageEffect.GetComponent<ParticleSystem>().startLifetime = startTime;
		}

		var obj = GameObject.Instantiate(enemyDamageEffect) as GameObject;
		obj.transform.position = target.transform.position + new Vector3(0, 2, 0);
		Destroy(obj, 1f);
	}

	public void PlayerMoveEffectCreate(GameObject player,bool isViolent) {
        
        if (isViolent)
        {
            effect = playerViolentMoveEffect;
        }
        else
        {
            effect = playerMoveEffect;
        }
        var obj = GameObject.Instantiate(effect, Vector3.zero, Quaternion.identity, this.transform.parent) as GameObject;
        obj.transform.position = player.transform.position;
		Destroy(obj, 0.5f);

	}

    public void EnemyMoveEffectCreate(GameObject enemy)
    {
        var obj = GameObject.Instantiate(enemyMoveEffect, Vector3.zero, Quaternion.identity, this.transform.parent) as GameObject;
        obj.transform.position = enemy.transform.position;
        Destroy(obj, 0.5f);

    }


}
