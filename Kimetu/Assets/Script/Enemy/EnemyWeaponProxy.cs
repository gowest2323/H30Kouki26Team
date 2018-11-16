using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の開始と終了で呼ばれるインターフェイス。
/// </summary>
public interface IAttackEventHandler {
	void AttackStart();
	void AttackEnd();
}

/// <summary>
/// アニメーションから関数を呼び出すためのラッパー。
/// おそらくこ階層の関数を呼び出せないのでこれを挟む
/// </summary>
public class EnemyWeaponProxy : MonoBehaviour {
	private IAttackEventHandler handler;

	// Use this for initialization
	void Start () {
		if(handler == null) {
			handler = GetComponentInChildren<IAttackEventHandler>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// アニメーションウィンドウからこの関数を呼び出すように設定してください。
	/// このスクリプトはエネミーのルートに設定します。
	/// </summary>
	public void AttackStartLazy() {
		handler.AttackStart();
	}

	/// <summary>
	/// アニメーションウィンドウからこの関数を呼び出すように設定してください。
	/// このスクリプトはエネミーのルートに設定します。
	/// </summary>
	public void AttackEndLazy() {
		handler.AttackEnd();
	}
}
