using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Assertions;

/// <summary>
/// 現在のシーンの "Enemy" の増減に応じて、
/// それを追跡するスライダーを生成するスクリプトです。
/// このスライダーはエネミーのHPと同期します。
/// </summary>
public class DebugEnemyUI : MonoBehaviour {
	[SerializeField]
	private RectTransform canvasRect;

	[SerializeField]
	private GameObject uiPrefab;

	private List<EnemyUI> uiList;

	public struct EnemyUI {
		public GameObject enemyObject;
		public GameObject uiObject;
		private Slider slider;

		public EnemyUI(GameObject enemyObject, GameObject uiObject) {
			this.enemyObject = enemyObject;
			this.uiObject = uiObject;
			this.slider = uiObject.GetComponent<Slider>();
		}

		public void Synchro() {
			if(enemyObject == null) {
				return;
			}
			var status = enemyObject.GetComponent<EnemyStatus>();
			slider.value = status.GetHPRatio();
		}
	}

	// Use this for initialization
	void Start () {
		if(canvasRect == null) {
			this.canvasRect = GetComponent<RectTransform>();
		}
		this.uiList = new List<EnemyUI>();
		Assert.IsTrue(canvasRect != null);
		StartCoroutine(UIUpdate());
	}
	
	// Update is called once per frame
	void Update () {
		uiList.ForEach((e) => e.Synchro());
        gameObject.transform.rotation = Camera.main.transform.rotation;
    }

	private IEnumerator UIUpdate() {
		var wait = new WaitForSeconds(1);
		while(true) {
			yield return wait;
			//消えたエネミーの確認
			uiList.Where((e) => e.enemyObject == null)
				  .ToList()
				  .ForEach((e) => GameObject.Destroy(e.uiObject));
			uiList.RemoveAll((e) => e.enemyObject == null);
			//現れたオブジェクトの確認
			var enemies = GameObject.FindGameObjectsWithTag(TagName.Enemy.String());
			foreach(var enemy in enemies) {
				//既に追加されている
				var include = uiList.Any((e) => e.enemyObject == enemy);
				if(include) continue;
				//新たに現れた
				var uiObj = GameObject.Instantiate(uiPrefab, Vector3.zero, Quaternion.identity, transform);
				var trackUI = uiObj.AddComponent<TrackUI>();
				var entry = new EnemyUI(enemy, uiObj);
				trackUI.SetCanvasRect(canvasRect);
				trackUI.SetTarget(enemy);
				uiList.Add(entry);
			}
        }
    }
}
