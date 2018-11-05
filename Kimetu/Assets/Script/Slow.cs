using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 181012 何
/// スロークラス
/// </summary>
public class Slow : SingletonMonoBehaviour<Slow>
{
    //再生速度
    [SerializeField]
    private float slowSpeed = 0.3f;
    //プレイヤーの再生速度
    [SerializeField]
    private float highSpeed = 1.2f;
    //スロー秒数
    [SerializeField]
    private float slowTime = 2f;
    //スローリスト
    private List<CharacterAnimation> slowAnimationList = new List<CharacterAnimation>();
    //スロー中か？
    private bool isSlow = false;
    [SerializeField]
    private SlowColorChanger colorChanger;
    private float currentPlayerSpeed = 1;
    private float currentOtherSpeed = 1;
    public bool isSlowNow { get { return isSlow; }}



    public float DeltaTime()
    {
        return Time.deltaTime * currentOtherSpeed;
    }

    public float PlayerDeltaTime()
    {
        return Time.deltaTime * currentPlayerSpeed;
    }

    /// <summary>
    /// スロー開始
    /// </summary>
    /// <param name="animationList"></param>
    public void SlowStart(List<CharacterAnimation> animationList)
    {
        //キャラクターのアニメーションリストを受け取る
        slowAnimationList = animationList;
        //コルーチン
        StartCoroutine(SlowCoroutine(slowTime, slowAnimationList));
    }

    private IEnumerator SlowCoroutine(float waitSeconds, List<CharacterAnimation> slowAnimList)
    {
        currentPlayerSpeed = slowSpeed;
        currentOtherSpeed = slowSpeed;
        colorChanger.SlowStart();
        isSlow = true;
        //アニメーションリストの再生速度をスローに
        foreach (var anim in slowAnimList)
        {
            Debug.Log("slow " + anim.gameObject.name);
            anim.speed = slowSpeed;
        }

        yield return new WaitForSeconds(waitSeconds);

        //アニメーションリストの再生速度をデフォ値に
        foreach (var anim in slowAnimList)
        {
            anim.speed = 1;
        }
        //スローリストをクリア
        slowAnimList.Clear();
        isSlow = false;
        colorChanger.SlowEnd();
        currentOtherSpeed = 1;
        currentPlayerSpeed = 1;
    }


    public void PlayerAttacked(CharacterAnimation playerAnimation)
    {
        //スロー中
        if (isSlow)
        {
            currentPlayerSpeed = highSpeed;
            playerAnimation.speed = highSpeed;
        }
    }

    public void Remove(CharacterAnimation animation)
    {
        //スロー中のアニメーションに
        //指定のアニメーションがあったら削除
        slowAnimationList.RemoveAll(anim => anim == animation);
    }



}
