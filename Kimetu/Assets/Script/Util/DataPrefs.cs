using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageDataPrefs {
	private static readonly string StageNumber = "STAGE_NUM";
	private static readonly string Position_X = "POSITION_X";
	private static readonly string Position_Y = "POSITION_Y";
	private static readonly string Position_Z = "POSITION_Z";
	private static readonly string Rotation_X = "ROTATION_X";
	private static readonly string Rotation_Y = "ROTATION_Y";
	private static readonly string Rotation_Z = "ROTATION_Z";

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
	public static void SaveCheckPoint(Vector3 position, Quaternion rotation) {
		PlayerPrefs.SetFloat(Position_X, position.x);
		PlayerPrefs.SetFloat(Position_Y, position.y);
		PlayerPrefs.SetFloat(Position_Z, position.z);
		PlayerPrefs.SetFloat(Rotation_X, rotation.eulerAngles.x);
		PlayerPrefs.SetFloat(Rotation_Y, rotation.eulerAngles.y);
		PlayerPrefs.SetFloat(Rotation_Z, rotation.eulerAngles.z);
		PlayerPrefs.Save();
	}

	/// <summary>
	/// 保存されたチェックポイントの場所の取得
	/// </summary>
	/// <returns></returns>
	public static Vector3 GetCheckPointPosition() {
		float x = PlayerPrefs.GetFloat(Position_X);
		float y = PlayerPrefs.GetFloat(Position_Y);
		float z = PlayerPrefs.GetFloat(Position_Z);
		Vector3 position = new Vector3(x, y, z);
		return position;
	}

	/// <summary>
	/// 保存された角度の取得
	/// </summary>
	/// <returns></returns>
	public static Quaternion GetCheckPointRotation() {
		float rx = PlayerPrefs.GetFloat(Rotation_X);
		float ry = PlayerPrefs.GetFloat(Rotation_Y);
		float rz = PlayerPrefs.GetFloat(Rotation_Z);
		Quaternion rotate = Quaternion.Euler(rx, ry, rz);
		return rotate;
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
		PlayerPrefs.DeleteKey(Rotation_X);
		PlayerPrefs.DeleteKey(Rotation_Y);
		PlayerPrefs.DeleteKey(Rotation_Z);
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
