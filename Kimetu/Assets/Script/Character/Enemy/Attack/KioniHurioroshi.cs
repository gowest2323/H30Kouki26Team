using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class KioniHurioroshi : EnemyAttack, IAttackEventHandler
{
    [SerializeField]
    private string animationClipName = "oni@360_m";
    private Transform playerTransform;
    private float attackTime;
    private Animator anim;
    private Transform topTransform;
    private System.IDisposable observer;
    private float rotateTime;
    private Vector3 aim;

    protected override void Start()
    {
        base.Start();
        playerTransform = GetPlayer().transform;
        topTransform = GetTopTransform();
        anim = enemyAnimation.anim;
        System.Type type = EnemyAttackTypeDictionary.typeDictionary[attackType];
        var attackHook = GetComponentInParent(type) as IEventHook;

        if (attackHook == null)
        {
            Debug.Log("type: " + type.Name);
        }

        this.observer = attackHook.trigger.Subscribe((e) => {
            if (e) { AttackStart(); }
            else { AttackEnd(); }
        });
        rotateTime = RotationTime();
    }

    private void OnDestroy()
    {
        //イベントの購読を終了
        observer.Dispose();
    }

    public override IEnumerator Attack()
    {
        //攻撃範囲描画
        //攻撃の位置にマーカーを合わせる
        aim = playerTransform.position - topTransform.position;
        attackAreaDrawer.putOffset = aim * 0.5f;
        DrawStartAttackArea();
        enemyAnimation.StartAttackAnimation(attackType);
        yield return WaitForce();
        yield return WaitStartAttackAnimation();
        StartCoroutine(Rotate());
        //攻撃アニメーション終了まで待機
        yield return WaitEndAttackAnimation();
        DrawEndAttackArea();
        cancelFlag = false;
    }

    private IEnumerator Rotate()
    {
        float time = 0.0f;
        Quaternion before = topTransform.rotation;
        Quaternion after = Quaternion.LookRotation(aim, Vector3.up);

        while (time < rotateTime)
        {
            float slowDelta = Slow.Instance.DeltaTime();
            time += slowDelta;
            float t = time / rotateTime;

            Quaternion rotate = Quaternion.Lerp(before, after, t);
            topTransform.rotation = rotate;
            yield return new WaitForSeconds(slowDelta);
        }

        topTransform.rotation = after;
    }

    /// <summary>
    /// 回転にかける時間を取得
    /// </summary>
    /// <returns></returns>
    private float RotationTime()
    {
        AnimationClip clip = FindAnimationClip(animationClipName);
        float startTime = clip.events[0].time;
        float endTime = clip.events[1].time;
        float rotateTime = endTime - startTime;
        return rotateTime;
    }
}
