using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE:Enemyの種類ごとに XXXState を用意する必要があるかもしれません
public enum EnemyState {
	Idle, //待機　
	Move, //移動注
	Attack, //攻撃時
}

