using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//NOTE:Enemyの種類ごとに XXXAction を用意する必要があるかもしれません
//     例えば AlphaBossAIに対応する AlphaBossActionなど

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyStatus))]

public class EnemyAction : MonoBehaviour, IDamageable, ICharacterAnimationProvider
{
    private EnemyAnimation enemyAnimation; //アニメーション管理
    private Status status; //ステータス管理
    private EnemyState state;

    [SerializeField]
    private Weapon weapon;
    /// <summary>
    /// 回生に使われられるか
    /// </summary>
    public bool canUseHeal { private set; get; }
	public CharacterAnimation characterAnimation { get { return enemyAnimation; }}

    // Use this for initialization
    void Start()
    {
        this.state = EnemyState.Idle;
        this.enemyAnimation = new EnemyAnimation(GetComponent<Animator>());
        status = GetComponent<Status>();
        canUseHeal = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Run() {
        this.state = EnemyState.Move;
        enemyAnimation.StartRunAnimation();
    }

    /// <summary>
    /// 攻撃する
    /// </summary>
    public void Attack()
    {
        if(this.state == EnemyState.Attack) {
            return;
        }
        StopRunAnimation();
        enemyAnimation.StartAttackAnimation();
        StartCoroutine(AttackStart());
    }

    /// <summary>
    /// 攻撃された
    /// </summary>
    /// <param name="damageSource">ダメージ情報</param>
    public void OnHit(DamageSource damageSource)
    {
        status.Damage(damageSource.damage);

        //死亡したら倒れるモーション
        if (status.IsDead())
        {
            StartCoroutine(PassOut());
        }
        //まだ生きていたらダメージモーション
        else
        {
            //enemyAnimation.StartDamageAnimation();
        }
    }

    private IEnumerator AttackStart()
    {
        this.state = EnemyState.Attack;
        weapon.AttackStart();
        //攻撃時間分待機する
        yield return new WaitForSeconds(1.0f);
        weapon.AttackEnd();
        this.state = EnemyState.Idle;
    }

    /// <summary>
    /// カウンターされる
    /// </summary>
    public void Countered()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 倒れる
    /// </summary>
    /// <returns></returns>
    private IEnumerator PassOut()
    {
        //AlphaBossAIはNavMeshAgentで敵を追いかけますが、
        //NavMeshAgentが有効だと通常の transform 変更は効かないようです。
        //その対策として暫定的に下記のコードを書きました。
        var navmesh = GetComponent<NavMeshAgent>();
        if(navmesh != null) Destroy(navmesh);
        //倒れるアニメーションが終了するまで待機
        this.transform.Rotate(new Vector3(0, 0, 90));
        yield return new WaitForSeconds(3.0f);
        canUseHeal = true;
    }

    /// <summary>
    /// 回生に使われた
    /// </summary>
    public void UsedHeal()
    {
        if (!canUseHeal) Debug.LogError("2回以上UsedHealが使われました。");
        canUseHeal = false;
        Destroy(this.gameObject, 0.5f);
    }
    private void StopRunAnimation() {
        if(this.state == EnemyState.Move) {
            enemyAnimation.StopRunAnimation();
        }
    }
}
