using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossAI : EnemyAI, IDamageable {
	private bool isAction; //行動中か？
	[SerializeField]
	private IdleAction idle;
	[SerializeField, Tooltip("回転斬り")]
	private AttackAction rotateSlash;
	[SerializeField, Tooltip("二度斬り")]
	private AttackAction twiceShash;
	[SerializeField, Tooltip("振り回し歩き")]
	private AttackAction hurimawashiAruki;
	[SerializeField]
	private NearPlayerAction nearPlayer;
	[SerializeField]
	private DamageAction damage;
	[SerializeField]
	private DeathAction death;
	private EnemyStatus status;
	private EnemyAnimation enemyAnimation;

	protected override void Start() {
		base.Start();
		status = GetComponent<EnemyStatus>();
		currentActionCoroutine = Think();
		canUseHeal = false;
	}

	public override void Countered() {
		//行動を停止し、ダメージアクションに移行
		StopCoroutine(currentActionCoroutine);
		currentActionCoroutine = StartCoroutine(damage.Action(ActionCallBack, DamagePattern.Countered));
		currentState = EnemyState.Damage;
		return;
	}

	public override void OnHit(DamageSource damageSource) {
		//すでに死亡しているなら何もしない
		//これがないと、死亡したエネミーに攻撃が当たったとき、
		//エネミーのローテーションがおかしくなる(PassOutが実行されるため??)
		if (status.IsDead()) { return; }

		ApplyDamage(damageSource);
		//現在の行動を停止
		StopCoroutine(currentActionCoroutine);

		//死亡したら倒れるモーション
		if (status.IsDead()) {
			Death();
		}
		//まだ生きていたらダメージモーション
		else {
			currentState = EnemyState.Damage;
			//ダメージ状態に移行
			currentActionCoroutine = StartCoroutine(damage.Action(ActionCallBack));
			//enemyAnimation.StartDamageAnimation();
		}
	}

	/// <summary>
	/// 行動決定
	/// </summary>
	/// <returns></returns>
	protected override Coroutine Think() {
		isAction = true;

		if (status.IsDead()) {
			return StartCoroutine(death.Action(DeadEnd));
		}

		switch (currentState) {
			//待機の次はプレイヤーに接近
			case EnemyState.Idle:
				currentState = EnemyState.MoveNear;
				return StartCoroutine(nearPlayer.Action(ActionCallBack));

			//攻撃の次は待機
			case EnemyState.Attack:
				currentState = EnemyState.Idle;
				return StartCoroutine(idle.Action(ActionCallBack));

			//ダメージを受けた後は待機
			case EnemyState.Damage:
				currentState = EnemyState.Idle;
				return StartCoroutine(idle.Action(ActionCallBack));

			//接近した後は十分近づいていたら攻撃、もしくは待機
			case EnemyState.MoveNear:
				if (nearPlayer.isNearPlayer) {
					currentState = EnemyState.Attack;

					//優先順位は二度斬り、回転斬り、振り回し歩きの順に高い
					if (twiceShash.CanAttack(player)) {
						return StartCoroutine(twiceShash.Action(ActionCallBack));
					} else if (rotateSlash.CanAttack(player)) {
						return StartCoroutine(rotateSlash.Action(ActionCallBack));
					} else {
						return StartCoroutine(hurimawashiAruki.Action(ActionCallBack));
					}
				} else {
					currentState = EnemyState.Idle;
					return StartCoroutine(idle.Action(ActionCallBack));
				}

			default:
				return StartCoroutine(idle.Action(ActionCallBack));
		}
	}

	private void Update() {
		Action();
	}

	/// <summary>
	/// 行動する
	/// </summary>
	private void Action() {
		//行動中なら何もしない
		if (isAction) return;

		//行動していなければ行動を決定し実行
		currentActionCoroutine = Think();
	}

	/// <summary>
	/// 行動終了時に呼ばれる
	/// </summary>
	public void ActionCallBack() {
		isAction = false;
	}

	/// <summary>
	/// 死亡処理
	/// </summary>
	private void Death() {
		StopCoroutine(currentActionCoroutine);
		currentActionCoroutine = Think();
	}

	/// <summary>
	/// 死亡後に呼ばれる
	/// </summary>
	public void DeadEnd() {
		canUseHeal = deathByRepl;

		if (deathByRepl) {
			ShowBeam();
		} else {
			Extinction();
		}
	}

	public override void UsedHeal() {
		base.UsedHeal();
		Destroy(this.gameObject);
	}
}
