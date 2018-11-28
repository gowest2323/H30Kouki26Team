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
/// 攻撃のアニメーションの特定のタイミングでメソッドを呼び出す場合、
/// Animator と同じ階層にあるスクリプトのメソッドしか呼び出せないようです。
/// 当たり判定が意図せず多段ヒットすることがあるため、
/// それを防止するためにアニメーションの開始と終了で EnemyAttack の#AttackStart, #AttackEndを呼び出したいのですが、
/// それを実現するためには全ての EnemyAttack を 同じ階層に設定する必要があります。
///
/// また、Animator からメソッドを呼び出す時、名前によってのみメソッドを指定できます。
/// そのため、#AttackStart, #AttackEnd ではなく、
/// #(攻撃手段)AttackStart, #(攻撃手段)AttackEnd のような形式のメソッドが必要です。
/// (重複しない名前が必要になる)
///
/// それを EnemyAttack側で攻撃に対応した #AttackStart, #AttackEnd をつけたくなかったので、
/// EnemyAttack側ではメソッド名を統一し、
/// このラッパー側で攻撃の種類に対応したメソッド名をつけることで対応しています。
///
/// 現在Animatorはエネミープレハブのルートに追加されているので、
/// このスクリプトもまたエネミープレハブのルートに設定する必要があります。
/// </summary>
public class EnemyWeaponProxy : MonoBehaviour {
	[SerializeField]
	private EnemyAttack swingAttackHandler;
	[SerializeField]
	private EnemyAttack horizontalAttackHandler;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
	//SwingAttack(振り下ろし)

	public void StartSwingAttack() {
		swingAttackHandler.AttackStart();
	}

	public void EndSwingAttack() {
		swingAttackHandler.AttackEnd();
	}
	//HorizontalAttack(なぎ払い)

	public void StartHorizontalAttack() {
		horizontalAttackHandler.AttackStart();
	}

	public void EndHorizontalAttack() {
		horizontalAttackHandler.AttackEnd();
	}
}
