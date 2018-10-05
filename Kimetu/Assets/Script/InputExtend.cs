﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 181005 何
/// Input拡張クラス
/// </summary>
public class InputExtend : MonoBehaviour
{
    /// <summary>
    /// コマンド
    /// </summary>
    public enum Command
    {
        Avoid,
        Guard,
        Attack
    }

    private static Dictionary<Command, float> dict;

    private void Start()
    {
        dict = new Dictionary<Command, float>();

        for(int i = 0; i < System.Enum.GetNames(typeof(Command)).Length; i++)
        {
            dict.Add(((Command)i), 0);
        }
    }

    private void Update()
    {
        for (int i = 0; i < System.Enum.GetNames(typeof(Command)).Length; i++)
        {
            //ボタン押している間
            if (GetButton(((Command)i)))
            {
                //秒数計算
                dict[((Command)i)] += Time.deltaTime;
            }

            //ボタン離したら
            if (GetButtonUp(((Command)i)))
            {
                //秒数リセット
                dict[((Command)i)] = 0;
            }
        }
    }

    public static bool GetButtonDown(Command command)
    {
        if (Input.GetButtonDown(command.ToString()))
            return true;
        else
            return false;
    }

    public static bool GetButton(Command command)
    {
        if (Input.GetButton(command.ToString()))
            return true;
        else
            return false;
    }

    public static bool GetButtonUp(Command command)
    {
        if (Input.GetButtonUp(command.ToString()))
            return true;
        else
            return false;
    }

    /// <summary>
    /// ボタンが一定秒数以上長押し
    /// </summary>
    /// <param name="command"></param>
    /// <param name="pressedTime"></param>
    /// <returns></returns>
    public static bool GetButtonState(Command command, float pressedTime)
    {
        if (dict[command] >= pressedTime)
            return true;
        else
            return false;
    }


}