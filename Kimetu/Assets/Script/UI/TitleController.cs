using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour {
	[SerializeField]
	private MenuUI menu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!Input.GetButton(InputMap.Type.AButton.GetInputName())) {
			return;
		}
		var selected = menu.selected;
		var fadeData = new FadeData(1f, 1f, Color.black);
		//始める
		if(selected == 0) {
            //データの削除
            StageDataPrefs.DeleteAll();
			SceneChanger.Instance().Change(SceneName.Stage01, fadeData);
		//途中から
		} else if(selected == 1) {
            Resume(fadeData);
            //SceneChanger.Instance().Change(SceneName.Stage01, fadeData);
            //GameRegistry.instance.Load();
            //操作説明
        }
        else if(selected == 2) {
			SceneChanger.Instance().Change(SceneName.Control, fadeData);
		//オプション
		} else if(selected == 3) {
			SceneChanger.Instance().Change(SceneName.Option, fadeData);
		//クレジット
		} else if(selected == 4) {
			SceneChanger.Instance().Change(SceneName.Credit, fadeData);
		//終了
		} else if(selected == 5) {
			Application.Quit();
		}
	}

    private void Resume(FadeData fadeData)
    {
        StageManager.Resume(fadeData);
    }
}
