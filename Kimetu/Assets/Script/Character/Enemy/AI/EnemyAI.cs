using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour, IDamageable {
	protected EnemyState currentState; //現在の状態
	protected Coroutine currentActionCoroutine; //現在の行動コルーチン
	[SerializeField, Header("腰オブジェクトの名前")]
	protected string waistObjectName = "mixamorig:Hips/mixamorig:Spine";
	protected Transform waist; //腰オブジェクト
	protected List<EnemyState> reserveStates; //行動予約
	protected GameObject player;

	protected virtual void Start() {
		waist = transform.Find(waistObjectName);
		UnityEngine.Assertions.Assert.IsNotNull(waist, "waist not found");
		player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		UnityEngine.Assertions.Assert.IsNotNull(player, "player not found");
		reserveStates = new List<EnemyState>();
		currentState = EnemyState.Idle;
	}

	public void AddState(EnemyState state) { reserveStates.Add(state); }

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

		if (Slow.Instance.isSlowNow && status.IsDead()) {
			this.deathByRepl = true;
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
		if(status.IsDead()) {
			return;
		}
		if(isBoss && status.GetHP()==1) {
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
		var seconds = 5f;
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
}
