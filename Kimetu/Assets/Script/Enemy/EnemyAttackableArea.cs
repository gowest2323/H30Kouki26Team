using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackableArea : MonoBehaviour
{
    [SerializeField, Range(0.0f, 360.0f)]
    private float attackbleAngle = 0.0f; //攻撃可能角度(60°->左右30°)
    [SerializeField]
    private float attackRange = 0.0f;
    public float AttackRange
    {
        get { return attackRange; }
    }

    [SerializeField, Range(0.0f, 360.0f)]
    private float searchableAngle = 0.0f; //視野角度(60°->左右30°)
    [SerializeField]
    private float searchRange = 0.0f;
    public float SearchRange
    {
        get { return searchRange; }
    }

    private float attackableAreaCosTheta = 0.0f;
    private float searchableAreaCosTheta = 0.0f;

    public enum Area
    {
        Attackable,
        Searchable
    }


    // Use this for initialization
    void Awake ()
    {
        ApplyAttackableAngle();
        ApplySearchableAngle();
    }

    // Update is called once per frame
    void Update ()
    {
	}

    // シリアライズされた値がインスペクター上で変更されたら呼ばれます。
    private void OnValidate()
    {
        ApplyAttackableAngle();
        ApplySearchableAngle();
    }

    private void ApplyAttackableAngle()
    {
        float attackableRad = attackbleAngle * 0.5f * Mathf.Deg2Rad;
        attackableAreaCosTheta = Mathf.Cos(attackableRad);
    }

    private void ApplySearchableAngle()
    {
        float searchableRad = searchableAngle * 0.5f * Mathf.Deg2Rad;
        searchableAreaCosTheta = Mathf.Cos(searchableRad);
    }

    /// <summary>
    /// 指定の範囲内にプレイヤーがいるか？
    /// </summary>
    /// <param name="player"></param>
    /// <param name="area">チェックする範囲</param>
    /// <returns></returns>
    public virtual bool IsPlayerInArea(GameObject player, Area area)
    {
        Vector3 targetPos = player.transform.position;
        Vector3 myPos = transform.position;

        Vector3 targetPosXZ = Vector3.Scale(targetPos, new Vector3(1, 0, 1));
        Vector3 myPosXZ = Vector3.Scale(myPos, new Vector3(1, 0, 1));

        Vector3 toTargetFlatDir = (targetPosXZ - myPosXZ).normalized;

        switch (area)
        {
            //攻撃範囲
            case Area.Attackable:
                if (!IsWithinRangeAngle(transform.forward, toTargetFlatDir, attackableAreaCosTheta))
                {
                    return false;
                }
                return true;

            //視野範囲
            case Area.Searchable:
                if (!IsWithinRangeAngle(transform.forward, toTargetFlatDir, searchableAreaCosTheta))
                {
                    return false;
                }

                Vector3 targetCheckPos = targetPos + Vector3.up * 1f;//腰あたりの高さ
                Vector3 myCheckPos = myPos + Vector3.up * 1;//腰あたりの高さ
                Vector3 toTargetDir = (targetCheckPos - myCheckPos).normalized;

                if (!IsHitRay(myCheckPos, toTargetDir, player))
                {
                    return false;
                }
                return true;

            default:
                return false;

        }
    }

    private bool IsWithinRangeAngle(Vector3 forwardDir, Vector3 toTargetDir, float cosTheta)
    {
        // 方向ベクトルが無い場合は、同位置にあるものだと判断する。
        if (toTargetDir.sqrMagnitude <= Mathf.Epsilon)
        {
            return true;
        }

        float dot = Vector3.Dot(forwardDir, toTargetDir);
        return dot >= cosTheta;
    }


    private bool IsHitRay(Vector3 fromPosition, Vector3 toTargetDir, GameObject target)
    {
        // 方向ベクトルが無い場合は、同位置にあるものだと判断する。
        if (toTargetDir.sqrMagnitude <= Mathf.Epsilon)
        {
            return true;
        }

        RaycastHit onHitRay;
        if (!Physics.Raycast(fromPosition, toTargetDir, out onHitRay, searchRange))
        {
            return false;
        }
        if (onHitRay.transform.tag != TagName.Player.String())
        {
            return false;
        }

        return true;
    }

    //MonoBehaviour多重継承しているのでpublicで
    private void OnDrawGizmos()
    {
        //視野範囲ギズモ
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        FanTypeGizmos.DrawFanGizmos(gameObject, searchableAngle, searchRange, Color.yellow);
        //攻撃範囲ギズモ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        FanTypeGizmos.DrawFanGizmos(gameObject, attackbleAngle, attackRange, Color.red);
    }
}
