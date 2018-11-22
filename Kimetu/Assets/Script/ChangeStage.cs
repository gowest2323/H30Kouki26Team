using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChangeStage : MonoBehaviour, ILongPressInformation {
    private bool playerStay;
    public GameObject canvas;
    [SerializeField, Header("再生するタイムライン")]
    private PlayableDirector playableDirector;
    [SerializeField]
    private SceneName nextSceneName;

    private GameObject player;

    //ILongPressInformation
    public string longPressMessage { get { return "次のステージへ";}}
    public bool canLongPress { get { return playerStay && player.GetComponent<Status>().IsAlive(); }}

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player");
        playerStay = false;
        canvas.gameObject.SetActive(false);
        GetComponent<LongPressDetector>();
        GetComponent<LongPressDetector>().OnLongPressComplete += () => {
            Debug.Log("stay " + playerStay);
            if (playerStay == true)
            {
                //現在のステージ番号を保存
                string currentScene = SceneManager.GetActiveScene().name;
                int currentStageNumber = StageNumber.GetStageNumber(currentScene);
                StageDataPrefs.SaveStageNumber(++currentStageNumber);
                //チェックポイントのデータの削除
                StageDataPrefs.DeleteCheckPoint();
                SceneChanger.Instance().Change(nextSceneName, new FadeData(1, 1, Color.black));
            }
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerStay = true;
            canvas.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerStay = false;
            canvas.gameObject.SetActive(false);
        }
    }
}
