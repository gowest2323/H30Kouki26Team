using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StageNumber : MonoBehaviour {
	/// <summary>
	/// ステージの名前と番号
	/// </summary>
	private struct StageNameNumber {
		public string name;
		public int number;
		public StageNameNumber(string name, int number) {
			this.name = name;
			this.number = number;
		}
	}

	private static List<StageNameNumber> stageNumbers =
	new List<StageNameNumber> {
		new StageNameNumber("Stage01", 0 ),
		new StageNameNumber("Stage01_Boss", 1 ),
		new StageNameNumber("Stage02", 2 ),
		new StageNameNumber("Stage02_Boss", 3  ),
		new StageNameNumber("Stage03", 4  ),
		new StageNameNumber("Stage03_Boss", 5  ),
	};

	/// <summary>
	/// ステージ番号の取得
	/// </summary>
	/// <param name="stageName"></param>
	/// <returns></returns>
	public static int GetStageNumber(string stageName) {
		foreach (var pair in stageNumbers) {
			if (pair.name == stageName) {
				return pair.number;
			}
		}

		Debug.LogError("登録されていないステージが呼ばれました。");
		return 0;
	}

	/// <summary>
	/// ステージ名の取得
	/// </summary>
	/// <param name="stageNumber"></param>
	/// <returns></returns>
	public static string GetStageName(int stageNumber) {
		foreach (var pair in stageNumbers) {
			if (pair.number == stageNumber) {
				return pair.name;
			}
		}

		Debug.LogError("登録されていないステージが呼ばれました。");
		return stageNumbers[0].name;
	}
}
