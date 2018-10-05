using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger instance;

    [RuntimeInitializeOnLoadMethod()]
    public static SceneChanger Instance()
    {
        if (!instance)
        {
            Init();
        }
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //すでにインスタンスがあったら破棄する。
            Destroy(this);
        }
    }

    /// <summary>
    /// 現在のシーンを取得
    /// </summary>
    /// <returns></returns>
    private SceneName GetCurrentScene()
    {
        return SceneNameManager.GetKeyByValue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// シーンの変更
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="fade"></param>
    public void Change(SceneName scene, FadeData fade)
    {
        StartCoroutine(ChangeCoroutine(scene, fade));
    }

    private IEnumerator ChangeCoroutine(SceneName scene, FadeData fade)
    {
        yield return Fade.Instance().FadeOutCoroutine(fade.fadeOutTime, fade.fadeColor);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.String());
        Fade.Instance().FadeIn(fade.fadeInTime, fade.fadeColor);
    }

    private static void Init()
    {
        //SceneChangerオブジェクトを生成
        GameObject go = new GameObject("SceneChanger");
        instance = go.AddComponent<SceneChanger>();
        //シーン変更時に破壊されないようにする
        DontDestroyOnLoad(go);
    }
}
