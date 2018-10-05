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
    /// <param name="scene">次のシーン</param>
    public void Change(SceneName scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.String());
    }

    /// <summary>
    /// シーンの変更
    /// </summary>
    /// <param name="scene">次のシーン</param>
    /// <param name="fade">フェードの情報</param>
    public void Change(SceneName scene, FadeData fade)
    {
        StartCoroutine(ChangeCoroutine(scene, fade));
    }

    /// <summary>
    /// シーンを変更するコルーチン
    /// </summary>
    /// <param name="scene">次のシーン</param>
    /// <param name="fade">フェードの情報</param>
    /// <returns></returns>
    private IEnumerator ChangeCoroutine(SceneName scene, FadeData fade)
    {
        //フェードアウトし終わるまで待機
        yield return Fade.Instance().FadeOutCoroutine(fade.fadeOutTime, fade.fadeColor);
        //シーンを変更する
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.String());
        //フェードイン処理
        Fade.Instance().FadeIn(fade.fadeInTime, fade.fadeColor);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private static void Init()
    {
        //SceneChangerオブジェクトを生成
        GameObject go = new GameObject("SceneChanger");
        instance = go.AddComponent<SceneChanger>();
        //シーン変更時に破壊されないようにする
        DontDestroyOnLoad(go);
    }
}
