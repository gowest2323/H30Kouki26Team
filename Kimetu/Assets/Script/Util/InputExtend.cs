using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 181005 何
/// Input拡張クラス
/// </summary>
public class InputExtend : MonoBehaviour {
	/// <summary>
	/// コマンド
	/// </summary>
	public enum Command {
		Avoid,
		Guard,
		Attack
	}

	private static Dictionary<Command, float> dict;
	private static Dictionary<Command, InputMap.Type> typeDict;

	public static bool isCreated = false;//生成されたか？

	private void Awake() {
		//シーン切替を検知
		SceneManager.sceneLoaded += SceneLoaded;

		//1つしか存在しない
		if (!isCreated) {
			DontDestroyOnLoad(this.gameObject);
			isCreated = true;
		} else {
			Destroy(this.gameObject);
		}
	}

	private void Start() {
		dict = new Dictionary<Command, float>();
		typeDict = new Dictionary<Command, InputMap.Type>();

		for (int i = 0; i < System.Enum.GetNames(typeof(Command)).Length; i++) {
			dict.Add(((Command)i), 0);
		}

		typeDict[Command.Attack] = InputMap.Type.XButton;
		typeDict[Command.Avoid] = InputMap.Type.AButton;
		typeDict[Command.Guard] = InputMap.Type.LButton;
	}

	private void Update() {
		for (int i = 0; i < System.Enum.GetNames(typeof(Command)).Length; i++) {
			//ボタン押している間
			if (GetButton(((Command)i))) {
				//秒数計算
				dict[((Command)i)] += Time.deltaTime;
			}

			//ボタン離したら
			if (GetButtonUp(((Command)i))) {
				//秒数リセット
				dict[((Command)i)] = 0;
			}
		}
	}

	public static bool GetButtonDown(Command command) {
		if (Input.GetButtonDown(typeDict[command].GetInputName()))
			return true;
		else
			return false;
	}

	public static bool GetButton(Command command) {
		if (Input.GetButton(typeDict[command].GetInputName()))
			return true;
		else
			return false;
	}

	public static bool GetButtonUp(Command command) {
		if (Input.GetButtonUp(typeDict[command].GetInputName()))
			return true;
		else
			return false;
	}

	/// <summary>
	/// ボタンが一定秒数以上長押し
	/// </summary>
	/// <param name="command"></param>
	/// <param name="pressedTime"></param>
	/// <returns></returns>
	public static bool GetButtonState(Command command, float pressedTime) {
		if (dict[command] >= pressedTime)
			return true;
		else
			return false;
	}

	/// <summary>
	/// リセット
	/// </summary>
	public static void ResetAllButtonState() {
		for (int i = 0; i < System.Enum.GetNames(typeof(Command)).Length; i++) {
			//秒数リセット
			dict[((Command)i)] = 0;
		}
	}

	/// <summary>
	/// シーンが呼ばれた時呼ばれるメソッド
	/// </summary>
	/// <param name="scene"></param>
	/// <param name="mode"></param>
	private void SceneLoaded(Scene scene, LoadSceneMode mode) {
		Debug.Log(scene.name + " Loaded");

		if (dict != null) ResetAllButtonState();
	}
}