using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの攻撃エフェクトを作成するクラス。
/// </summary>
public class EnemyEffectGenerator : MonoBehaviour {
    [SerializeField]
    private GameObject enemyEffectPrefab;

    private List<GameObject> enemyEffectList;

	// Use this for initialization
	void Start () {
        this.enemyEffectList = new List<GameObject>();
        CreateEffect();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateEffect()
    {
        enemyEffectList.ForEach((e) => GameObject.Destroy(e));
        enemyEffectList.Clear();
        var objs = GameObject.FindGameObjectsWithTag(TagName.Enemy.String());
        foreach(var obj in objs)
        {
            var effectArea = obj.transform.FindRec("EnemyClubEffectArea");
            var start = effectArea.transform.Find("Start");
            var end = effectArea.transform.Find("End");
            //エネミーに対応したエフェクトを作成
            var effect = GameObject.Instantiate(enemyEffectPrefab);
            effect.transform.parent = transform;
            var script = effect.GetComponent<CreateSwordTrail>();
            script.startPosition = start;
            script.endPosition = end;
            //保存
            enemyEffectList.Add(effect);
        }
    }
}
