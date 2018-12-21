using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class LastBossAI : EnemyAI, IEnemyInfoProvider {
	[SerializeField]
	private IdleAction idle;
	[SerializeField]
	private ChasePlayer chasePlayer;
	[SerializeField]
	private AttackAction combo;
	[SerializeField]
	private AttackAction nagiharai;
	[SerializeField]
	private AttackAction thunder;
	[SerializeField]
	private DamageAction damage;
	[SerializeField]
	private DeathAction death;

	public string informationText { private set; get; }

	[Serializable]
	private class ThunderHPBorder {
		[SerializeField]
		public int hp;
		[HideInInspector]
		public bool done;
	}
	[SerializeField]
	private List<ThunderHPBorder> thunderHPBorders;

	protected override void Start() {
		base.Start();
		death.deadEnd = DeadEnd;
	}

	protected override Coroutine Think() {
		//行動予約がある
		if (reserveStates.Count > 0) {
			EnemyState next = reserveStates[0];
			reserveStates.RemoveAt(0);

			if (next == EnemyState.Attack) {
				return StartAttackAction(thunder);
			}

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

					////優先順位に応じて攻撃
					if (combo.CanAttack()) {
						return StartAttackAction(combo);
					} else {
						return StartAttackAction(nagiharai);
					}

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
				currentAction = combo;
				return CoroutineManager.Instance.StartCoroutineEx(combo.Action());

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

		if (status.IsDead()) {
			NewReserve(EnemyState.Death, true);
			StopAction();
		} else {
			ShowDamageEffect();

			//今の状態がこうげきを受けたことで停止するか
			if (DamagedCancelAction(currentState)) {
				StopAction();
			}

			if (Slow.Instance.isSlowNow) {
				damage.damagePattern = DamagePattern.Normal;
				NewReserve(EnemyState.Damage, true);
			}

			Debug.Log(status.GetHP());

			for (int i = 0; i < thunderHPBorders.Count; i++) {
				if (thunderHPBorders[i].done) continue;

				if (status.GetHP() < thunderHPBorders[i].hp) {
					thunderHPBorders[i].done = true;
					NewReserve(EnemyState.Attack, false);
					break;
				}
			}
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
