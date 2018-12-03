using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<EnemySpawnData> spawnDatas; //敵生成情報リスト
    private bool spawned; //生成したかどうか

    // Use this for initialization
    void Start()
    {
        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        spawned = false;
    }

    /// <summary>
    /// 生成する
    /// </summary>
    public void Spawn()
    {
        //生成済みなら早期リターン
        if (spawned) return;
        //スポナーに登録されているすべての敵を生成
        foreach (var spawnData in spawnDatas)
        {
            Instantiate(spawnData.enemy, spawnData.spawnTransform);
        }
        spawned = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        //プレイヤーが生成範囲に入ったら生成する
        if (TagNameManager.Equals(other.tag, TagName.Player))
        {
            Spawn();
        }
    }
}
