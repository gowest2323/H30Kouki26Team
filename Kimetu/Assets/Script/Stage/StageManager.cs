using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageManager : MonoBehaviour {
	[SerializeField]
	private Transform first;
	private Vector3 restartPosition;//
	private Quaternion restartRotation;
	[SerializeField]
	private EnemySpawnerManager manager;
	private static bool resum = false;

	private void Start() {
		//データがそんざいしなければ最初の場所から開始
		if (!StageDataPrefs.IsSavedData() || !StageManager.resum) {
			restartPosition = first.position;
			restartRotation = first.rotation;
			return;
		}

		//データが存在するならその場所から開始
		SubstituteSavedCheckPointTransform();
		//プレイヤーの座標を書き換える
		GameObject player = GameObject.FindGameObjectWithTag(TagName.Player.String());
		PlayerAction playerAction = player.GetComponent<PlayerAction>();
		playerAction.StartPositionRotation(restartPosition, restartRotation);
		CameraController camera = GameObject.FindObjectOfType<CameraController>();
		camera.PositionToPlayerBack();
		StageManager.resum = false;

	}

	/// <summary>
	/// チェックポイントを通過した
	/// </summary>
	/// <param name="position">座標</param>
	/// <param name="rotation">角度</param>
	public void Pass(Vector3 position, Quaternion rotation) {
		restartPosition = position;
		restartRotation = rotation;
		//リスタート地点を保存
		StageDataPrefs.SaveCheckPoint(restartPosition, restartRotation);
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

	/// <summary>
	/// 再開場所と角度を取得し代入する
	/// </summary>
	private void SubstituteSavedCheckPointTransform() {
		restartPosition = StageDataPrefs.GetCheckPointPosition();
		restartRotation = StageDataPrefs.GetCheckPointRotation();
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(StageManager))]
public class StageManagerEditor : Editor {
	private StageManager self = null;

	void OnEnable() {
		this.self = (StageManager)target;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.IntField("ステージ番号", StageDataPrefs.GetStageNumber());
		EditorGUILayout.Vector3Field("座標", StageDataPrefs.GetCheckPointPosition());
		EditorGUILayout.Vector3Field("回転", StageDataPrefs.GetCheckPointRotation().eulerAngles);

		if (GUILayout.Button("消去")) {
			StageDataPrefs.DeleteAll();
		}

		EditorGUILayout.EndVertical();
	}
}
#endif