﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OneAttackPatternAI : EnemyAI, IEnemyInfoProvider {
	private bool isAction; //行動中か？
	[SerializeField, Tooltip("待機アクション")]
	private IdleAction idle;
	[SerializeField, Tooltip("攻撃アクション")]
	private AttackAction attack;
	[SerializeField, Tooltip("初期位置に戻るアクション")]
	private DefaultPositionMoveAction defaultPositionMove;
	[SerializeField, Tooltip("サーチアクション")]
	private SearchAction search;
	[SerializeField, Tooltip("プレイヤー接近アクション")]
	private NearPlayerAction nearPlayer;
	[SerializeField, Tooltip("ダメージアクション")]
	private DamageAction damage;
	[SerializeField, Tooltip("死亡アクション")]
	private DeathAction death;
	private EnemyStatus status; //敵のステータス

	public string informationText { private set; get; }

	protected override void Start() {
		base.Start();
		status = GetComponent<EnemyStatus>();
		currentActionCoroutine = Think();
		canUseHeal = false;
	}

	public override void Countered() {
		//行動を停止し、ダメージアクションに移行
		if (currentActionCoroutine != null) {
			StopCoroutine(currentActionCoroutine);
		}

		currentActionCoroutine = StartCoroutine(damage.Action(ActionCallBack, DamagePattern.Countered));
		currentState = EnemyState.Damage;
		return;
	}

	/// <summary>
	/// ダメージを受ける
	/// </summary>
	/// <param name="damageSource">ダメージ情報</param>
	public override void OnHit(DamageSource damageSource) {
		//すでに死亡しているなら何もしない
		//これがないと、死亡したエネミーに攻撃が当たったとき、
		//エネミーのローテーションがおかしくなる(PassOutが実行されるため??)
		if (status.IsDead()) { return; }

		ApplyDamage(damageSource);

		//死亡したとき
		if (status.IsDead()) {
			Death();
		} else {
			//行動を停止し、ダメージアクションに移行
			if (currentActionCoroutine != null) {
				StopCoroutine(currentActionCoroutine);
			}

			currentActionCoroutine = StartCoroutine(damage.Action(ActionCallBack));
			currentState = EnemyState.Damage;
		}
	}

	protected override Coroutine Think() {
		isAction = true;
		//次の行動をとる(ない場合はNoneが返ってくる)
		EnemyState nextReservedState = reserveStates.FirstOrDefault();

		//行動予約がある
		if (nextReservedState != EnemyState.None) {
			reserveStates.RemoveAt(0);
			EnemyState next = nextReservedState;

			//接近行動が予約されている
			if (next == EnemyState.MoveNear) {
				currentState = EnemyState.MoveNear;
				return StartCoroutine(nearPlayer.Action(ActionCallBack));
			}
			//元の場所に戻ることが予約されている
			else if (next == EnemyState.MoveDefaultPosition) {
				currentState = EnemyState.MoveDefaultPosition;
				return StartCoroutine(defaultPositionMove.Action(ActionCallBack));
			}
		}

		//現在の状態によって分岐
		switch (currentState) {
			//待機状態からはプレイヤーを探索
			case EnemyState.Idle:
				if (idle.isFindPlayer) {
					currentState = EnemyState.MoveNear;
					return StartCoroutine(nearPlayer.Action(ActionCallBack));
				}
				//元の場所にいなければ元の場所に戻る
				else if (!defaultPositionMove.IsDefaultPosition()) {
					currentState = EnemyState.MoveDefaultPosition;
					return StartCoroutine(defaultPositionMove.Action(ActionCallBack));
				} else {
					currentState = EnemyState.Search;
					return StartCoroutine(search.Action(ActionCallBack));
				}

			//攻撃後は待機
			case EnemyState.Attack:
				currentState = EnemyState.Idle;
				return StartCoroutine(idle.Action(ActionCallBack, 0.25f));

			//索敵後、プレイヤーを見つけていたら接近。そうでなければ待機。
			case EnemyState.Search:
				if (search.canSearched) {
					currentState = EnemyState.MoveNear;
					return StartCoroutine(nearPlayer.Action(ActionCallBack));
				} else {
					currentState = EnemyState.Idle;
					return StartCoroutine(idle.Action(ActionCallBack));
				}

			//ダメージを受けた後は待機後接近する。
			case EnemyState.Damage:
				reserveStates.Add(EnemyState.MoveNear);
				currentState = EnemyState.Idle;
				return StartCoroutine(idle.Action(ActionCallBack, 0.5f));

			//接近後はプレイヤーが近くにいれば攻撃
			case EnemyState.MoveNear:
				if (nearPlayer.isNearPlayer) {
					currentState = EnemyState.Attack;
					return StartCoroutine(attack.Action(ActionCallBack));
				}
				//プレイヤーが近くにおらず、見失ったなら初期位置に戻る
				else if (nearPlayer.isMissingPlayer) {
					currentState = EnemyState.Idle;
					reserveStates.Add(EnemyState.MoveDefaultPosition);
					return StartCoroutine(defaultPositionMove.Action(ActionCallBack));
				}
				//それ以外は待機。
				else {
					currentState = EnemyState.Idle;
					return StartCoroutine(idle.Action(ActionCallBack));
				}

			//初期位置に戻る行動の後はプレイヤーを発見していたら接近。
			case EnemyState.MoveDefaultPosition:
				if (defaultPositionMove.isDetectPlayer) {
					currentState = EnemyState.MoveNear;
					return StartCoroutine(nearPlayer.Action(ActionCallBack));
				}
				//発見していなければその場で待機。
				else {
					currentState = EnemyState.Idle;
					return StartCoroutine(idle.Action(ActionCallBack));
				}

			default:
				Debug.Log("default " + currentState);
				return StartCoroutine(idle.Action(ActionCallBack));
		}
	}

	private void Update() {
		Action();
		this.informationText = currentState.ToString();

		if (currentState == EnemyState.MoveNear) {
			informationText = nearPlayer.informationText;
		}
	}

	private void Action() {
		//行動中なら何もしない
		if (isAction) return;

		//行動していなければ行動を決定し実行
		currentActionCoroutine = Think();
	}

	public void ActionCallBack() {
		isAction = false;
	}

	private void Death() {
		if (currentActionCoroutine != null) {
			StopCoroutine(currentActionCoroutine);
		}

		currentActionCoroutine = StartCoroutine(death.Action(DeadEnd));
		currentState = EnemyState.Death;
		isAction = true;
	}

	public void DeadEnd() {
		canUseHeal = deathByRepl;

		if (deathByRepl) {
			ShowBeam();
		} else {
			Extinction();
		}
	}
}
