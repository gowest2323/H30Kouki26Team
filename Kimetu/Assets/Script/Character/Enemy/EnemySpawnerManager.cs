using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField]
    private List<EnemySpawner> spawners; //敵生成器リスト

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        foreach (var spawner in spawners)
        {
            spawner.Init();
        }
    }
}
