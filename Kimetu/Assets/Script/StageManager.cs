﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private Transform firstPosition;
    private Vector3 restartPosition;//
    [SerializeField]
    private EnemySpawnerManager manager;
    [SerializeField]
    private bool debugMode = false;
    private static bool resum = false;

    private void Start()
    {
        //データがそんざいしなければ最初の場所から開始
        if (!StageDataPrefs.IsSavedData() && StageManager.resum)
        {
            restartPosition = firstPosition.position;
            StageManager.resum = false;
            return;
        }
        #if UNITY_EDITOR
        if(debugMode) {
            restartPosition = firstPosition.position;
            StageDataPrefs.DeleteCheckPoint();
            return;
        }
        #endif
        //データが存在するならその場所から開始
        restartPosition = StageDataPrefs.GetCheckPosition();
        //プレイヤーの座標を書き換える
        GameObject player = GameObject.FindGameObjectWithTag(TagName.Player.String());
        PlayerAction playerAction = player.GetComponent<PlayerAction>();
        playerAction.StartPosition(restartPosition);

    }

    public void Pass(Vector3 position)
    {
        restartPosition = position;
        //リスタート地点を保存
        StageDataPrefs.SaveCheckPoint(position);
    }
    public Vector3 RestartPosition()
    {
        manager.Init();
        return restartPosition;
    }

    public static void Resume(FadeData fadeData) {
        StageManager.resum = true;
        int currentStageNumber = StageDataPrefs.GetStageNumber();
        string stage = StageNumber.GetStageName(currentStageNumber);
        SceneChanger.Instance().Change(SceneNameManager.GetKeyByValue(stage), fadeData);
    }
}