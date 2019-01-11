using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {
	public static T GetComponentOrNull<T>(this GameObject self) where T : Component {
		if(self == null) {
			return null;
		}
		return self.GetComponent<T>();
	}

	/// <summary>
	/// nameと同じ名前のオブジェクトを再帰的に検索します。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	public static GameObject FindRec(this Transform self, string name) {
		var e = self.Find(name);
        if (e != null) {
            return e.gameObject;
        }
        for (int i = 0; i < self.childCount; i++) {
            var subtree = FindRec(self.GetChild(i), name);
            if (subtree != null) {
                return subtree;
            }
        }
        return null;
	}

    /// <summary>
    /// Y方向を無視して距離を計算します。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float DistanceXZ(GameObject a, GameObject b) {
        return DistanceXZ(a.transform.position, b.transform.position);
    }

    /// <summary>
    /// Y方向を無視して距離を計算します。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float DistanceXZ(Vector3 a, Vector3 b) {
        var ignoreY = new Vector3(1, 0, 1);
        return Vector3.Distance(
            Vector3.Scale(a, ignoreY),
            Vector3.Scale(b, ignoreY)
        );
    }

    /// <summary>
	/// 範囲内の最も近くにいる敵を返す
	/// </summary>
	/// /// <param name="basePoint">自分の位置</param>
	/// <param name="maxDistance">敵を探す範囲</param>
	/// <param name="throughWall">壁を貫通するか</param>
	/// <returns>敵が存在しなければnullを返す</returns>
    public static GameObject SearchMostNearEnemyInTheRange(Vector3 basePoint, float maxDistance, bool throughWall) {
        //敵リストを取得
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(TagName.Enemy.String());

		//敵がいなければnullを返す
		if (enemies.Length == 0) return null;

		GameObject result = null;
		float closestDistance = 0.0f; //一番近い敵との距離

		foreach (var enemy in enemies) {
			//敵が死亡していたら次のループへ
			EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();

			if (enemyStatus == null || enemyStatus.IsDead()) continue;

			//自分と敵のXZ座標で距離を判定
			float distance = DistanceXZ(basePoint, enemy.transform.position);

			//範囲外なら次のループへ
			if (distance > maxDistance) continue;

			//現在の近い敵との距離より近ければ更新
			if (closestDistance < distance) {
				closestDistance = distance;
				result = enemy;
			}
		}

		return result;
    }

	/// <summary>
	/// ヒエラルキーの全てのオブジェクトからTを取得して返します。
	/// </summary>
	/// <returns>The components from all object.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static List<T> GetComponentsFromAllObject<T>() where T : Component {
		var ret = new List<T>();
		foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
			if (!obj.activeInHierarchy) continue;
			var comp = obj.GetComponent<T>();
			if (comp == null) continue;
			ret.Add(comp);
		}
		return ret;
	}

	/// <summary>
	/// 指定の位置、距離、方向、レイヤーで何かつぶつかるなら true.
	/// </summary>
	/// <returns><c>true</c>, if hit to any was ised, <c>false</c> otherwise.</returns>
	/// <param name="origin">Origin.</param>
	/// <param name="dir">Dir.</param>
	/// <param name="distance">Distance.</param>
	/// <param name="mask">Mask.</param>
	public static bool IsHitToAny(Vector3 origin, Vector3 dir, float distance = 1f, int mask = 0) {
		RaycastHit hit;
		if (Physics.Raycast(origin, dir, out hit, distance, mask)) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// 指定の位置、方向、距離でステージとぶつかるなら true.
	/// </summary>
	/// <returns><c>true</c>, if hit to stage was ised, <c>false</c> otherwise.</returns>
	/// <param name="origin">Origin.</param>
	/// <param name="dir">Dir.</param>
	/// <param name="distance">Distance.</param>
	public static bool IsHitToStage(Vector3 origin, Vector3 dir, float distance = 1f) {
		return IsHitToAny(origin, dir, distance, LayerMask.GetMask(LayerName.Stage.String()));
	}

	/// <summary>
	/// 指定の名前で順番に検索して見つかったならそれを返します。
	/// </summary>
	/// <returns>The any.</returns>
	/// <param name="names">Names.</param>
	public static GameObject FindAny(params string[] names) {
		foreach(var name in names) {
			var obj = GameObject.Find(name);
			if (obj != null) return obj;
		}
		return null;
	}
}
