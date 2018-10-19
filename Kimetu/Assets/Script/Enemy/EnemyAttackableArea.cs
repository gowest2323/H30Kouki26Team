using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackableArea : MonoBehaviour
{
    [SerializeField, Range(0.0f, 360.0f)]
    private float attackbleAngle = 0.0f;

    private float attackableAreaCosTheta = 0.0f;


    // Use this for initialization
    void Awake ()
    {
        ApplyAttackableAngle();
    }

    // Update is called once per frame
    void Update ()
    {
	}

    // シリアライズされた値がインスペクター上で変更されたら呼ばれます。
    private void OnValidate()
    {
        ApplyAttackableAngle();
    }

    private void ApplyAttackableAngle()
    {
        float attackableRad = attackbleAngle * 0.5f * Mathf.Deg2Rad;
        attackableAreaCosTheta = Mathf.Cos(attackableRad);
    }


    public bool IsPlayerInAttackableArea(GameObject player)
    {
        Vector3 targetPos = player.transform.position;
        Vector3 myPos = transform.position;

        Vector3 targetPosXZ = Vector3.Scale(targetPos, new Vector3(1, 0, 1));
        Vector3 myPosXZ = Vector3.Scale(myPos, new Vector3(1, 0, 1));

        Vector3 toTargetFlatDir = (targetPosXZ - myPosXZ).normalized;

        if (IsWithinRangeAngle(transform.forward, toTargetFlatDir, attackableAreaCosTheta))
        {
            return true;
        }
        return false;
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


}
