using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {
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
}
