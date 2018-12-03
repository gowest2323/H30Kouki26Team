using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageManager : MonoBehaviour {
	[SerializeField]
	private Transform firstPosition;
	private Vector3 restartPosition;//
	[SerializeField]
	private EnemySpawnerManager manager;
	private static bool resum = false;

	private void Start() {
		//データがそんざいしなければ最初の場所から開始
		if (!StageDataPrefs.IsSavedData() || !StageManager.resum) {
			restartPosition = firstPosition.position;
			return;
		}

		//データが存在するならその場所から開始
		restartPosition = StageDataPrefs.GetCheckPosition();
		//プレイヤーの座標を書き換える
		GameObject player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		PlayerAction playerAction = player.GetComponent<PlayerAction>();
		playerAction.StartPosition(restartPosition);
		StageManager.resum = false;

	}

	public void Pass(Vector3 position) {
		restartPosition = position;
		//リスタート地点を保存
		StageDataPrefs.SaveCheckPoint(position);
		//現在のシーン番号を保存
		string currentScene = SceneManager.GetActiveScene().name;
		int currentStageNumber = StageNumber.GetStageNumber(currentScene);
		StageDataPrefs.SaveStageNumber(currentStageNumber);
	}
	public Vector3 RestartPosition() {
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

#if UNITY_EDITOR
[CustomEditor (typeof(StageManager))]
public class StageManagerEditor : Editor　 {
	private StageManager self = null;

	void OnEnable () {
		this.self = (StageManager) target;
	}

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.IntField("ステージ番号", StageDataPrefs.GetStageNumber());
		EditorGUILayout.Vector3Field("座標", StageDataPrefs.GetCheckPosition());

		if (GUILayout.Button("消去")) {
			StageDataPrefs.DeleteAll();
		}

		EditorGUILayout.EndVertical();
	}
}
#endif