using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneAttackPatternAI : EnemyAI, IEnemyInfoProvider {
	private bool isAction; //行動中か？
	[SerializeField, Tooltip("待機アクション")]
	private IdleAction idle;
	[SerializeField, Tooltip("攻撃アクション")]
	private AttackAction attack;
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
		StopCoroutine(currentActionCoroutine);
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
			StopCoroutine(currentActionCoroutine);
			currentActionCoroutine = StartCoroutine(damage.Action(ActionCallBack));
			currentState = EnemyState.Damage;
		}
	}

	protected override Coroutine Think() {
		isAction = true;

		if (reserveState != EnemyState.None) {
			EnemyState next = reserveState;
			reserveState = EnemyState.None;

			if (next == EnemyState.MoveNear) {
				currentState = EnemyState.MoveNear;
				return StartCoroutine(nearPlayer.Action(ActionCallBack));
			}
		}

		switch (currentState) {
			case EnemyState.Idle:
				currentState = EnemyState.Search;
				return StartCoroutine(search.Action(ActionCallBack));

			case EnemyState.Attack:
				currentState = EnemyState.Idle;
				return StartCoroutine(idle.Action(ActionCallBack, 0.25f));

			case EnemyState.Search:
				if (search.canSearched) {
					currentState = EnemyState.MoveNear;
					return StartCoroutine(nearPlayer.Action(ActionCallBack));
				} else {
					currentState = EnemyState.Idle;
					return StartCoroutine(idle.Action(ActionCallBack));
				}

			case EnemyState.Damage:
				reserveState = EnemyState.MoveNear;
				currentState = EnemyState.Idle;
				return StartCoroutine(idle.Action(ActionCallBack, 0.5f));

			case EnemyState.MoveNear:
				if (nearPlayer.isNearPlayer) {
					currentState = EnemyState.Attack;
					return StartCoroutine(attack.Action(ActionCallBack));
				} else {
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
		if (isAction) return;

		currentActionCoroutine = Think();
	}

	public void ActionCallBack() {
		isAction = false;
	}

	private void Death() {
		StopCoroutine(currentActionCoroutine);
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
