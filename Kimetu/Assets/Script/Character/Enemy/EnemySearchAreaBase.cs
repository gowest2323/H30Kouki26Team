using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySearchableAreaBase : MonoBehaviour {
	/// <summary>
	/// プレイヤーが範囲内にいるか？
	/// </summary>
	/// <param name="player">プレイヤーオブジェクト</param>
	/// <param name="toRayCast">レイを飛ばすか</param>
	/// <returns></returns>
	public abstract bool IsPlayerInArea(GameObject player, bool toRayCast);

	/// <summary>
	/// レイがプレイヤーと当たったか
	/// </summary>
	/// <param name="fromPosition">始点</param>
	/// <param name="toTargetDir">方向</param>
	/// <param name="target">ターゲットとなるオブジェクト</param>
	/// <param name="distance">レイの長さ</param>
	/// <returns>当たったのがプレイヤーならtrue</returns>
	protected virtual bool IsHitRayToPlayer(Vector3 fromPosition, Vector3 toTargetDir, GameObject target, float distance) {
		RaycastHit hit;
		int layerMask = LayerMask.GetMask(LayerName.PlayerDamageable.String());
		Ray ray = new Ray(fromPosition, toTargetDir);

		//何も当たらなければ終了
		if (!Physics.Raycast(ray, out hit, distance, layerMask)) {
			return false;
		}

		return hit.transform.tag == TagName.Player.String();
	}
}
