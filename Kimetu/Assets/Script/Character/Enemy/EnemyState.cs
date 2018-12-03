using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE:Enemyの種類ごとに XXXState を用意する必要があるかもしれません
public enum EnemyState {
	Idle, //待機　
	Move, //移動中
	Attack, //攻撃時
	Search, //索敵時
	Damage, //被弾時
	MoveNear, //接近中
    MoveDefaultPosition, //デフォルトの場所に移動中
	Death, //倒れている
	None,
}

