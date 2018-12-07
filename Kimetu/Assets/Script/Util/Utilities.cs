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
}
