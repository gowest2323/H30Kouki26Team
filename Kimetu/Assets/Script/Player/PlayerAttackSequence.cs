using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 攻撃の一段階が終了すると呼ばれる。
/// </summary>
public delegate void AttackPhaseFinished();

/// <summary>
/// 攻撃の結果を返します。
/// </summary>
public enum AttackResult {
	OK,
	Err
}

/// <summary>
/// プレイヤーの攻撃の流れ。
/// </summary>
public class PlayerAttackSequence : MonoBehaviour {
	public event AttackPhaseFinished OnAttackPhaseFinished = delegate { };
	[SerializeField]
	private PlayerAnimation playerAnimation;
	[SerializeField]
	private Weapon weapon;
	private Coroutine coroutine;
	private int attackStack;
	public bool isAttack { get { return attackStack > 0; }}

	// Use this for initialization
	void Start () {
		if(this.playerAnimation == null) {
			this.playerAnimation = GetComponent<PlayerAnimation>();
		}
		if(this.weapon == null) {
			this.weapon = GetComponentInChildren<Weapon>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// 現在の攻撃のフェーズから対応する次の攻撃を選択して実行します。
	/// </summary>
	/// <returns></returns>
	public AttackResult Attack() {
		if(attackStack == 0) {
			this.coroutine = StartCoroutine(StartAttack());
		} else if (attackStack == 4) {
			//最後まで攻撃が入力されているなら無視
			return AttackResult.Err;
		} else {
			StopAttack();
			this.coroutine = StartCoroutine(StartAttack());
		}
		return AttackResult.OK;
	}

	private void StopAttack() {
		if(this.coroutine != null) {
			StopCoroutine(coroutine);
			this.coroutine = null;
		}
	}

	private IEnumerator StartAttack()　{
//        if (isAttack) yield break;

//        isAttack = true;
		attackStack++;
        Slow.Instance.PlayerAttacked(playerAnimation);

        weapon.AttackStart();
        //playerAnimation.StartAttackAnimation();
		StartAnimation();
        //status.DecreaseStamina(decreaseAttackStamina);
        AudioManager.Instance.PlayPlayerSE(AudioName.SE_CUT.String());
        yield return new WaitForSeconds(1);
        weapon.AttackEnd();
        //isAttack = false;
		attackStack = 0;
		OnAttackPhaseFinished();
        //state = PlayerState.Idle;
    }

	private void StartAnimation() {
		playerAnimation.StartAttackAnimation(attackStack - 1);
	}
}
