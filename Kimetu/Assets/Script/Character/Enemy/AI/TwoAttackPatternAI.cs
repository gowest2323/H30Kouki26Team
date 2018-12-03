using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 二つの攻撃方法を持つ敵AI
/// priorityAttackの範囲内にいなければsecondaryAttackを繰り出す。
/// </summary>
public class TwoAttackPatternAI : EnemyAI, IDamageable {
	private bool isAction; //行動中か？
	[SerializeField]
	private IdleAction idle;
	[SerializeField, Header("優先して出す攻撃")]
	private AttackAction priorityAttack;
	[SerializeField, Header("優先攻撃ができないときの攻撃")]
	private AttackAction secondaryAttack;
	[SerializeField]
	private SearchAction search;
	[SerializeField]
	private NearPlayerAction nearPlayer;
	[SerializeField]
	private DamageAction damage;
	[SerializeField]
	private DeathAction death;
	[SerializeField]
	private GameObject player;
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

		switch (currentState) {
			//待機の次はプレイヤーに接近
			case EnemyState.Idle:
				currentState = EnemyState.Search;
				return StartCoroutine(search.Action(ActionCallBack));

			//攻撃の次は待機
			case EnemyState.Attack:
				currentState = EnemyState.Idle;
				return StartCoroutine(idle.Action(ActionCallBack));

			//索敵でプレイヤーを見つけたかどうかで分岐
			case EnemyState.Search:
				if (search.canSearched) {
					currentState = EnemyState.MoveNear;
					return StartCoroutine(nearPlayer.Action(ActionCallBack));
				} else {
					currentState = EnemyState.Idle;
					return StartCoroutine(idle.Action(ActionCallBack));
				}

			//ダメージを受けた後は待機
			case EnemyState.Damage:
				currentState = EnemyState.Idle;
				return StartCoroutine(idle.Action(ActionCallBack));

			//接近した後は十分近づいていたら攻撃、もしくは待機
			case EnemyState.MoveNear:
				if (nearPlayer.isNearPlayer) {
					currentState = EnemyState.Attack;

					//優先攻撃が当たる範囲ならその攻撃
					if (priorityAttack.CanAttack(player)) {
						return StartCoroutine(priorityAttack.Action(ActionCallBack));
					}
					//範囲外なら次の攻撃
					else {
						return StartCoroutine(secondaryAttack.Action(ActionCallBack));
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
		currentActionCoroutine = StartCoroutine(death.Action(DeadEnd));
		isAction = true;
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
}
