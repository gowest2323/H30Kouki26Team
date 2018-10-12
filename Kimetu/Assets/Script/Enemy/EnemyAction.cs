using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour {
	private EnemyAnimation enemyAnimation;

	// Use this for initialization
	void Start () {
		this.enemyAnimation = new EnemyAnimation(GetComponent<Animator>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnHit(Weapon weapon) {
		//TODO:ここでダメージアニメーションを開始する
		//TODO:HPを減らす
	}
}
