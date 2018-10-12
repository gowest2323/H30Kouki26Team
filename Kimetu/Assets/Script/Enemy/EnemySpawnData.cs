using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnData
{
    public Transform spawnTransform; //敵生成地点情報
    public GameObject enemy; //敵の種類

}
