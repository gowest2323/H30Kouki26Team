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
	private PlayerState state;

	//ILongPressInformation
	public string longPressMessage { get { return "次のステージへ";}}
	public bool canLongPress { get { return playerStay && IsPlayerAlive(); }}

	// Use this for initialization
	void Start () {
		this.player = GameObject.FindGameObjectWithTag("Player");
		playerStay = false;
		canvas.gameObject.SetActive(false);
		GetComponent<LongPressDetector>();
		GetComponent<LongPressDetector>().OnLongPressComplete += () => {
			Debug.Log("stay " + playerStay);

			if (playerStay == true) {
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
	void Update() {
		if (playerStay && !IsPlayerAlive()) {
			ActivateCanvas(false);
		}

		if (player.GetComponent<PlayerAction>().state == PlayerState.Damage) {
			GetComponent<LongPressDetector>().Cancel();
			ActivateCanvas(false);
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && IsPlayerAlive()) {
			ActivateCanvas(true);
		}
	}

	public void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			ActivateCanvas(false);
		}
	}

	private void ActivateCanvas(bool active) {
		playerStay = active;
		canvas.gameObject.SetActive(active);
	}

	private bool IsPlayerAlive() {
		return player.GetComponent<Status>().IsAlive();
	}
}
