﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyBoxSearchArea : EnemyAttackableArea
{
    [SerializeField,Header("目の場所")]
    private Transform eyeTransform; 
    private BoxCollider boxCollider; 
    private bool isPlayerInBoxArea; //プレイヤーがBoxの中にいるか
    private float searchableRange; //索敵範囲

    // Use this for initialization
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();       
        searchableRange = boxCollider.size.z * 2;
        isPlayerInBoxArea = false;
    }

    /// <summary>
    /// プレイヤーが範囲内にいるか？
    /// </summary>
    /// <param name="player"></param>
    /// <param name="area"></param>
    /// <returns></returns>
    public override bool IsPlayerInArea(GameObject player, EnemyAttackableArea.Area area)
    {
        //Box内にいなければ終了
        if (!isPlayerInBoxArea) return false;
        //自分からプレイヤーに向かってレイを飛ばす
        Vector3 targetPosition = player.transform.position;
        targetPosition.y = eyeTransform.position.y;
        Vector3 toTargetDir = (targetPosition - eyeTransform.position).normalized;
        //間に障害物がなければ範囲内にいる
        if (IsHitRay(eyeTransform.position, toTargetDir, player))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (!boxCollider) return;
        //箱型の範囲表示
        Gizmos.color = Color.blue;
        Matrix4x4 m = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = m;
        Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);

    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーが範囲内に入った
        if (other.tag == TagName.Player.String())
        {
            isPlayerInBoxArea = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        //プレイヤーが範囲内から出た
        if (other.tag == TagName.Player.String())
        {
            isPlayerInBoxArea = false;
        }
    }

    /// <summary>
    /// レイが当たったか
    /// </summary>
    /// <param name="fromPosition">始点</param>
    /// <param name="toTargetDir">方向</param>
    /// <param name="target">ターゲットとなるオブジェクト</param>
    /// <returns>当たったのがプレイヤーならtrue</returns>
    private bool IsHitRay(Vector3 fromPosition, Vector3 toTargetDir, GameObject target)
    {
        RaycastHit hit;
        //何も当たらなければ終了
        if (!Physics.Raycast(fromPosition, toTargetDir, out hit, searchableRange))
        {
            return false;
        }
        return hit.transform.tag == TagName.Player.String();
    }
}
