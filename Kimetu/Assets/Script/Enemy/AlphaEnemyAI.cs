using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class AlphaEnemyAI : AI
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private EnemyAction action;
    [SerializeField]
    private EnemyAnimation animation;
    [SerializeField]
    private EnemyStatus status;
    [SerializeField]
    private float attackRange = 1.5f;
    [SerializeField]
    private float routeCalculateInterval = 1.0f;

    private GameObject playerObj;

    private EnemyAttackableArea attackableArea;
    [SerializeField]
    private float searchRange = 5f;

    private bool isFindPlayer = false;

    private void Awake()
    {
        attackableArea = GetComponent<EnemyAttackableArea>();
    }
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        if (agent == null)
        {
            this.agent = GetComponent<NavMeshAgent>();
        }
        this.playerObj = GameObject.FindWithTag(TagName.Player.String());
        if (action == null)
        {
            this.action = GetComponent<EnemyAction>();
        }
        if (animation == null)
        {
            this.animation = new EnemyAnimation(GetComponent<Animator>());
        }
        if (status == null)
        {
            this.status = GetComponent<EnemyStatus>();
        }
        Assert.IsTrue(agent != null);
        Assert.IsTrue(playerObj != null);
        Assert.IsTrue(action != null);
        Assert.IsTrue(animation != null);
        Assert.IsTrue(status != null);
        StartCoroutine(Research());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //EnemyAttackableAreaスクリプトで変更したらそっち優先(視覚情報あるので)
        attackRange = attackableArea.AttackRange;
        searchRange = attackableArea.SearchRange;
    }

    private IEnumerator Research()
    {
        //1秒ごとに自分と敵の間の距離をみて、
        //ある程度離れていたなら追跡する
        var wait = new WaitForSeconds(routeCalculateInterval);
        while (true)
        {
            yield return wait;
            if (playerObj == null)
            {
                break;
            }
            if (status.GetHP() <= 0)
            {
                //TODO:すべてのアニメーションを終了する
                break;
            }

            Search();
            if (isFindPlayer)
            {
                Behaivour();
            }
        }
    }

    private void Behaivour()
    {
        var distance = Vector3.Distance(playerObj.transform.position, transform.position);
        if (distance > attackRange)
        {
            agent.SetDestination(playerObj.transform.position);
            action.Run();
        }
        else
        {
            if (attackableArea.IsPlayerInArea(playerObj, EnemyAttackableArea.Area.Attackable))
            {
                //ある程度距離が近くなったので攻撃を実行する
                //且つ範囲内
                action.Attack();

            }

            //ある程度距離が近くなったので攻撃を実行する
            //action.Attack();

        }
    }

    private void Search()
    {
        var distance = Vector3.Distance(playerObj.transform.position, transform.position);
        //視野距離内に
        if (distance < searchRange)
        {
            Debug.Log("isNotFind_INRange");

            //視野角度だったら
            if (attackableArea.IsPlayerInArea(playerObj, EnemyAttackableArea.Area.Searchable))
            {
                isFindPlayer = true;
                Debug.Log("isFind");
            }
            //ゲームルール的に一回見つかれたら戦闘しないといけないので
            //isFindPlayerをfalseにする処理は現在なし
        }
        else
        {
            Debug.Log("isNotFind_OUTRange");
        }
    }
}
