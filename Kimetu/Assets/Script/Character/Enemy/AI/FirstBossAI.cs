using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class FirstBossAI : EnemyAI, IEnemyInfoProvider {

	[SerializeField]
	private IdleAction idle;
	[SerializeField]
	private ChasePlayer chasePlayer;
	[SerializeField]
	private AttackAction attack;
	[SerializeField]
	private AttackAction attack2;
	[SerializeField]
	private DamageAction damage;
	[SerializeField]
	private DeathAction death;
	public string informationText { private set; get; }

	protected override void Start() {
		base.Start();
		death.deadEnd = DeadEnd;
	}

	protected override Coroutine Think() {
		//行動予約がある
		if (reserveStates.Count > 0) {
			EnemyState next = reserveStates[0];
			reserveStates.RemoveAt(0);
			return StartAction(next);
		} else {
			switch (currentState) {
				case EnemyState.Idle:
					return StartAction(EnemyState.MoveNear);

				case EnemyState.Attack:
					idle.waitSecond = 0.5f;
					NewReserve(EnemyState.MoveNear);
					return StartAction(EnemyState.Idle);

				case EnemyState.Damage:
					idle.waitSecond = 0.5f;
					NewReserve(EnemyState.MoveNear);
					return StartAction(EnemyState.Idle);

				case EnemyState.MoveNear:
					if (chasePlayer.isNearPlayer) {
						if (attack.CanAttack()) {
							return StartAttackAction(attack);
						} else {
							return StartAttackAction(attack2);
						}
					}

					break;

				case EnemyState.Death:
					return StartAction(EnemyState.Death);

				default:
					break;
			}
		}

		Debug.LogError("未設定の状態遷移が実行されようとしました。" + currentState);
		return null;
	}

	private Coroutine StartAction(EnemyState nextState) {
		this.currentState = nextState;

		switch (nextState) {
			case EnemyState.Idle:
				currentAction = idle;
				return CoroutineManager.Instance.StartCoroutineEx(idle.Action());

			case EnemyState.Attack:
				Debug.LogError("不正な形でAttackが実行されようとしました。");
				currentAction = attack;
				return CoroutineManager.Instance.StartCoroutineEx(attack.Action());

			case EnemyState.Damage:
				currentAction = damage;
				return CoroutineManager.Instance.StartCoroutineEx(damage.Action());

			case EnemyState.MoveNear:
				currentAction = chasePlayer;
				return CoroutineManager.Instance.StartCoroutineEx(chasePlayer.Action());

			case EnemyState.Death:
				currentAction = death;
				return CoroutineManager.Instance.StartCoroutineEx(death.Action());

			default:
				Debug.LogError("不正な行動の呼び出しがありました。 " + nextState);
				return null;
		}
	}

	private Coroutine StartAttackAction(AttackAction attack) {
		currentState = EnemyState.Attack;
		currentAction = attack;
		return CoroutineManager.Instance.StartCoroutineEx(attack.Action());
	}

	public override void OnHit(DamageSource damageSource) {
		//既に死亡していたら何もしない
		if (status.IsDead()) {
			return;
		}

		ApplyDamage(damageSource);
		StopAction();

		if (status.IsDead()) {
			NewReserve(EnemyState.Death, true);
		} else {
			ShowDamageEffect();
			damage.damagePattern = DamagePattern.Normal;
			NewReserve(EnemyState.Damage, true);
		}
	}

	/// <summary>
	/// カウンターされた
	/// </summary>
	public override void Countered() {
		StopAction();
		damage.damagePattern = DamagePattern.Countered;
		NewReserve(EnemyState.Damage, true);
	}
}
