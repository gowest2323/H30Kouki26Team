using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadMenuUI : MonoBehaviour
{
    [SerializeField]
    private MenuUI menu;
    [SerializeField]
    private StageManager manager;
    [SerializeField]
    private GameObject player;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetButton(InputMap.Type.AButton.GetInputName()))
        {
            return;
        }
        var selected = menu.selected;
        var fadeData = new FadeData(1f, 1f, Color.black);

        if (selected == 0)
        {//最後のチェックポイントから
            SceneChanger.Instance().Change(SceneName.Stage01, fadeData);
            player.transform.position = manager.transform.position;
        }
        else if (selected == 1)
        {//タイトルに戻る
            SceneChanger.Instance().Change(SceneName.Title, fadeData);
        }
        else if (selected == 2)
        {//ゲーム終了
            Application.Quit();
        }

    }
}
