using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger instance;
    public bool isChanging { private set; get; }
    private Coroutine currentCoroutine;

    private GameObject loadCanvas;

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
        SceneManager.LoadScene(scene.String());
    }

    /// <summary>
    /// シーンの変更
    /// </summary>
    /// <param name="scene">次のシーン</param>
    /// <param name="fade">フェードの情報</param>
    public void Change(SceneName scene, FadeData fade)
    {
        currentCoroutine = StartCoroutine(ChangeCoroutine(scene, fade));
    }

    /// <summary>
    /// シーンを変更するコルーチン
    /// </summary>
    /// <param name="scene">次のシーン</param>
    /// <param name="fade">フェードの情報</param>
    /// <returns></returns>
    private IEnumerator ChangeCoroutine(SceneName scene, FadeData fade)
    {
        if (isChanging && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        isChanging = true;
        //フェードアウトし終わるまで待機
        yield return StartCoroutine(Fade.Instance().FadeOutCoroutine(fade.fadeOutTime, fade.fadeColor));
        //シーンを変更する
        yield return StartCoroutine(LoadData(scene.String()));
        //SceneManager.LoadScene(scene.String());
        //TODO：ステージの名前

        //フェードイン処理
        yield return StartCoroutine(Fade.Instance().FadeInCoroutine(fade.fadeInTime, fade.fadeColor));
        isChanging = false;
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

    /// <summary>
    /// 非同期ロード
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadData(string sceneName)
    {
        //ロードキャンパス生成
        loadCanvas = Instantiate((GameObject)Resources.Load("Prefab/LoadCanvas"));
        yield return new WaitForSeconds(0.5f);

        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;// シーン遷移をしない

        Image loadImage = loadCanvas.GetComponentInChildren<Image>();
        Text loadText = loadCanvas.GetComponentInChildren<Text>();

        //　読み込みが終わるまで進捗状況を値に反映させる
        while (async.progress < 0.9f)
        {
            //ロード進展
            loadImage.rectTransform.Rotate(new Vector3(0, 0, 30));

            loadText.text = (async.progress * 100).ToString("F0") + "%";
            yield return new WaitForEndOfFrame();
        }

        loadText.text = "100%";
        yield return new WaitForSeconds(0.5f);

        async.allowSceneActivation = true;// シーン遷移許可
    }
}
