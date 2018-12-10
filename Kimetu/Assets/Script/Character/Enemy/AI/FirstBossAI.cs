using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirstBossAI : EnemyAI, IEnemyInfoProvider {
	[SerializeField, Tooltip("待機アクション")]
	private IdleAction idle;
	[SerializeField, Tooltip("振り下ろし")]
	private AttackAction swingAttack;
	[SerializeField, Tooltip("薙ぎ払い")]
	private AttackAction nagiharaiAttack;
	[SerializeField, Tooltip("プレイヤー接近アクション")]
	private NearPlayerAction nearPlayer;
	[SerializeField, Tooltip("ダメージアクション")]
	private DamageAction damage;
	[SerializeField, Tooltip("死亡アクション")]
	private DeathAction death;
	private EnemyStatus status;
	private DamageMode damageMode;

	public string informationText { private set; get; }

	protected override void Start() {
		base.Start();
		status = GetComponent<EnemyStatus>();
		currentActionCoroutine = Think();
		canUseHeal = false;
	}

	public override void Countered() {
		//行動を停止し、ダメージアクションに移行
		StopAction();
		isAction = true;
		reserveStates.Clear();
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
		StopAction();

		//死亡したら倒れるモーション
		if (status.IsDead()) {
			Death();
		}
		//まだ生きていたらダメージモーション
		else {
			ShowDamageEffect();
			//ダメージ状態に移行
			isAction = true;
			reserveStates.Clear();
			currentActionCoroutine = CoroutineManager.Instance.StartCoroutineEx(damage.Action(ActionCallBack));
			currentState = EnemyState.Damage;
		}
	}

	/// <summary>
	/// 行動決定
	/// </summary>
	/// <returns></returns>
	protected override Coroutine Think() {
		isAction = true;

		//死亡していたら死亡アクションに移行
		if (status.IsDead()) {
			currentState = EnemyState.Death;
			return CoroutineManager.Instance.StartCoroutineEx(death.Action(DeadEnd));
		}

		//次の行動をとる(ない場合はNoneが返ってくる)
		EnemyState nextReservedState = reserveStates.FirstOrDefault();

		//行動予約がある
		if (nextReservedState != EnemyState.None) {
			reserveStates.RemoveAt(0);

			//接近行動が予約されている
			if (nextReservedState == EnemyState.MoveNear) {
				currentState = EnemyState.MoveNear;
				return StartCoroutine(nearPlayer.Action(ActionCallBack));
			}
		}

		switch (currentState) {
			//待機の次はプレイヤーに接近
			case EnemyState.Idle:
				currentState = EnemyState.MoveNear;
				return CoroutineManager.Instance.StartCoroutineEx(nearPlayer.Action(ActionCallBack));

			//攻撃の次は待機
			case EnemyState.Attack:
				currentState = EnemyState.Idle;
				return CoroutineManager.Instance.StartCoroutineEx(idle.Action(ActionCallBack, 0.25f));

			//ダメージを受けた後は待機後接近する。
			case EnemyState.Damage:
				currentState = EnemyState.Idle;
				reserveStates.Add(EnemyState.MoveNear);
				return CoroutineManager.Instance.StartCoroutineEx(idle.Action(ActionCallBack, 0.5f));

			//接近した後は十分近づいていたら攻撃、もしくは待機
			case EnemyState.MoveNear:
				if (nearPlayer.isNearPlayer) {
					currentState = EnemyState.Attack;

					//振り下ろしが当たる範囲なら振り下ろす
					if (swingAttack.CanAttack(player)) {
						return CoroutineManager.Instance.StartCoroutineEx(swingAttack.Action(ActionCallBack));
					} else {
						return CoroutineManager.Instance.StartCoroutineEx(nagiharaiAttack.Action(ActionCallBack));
					}
				} else {
					currentState = EnemyState.Idle;
					return CoroutineManager.Instance.StartCoroutineEx(idle.Action(ActionCallBack));
				}

			default:
				return CoroutineManager.Instance.StartCoroutineEx(idle.Action(ActionCallBack));
		}
	}

	private void Update() {
		Action();
		this.informationText = currentState.ToString();

		if (currentState == EnemyState.MoveNear) {
			informationText = nearPlayer.informationText;
		}

		UpdateAura();
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
