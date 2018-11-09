using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct FadeData
{
    public float fadeInTime; //フェードアウト時間
    public float fadeOutTime; //フェードイン時間
    public Color fadeColor; //フェード色

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="fadeOutTime">フェードアウト時間</param>
    /// <param name="fadeInTime">フェードイン時間</param>
    /// <param name="color">フェード色</param>
    public FadeData(float fadeOutTime, float fadeInTime, Color color)
    {
        this.fadeOutTime = fadeOutTime;
        this.fadeInTime = fadeInTime;
        this.fadeColor = color;
    }
}

public class Fade : MonoBehaviour
{
    private Image image; //イメージオブジェクト

    private static Fade instance; //インスタンス

    [RuntimeInitializeOnLoadMethod()]
    public static Fade Instance()
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
        //シーン変更時に呼ばれるメソッドを設定
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private static void Init()
    {
        //Fadeオブジェクトを生成
        GameObject go = new GameObject("Fade");
        instance = go.AddComponent<Fade>();
        //シーン変更時に破壊されないようにする
        DontDestroyOnLoad(go);
        //キャンバスの追加
        Canvas canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //フェードの初期設定
        Image image = go.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// シーン変更時に呼ばれる
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void OnSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
    {
    }

    /// <summary>
    /// フェードインするコルーチン
    /// </summary>
    /// <param name="second">かける秒数</param>
    /// <param name="startColor">開始時のフェードの色</param>
    /// <returns></returns>
    public IEnumerator FadeInCoroutine(float second, Color startColor)
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        image.color = new Color(0, 0, 0, 0);

        float time = 0.0f;
        while (time < second)
        {
            image.color = startColor * (1.0f - time / second);
            time += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// フェードアウトするコルーチン
    /// </summary>
    /// <param name="second">かける秒数</param>
    /// <param name="endColor">フェード終了時の色</param>
    /// <returns></returns>
    public IEnumerator FadeOutCoroutine(float second, Color endColor)
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        float time = 0.0f;
        while (time < second)
        {
            image.color = endColor * (time / second);
            time += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    /// <param name="second">かける秒数</param>
    /// <param name="startColor">開始時のフェードの色</param>
    public void FadeIn(float second, Color startColor)
    {
        StartCoroutine(FadeInCoroutine(second, startColor));
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="second">かける秒数</param>
    /// <param name="endColor">フェード終了時の色</param>
    public void FadeOut(float second, Color endColor)
    {
        StartCoroutine(FadeOutCoroutine(second, endColor));
    }
}