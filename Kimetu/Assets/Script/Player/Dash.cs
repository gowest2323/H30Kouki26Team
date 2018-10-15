using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダッシュ管理
/// </summary>
public class Dash
{
    private float decreaseStaminaPerSecond; //1スタミナが何秒で減るか
    public float dashTimeCounter { private set; get; } //ダッシュ時間計測用
    private int decreasedStaminaNum; //スタミナがいくつ減ったか
    public delegate void DecreaseEvent();
    private DecreaseEvent decreaseEvent; //減少時に呼ばれる関数
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="decreaseStaminaPerSecond"></param>
    /// <param name="decreaseEvent"></param>

    public Dash(float decreaseStaminaPerSecond, DecreaseEvent decreaseEvent)
    {
        this.decreaseStaminaPerSecond = decreaseStaminaPerSecond;
        this.decreaseEvent = decreaseEvent;
        Reset();
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void Reset()
    {
        dashTimeCounter = 0.0f;
        decreasedStaminaNum = 1;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="deltaTime">差分時間</param>
    public void Update(float deltaTime)
    {
        dashTimeCounter += deltaTime;
        //今必要な減少回数分スタミナが減っているか調べる
        while (dashTimeCounter > decreasedStaminaNum * decreaseStaminaPerSecond)
        {
            decreasedStaminaNum++;
            decreaseEvent();
        }
    }
}
