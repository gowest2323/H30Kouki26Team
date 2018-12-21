using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class EnemyAI : MonoBehaviour, IDamageable {
	protected EnemyState currentState; //現在の状態
	protected List<EnemyState> reserveStates; //行動予約
	protected ActionBase currentAction;
	protected Coroutine currentActionCoroutine; //現在の行動コルーチン
	[SerializeField, Header("腰オブジェクトの名前")]
	protected string waistObjectName = "mixamorig:Hips/mixamorig:Spine";
	protected Transform waist; //腰オブジェクト
	protected GameObject player;
	protected Status status;
	protected bool endActionFlag;
	private System.IDisposable observer;

	[SerializeField, Header("黒くなって消えるのにかかる時間")]
	private float extinctionSeconds = 2.5f;

	[SerializeField, Header("沈んで消えるのにかかる時間")]
	private float sinkSeconds = 1.5f;

	[SerializeField, Header("沈む高さ")]
	private float sinkHeight = 2f;

	private PauseManager pauseManager;

	protected virtual void Start() {
		endActionFlag = false;
		waist = transform.Find(waistObjectName);
		UnityEngine.Assertions.Assert.IsNotNull(waist, "waist not found");
		player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		UnityEngine.Assertions.Assert.IsNotNull(player, "player not found");
		reserveStates = new List<EnemyState>();
		reserveStates.Add(EnemyState.Idle);
		currentState = EnemyState.Idle;
		this.auraPlace = gameObject.transform.parent.FindRec("mixamorig:Neck");
		this.status = GetComponent<Status>();
		CoroutineManager.Instance.StartCoroutineEx(Loop());
		this.observer = Slow.Instance.onStart.Subscribe((e) => {
			//ここでエフェクトを作成
			EffectManager.Instance.EnemySlowAuraCreate(auraPlace);
		});
		this.pauseManager = PauseManager.GetInstance();
	}

	// Use this for initialization
	protected virtual IEnumerator Loop() {
		yield return null;

		while (true) {
			float t = Time.time;
			yield return Think();
			Debug.Log(currentState + "行動時間: " + (Time.time - t));

			if (endActionFlag) {
				break;
			}
		}
	}

	/// <summary>
	/// 腰の座標
	/// </summary>
	public Vector3 waistPosition { get { return waist.position; } }

	/// <summary>
	/// 吸生に使われられるか？
	/// </summary>
	public bool canUseHeal { protected set; get; }

	/// <summary>
	/// はじきによって殺されたなら true.
	/// </summary>
	/// <value></value>
	public bool deathByRepl { private set; get; }

	public virtual void UsedHeal() {
		canUseHeal = false;
		Destroy(this.gameObject);
	}

	/// <summary>
	/// 次の行動を決定する
	/// </summary>
	/// <returns></returns>
	protected abstract Coroutine Think();
	public abstract void OnHit(DamageSource damageSource);
	public abstract void Countered();

	[SerializeField]
	private GameObject dieEffectPrefab;

	protected GameObject auraPlace;//オーラエフェクトの位置

	/// <summary>
	/// ダメージを適用します。
	/// </summary>
	/// <param name="damageSource"></param>
	protected void ApplyDamage(DamageSource damageSource) {
		var status = GetComponent<Status>();
		var isBoss = GetComponent<BossMarker>() != null;

		//ボスははじかないと死なない
		if (isBoss) {
			status.Damage(damageSource.damage, (Slow.Instance.isSlowNow ? DamageMode.Kill : DamageMode.NotKill));
		} else {
			status.Damage(damageSource.damage, DamageMode.Kill);
		}
		if(!status.IsDead()) {
			AudioManager.Instance.PlayEnemySE(AudioName.oni_uu_damage_04.String());
		} else {
			AudioManager.Instance.PlayEnemySE(AudioName.oni_aa_taore_05.String());
		}

		if (Slow.Instance.isSlowNow && status.IsDead()) {
			this.deathByRepl = true;
		}
	}

	/// <summary>
	/// オーラエフェクトを更新します。
	/// </summary>
	protected void UpdateAura() {
		if (!status.IsDead()) {
			EffectManager.Instance.EnemyAuraCreate(auraPlace);
		}
	}

	/// <summary>
	/// アニメーションが終了するまで待機したあとビームを発射します。
	/// </summary>
	/// <returns></returns>
	protected void ShowBeam() {
		StartCoroutine(StartShowBeam());
	}

	/// <summary>
	/// 鬼の体を五秒かけて黒くしたあと消します。
	/// </summary>
	protected void Extinction() {
		StartCoroutine(StartExtinction());
	}

	/// <summary>
	/// ダメージエフェクトを発生させます。
	/// ボスの場合は専用のものを発生させます。
	/// </summary>
	protected void ShowDamageEffect() {
		var status = GetComponent<Status>();
		var isBoss = GetComponent<BossMarker>() != null;

		if (status.IsDead()) {
			return;
		}

		if (isBoss && status.GetHP() == 1) {
			EffectManager.Instance.EnemyDamageEffectCreate(gameObject, true);
		} else {
			EffectManager.Instance.EnemyDamageEffectCreate(gameObject, false);
		}
	}

	private IEnumerator StartExtinction() {
		var hook = GetComponentInParent<OniDeadHook>();
		var enemyAnimation = GetComponentInParent<EnemyAnimation>();
		yield return enemyAnimation.WaitAnimation("oni", "dead");
		var offset = 0f;
		var seconds = extinctionSeconds;
		var separate = 100;
		var mat = GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
		var start = mat.color;
		var end = Color.black;
		var particle = GameObject.Instantiate(dieEffectPrefab);
		particle.transform.position = GetComponentInChildren<BeamShot>().transform.position;

		while (offset < seconds) {
			yield return new WaitForSeconds(seconds / separate);
			offset += (seconds / separate);
			mat.color = Color.Lerp(start, end, offset / seconds);
		}

		mat.color = Color.black;
		yield return new WaitForEndOfFrame();
		//黒くして沈める
		offset = 0f;
		seconds = sinkSeconds;
		var startPos = transform.position;

		while (offset < seconds) {
			var t = Time.time;
			yield return new WaitForEndOfFrame();
			offset += (Time.time - t);
			//沈める
			var newPos = startPos;
			newPos.y -= sinkHeight * (offset / seconds);
			transform.position = newPos;
		}

		GameObject.Destroy(particle);
		GameObject.Destroy(gameObject);
	}

	private IEnumerator StartShowBeam() {
		var hook = GetComponentInParent<OniDeadHook>();
		var beam = GetComponentInChildren<BeamShot>();
		var enemyAnimation = GetComponentInParent<EnemyAnimation>();
		yield return enemyAnimation.WaitAnimation("oni", "dead");
		//yield return hook.Wait();
		beam.StartShot();
	}

	protected void StopAction() {
		currentAction.Cancel();
	}

	/// <summary>
	/// 行動の予約をする
	/// </summary>
	/// <param name="state">予約するステート</param>
	/// <param name="oldClear">古い予約をクリアするか</param>
	protected void NewReserve(EnemyState state, bool oldClear = false) {
		if (oldClear) {
			reserveStates.Clear();
		}

		reserveStates.Add(state);
	}

	/// <summary>
	/// 死亡後に呼ばれる
	/// </summary>
	public void DeadEnd() {
		canUseHeal = deathByRepl;
		endActionFlag = true;

		if (deathByRepl) {
			ShowBeam();
		} else {
			Extinction();
		}
	}

	/// <summary>
	/// ダメージを受けると行動がキャンセルされるか
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	protected virtual bool DamagedCancelAction(EnemyState state) {
		return state == EnemyState.Idle
			   || state == EnemyState.MoveDefaultPosition
			   || state == EnemyState.Search
			   || state == EnemyState.MoveNear;
	}

	void OnDestroy() {
		observer.Dispose();
	}
	void Update() {
		if(!pauseManager.isPause) {
			UpdateAura();
		}
	}
}
