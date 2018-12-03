using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

//http://kan-kikuchi.hatenablog.com/entry/AudioNameCreator
//AudioManagerと合わせて使用します。
//ソースは基本コピペですが、既存のエディタ拡張に合わせて一部の名前を修正しています。

/// <summary>
/// オーディオのファイル名を定数で管理するクラスを作成するスクリプト
/// </summary>
public static class AudioNameCreator {

	private const string COMMAND_NAME  = "Editor/Audio";        // コマンド名
	private const string EXPORT_PATH   = "Assets/Script/Define/AudioName.cs"; //作成したスクリプトを保存するパス

	// ファイル名(拡張子あり、なし)
	private static readonly string FILENAME = Path.GetFileName(EXPORT_PATH);
	private static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(EXPORT_PATH);

	/// <summary>
	/// オーディオのファイル名を定数で管理するクラスを作成します
	/// </summary>
	[MenuItem(COMMAND_NAME)]
	public static void Create() {
		if (!CanCreate()) {
			return;
		}

		CreateScript();

		EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");
	}

	/// <summary>
	/// スクリプトを作成します
	/// </summary>
	public static void CreateScript() {
		var nameList = new List<string>();
		//指定したパスのリソースを全て取得
		nameList.AddRange(Resources.LoadAll("Audio/BGM").ToList().Select((e) => {
			AudioClip clip = e as AudioClip;
			return string.Format("{0}", clip.name);
		}));
		nameList.AddRange(Resources.LoadAll("Audio/SE").ToList().Select((e) => {
			AudioClip clip = e as AudioClip;
			return string.Format("{0}", clip.name);
		}));
		//文字列化
		var builder = EnumCreaterSupporter.CreateSctipt("AudioName", nameList.ToArray());

		string directoryName = Path.GetDirectoryName(EXPORT_PATH);

		if (!Directory.Exists(directoryName)) {
			Directory.CreateDirectory(directoryName);
		}

		File.WriteAllText(EXPORT_PATH, builder.ToString(), Encoding.UTF8);
		AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
	}

	/// <summary>
	/// オーディオのファイル名を定数で管理するクラスを作成できるかどうかを取得します
	/// </summary>
	[MenuItem(COMMAND_NAME, true)]
	private static bool CanCreate() {
		return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
	}

}