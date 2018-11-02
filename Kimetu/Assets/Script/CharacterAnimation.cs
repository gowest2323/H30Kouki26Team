using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class CharacterAnimation : MonoBehaviour
{
    protected Animator animator;

    /// <summary>
    /// アニメーションの速度を 0-1 の範囲で設定します。
    /// </summary>
    /// <value></value>
    public float speed
    {
        set
        {
            animator.speed = value;
        }
        get
        {
            return animator.speed;
        }
    }

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    protected virtual void Awa()
    {
        animator = GetComponent<Animator>();
        speed = 1.0f;
    }

    /// <summary>
    /// アニメーションが終了したか？
    /// </summary>
    /// <param name="epsilon">誤差</param>
    /// <param name="layerNo">判定するアニメーションのレイヤー番号</param>
    /// <returns></returns>
    public bool IsEndAnimation(float epsilon, int layerNo = 0)
    {
        AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(layerNo);
        return animatorInfo.normalizedTime < 1.0f - epsilon;
    }
}
