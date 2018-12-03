using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageDataPrefs {
	private static readonly string StageNumber = "STAGE_NUM";
	private static readonly string Position_X = "POSITION_X";
	private static readonly string Position_Y = "POSITION_Y";
	private static readonly string Position_Z = "POSITION_Z";

	/// <summary>
	/// ステージ番号の保存
	/// </summary>
	/// <param name="stageNumber"></param>
	public static void SaveStageNumber(int stageNumber) {
		PlayerPrefs.SetInt(StageNumber, stageNumber);
		PlayerPrefs.Save();
	}

	/// <summary>
	/// 再開場所の保存
	/// </summary>
	/// <param name="position"></param>
	public static void SaveCheckPoint(Vector3 position) {
		PlayerPrefs.SetFloat(Position_X, position.x);
		PlayerPrefs.SetFloat(Position_Y, position.y);
		PlayerPrefs.SetFloat(Position_Z, position.z);
		PlayerPrefs.Save();
	}

	/// <summary>
	/// 保存されたチェックポイントの場所の取得
	/// </summary>
	/// <returns></returns>
	public static Vector3 GetCheckPosition() {
		float x = PlayerPrefs.GetFloat(Position_X);
		float y = PlayerPrefs.GetFloat(Position_Y);
		float z = PlayerPrefs.GetFloat(Position_Z);
		Vector3 position = new Vector3(x, y, z);
		return position;
	}

	/// <summary>
	/// 保存されたステージ番号の取得
	/// </summary>
	/// <returns></returns>
	public static int GetStageNumber() {
		int stageNumber = PlayerPrefs.GetInt(StageNumber, 0);
		return stageNumber;
	}

	/// <summary>
	/// チェックポイント情報の削除
	/// </summary>
	public static void DeleteCheckPoint() {
		PlayerPrefs.DeleteKey(Position_X);
		PlayerPrefs.DeleteKey(Position_Y);
		PlayerPrefs.DeleteKey(Position_Z);
		PlayerPrefs.Save();
	}

	/// <summary>
	/// 全情報を削除
	/// </summary>
	public static void DeleteAll() {
		PlayerPrefs.DeleteKey(StageNumber);
		DeleteCheckPoint();
		//PlayerPrefs.DeleteAll();
	}

	/// <summary>
	/// データが保存されているか
	/// </summary>
	/// <returns></returns>
	public static bool IsSavedData() {
		return PlayerPrefs.HasKey(Position_X);
	}
}
