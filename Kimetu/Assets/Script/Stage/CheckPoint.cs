using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CheckPoint : MonoBehaviour {
	[SerializeField]
	private StageManager stageManager;

	private void Start() {
		//StageManagerがインスペクターから割り当てられていなければ親から取得
		if (stageManager == null) {
			Debug.Log(gameObject.name + "に" + typeof(StageManager).Name + "が割り当てられていません。\n" + "親オブジェクトから取得します。");
			stageManager = transform.parent.GetComponent<StageManager>();
		}

		//見つからなければ警告
		Assert.IsNotNull(stageManager, typeof(StageManager).Name + "が見つかりません。");

		//リリース時にはチェックポイントを表示しない
		if (!Debug.isDebugBuild) {
			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			meshRenderer.enabled = false;
		}
	}

	public void OnTriggerEnter(Collider collider) {
		//プレイヤーと当たったらStageManagerに通知
		if (collider.tag == TagName.Player.String()) {
			Vector3 temp = transform.rotation.eulerAngles;
			stageManager.Pass(transform.position, Quaternion.Euler(temp));
		}
	}
}