using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScene : MonoBehaviour
{
    [SerializeField]
    private FadeData changeTitleFade;
    private PauseManager pauseManager;

    private void Start()
    {
        pauseManager = GameObject.FindObjectOfType<PauseManager>();
    }

    /// <summary>
    /// ゲームに戻る
    /// </summary>
    public void ReturnGame()
    {
        pauseManager.Resume();
    }

    /// <summary>
    /// 操作説明表示
    /// </summary>
    public void PrintOperationDescription()
    {

    }

    /// <summary>
    /// タイトルへ戻る
    /// </summary>
    public void ToTitle()
    {
        //シーンの切り替えにコルーチンを利用しているためtimescaleを戻す
        Time.timeScale = 1.0f;
        SceneChanger.Instance().Change(SceneName.Title, changeTitleFade);
    }
}
