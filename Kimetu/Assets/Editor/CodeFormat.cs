using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;

/// <summary>
/// Astyle Format を Unityから利用するためのエディター拡張です。
/// Astyleについて(https://qiita.com/hakuta/items/29c988181d40829b1679)
/// ソースコードのフォーマットを統一するために追加しました。
/// </summary>
public class CodeFormat : EditorWindow {
	private List<string> files;
	private Vector2 scrollPos = Vector2.zero;
	private string searchText = "";
	private string astylePath;

	private static readonly string ASTYLE_PATH_KEY = "Kimetu.AstylePath";
	private string FORMAT_SETTING;

	[MenuItem("Editor/CodeFormat")]
	static void CreateWindow() {
		CodeFormat window = (CodeFormat)EditorWindow.GetWindow(typeof(CodeFormat));
		window.Init();
		window.Show();
	}

	/// <summary>
	/// フォーマット対象となるファイルの一覧を取得します。
	/// </summary>
	private void Init() {
		this.FORMAT_SETTING = Application.dataPath + "/csfmt.txt";
		this.files = new List<string>();
		this.astylePath = PlayerPrefs.GetString(ASTYLE_PATH_KEY, "astyle");
		var assets = Application.dataPath;
		var all = Directory.GetFiles(assets, "*.cs", SearchOption.AllDirectories);
		files = all.OrderBy(f => File.GetLastWriteTime(f))
				.Reverse()
				.ToList();
	}

	/// <summary>
	/// ファイルをフォーマットするためのGUIを描画。
	/// </summary>
	void OnGUI() {
		if (EditorApplication.isCompiling || Application.isPlaying) {
			return;
		}

		if (!File.Exists(FORMAT_SETTING)) {
			EditorGUILayout.LabelField(string.Format("{0}: not found"), FORMAT_SETTING);
			return;
		}

		this.scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		EditorGUILayout.BeginVertical();

		if (GUILayout.Button("Update List")) {
			Init();
			return;
		}

		if (GUILayout.Button("Format All")) {
			FormatAll();
			Close();
			return;
		}

		ShowExecutableFileBar();
		ShowSearchBar();
		ShowFileList();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndScrollView();
	}

	private void FormatAll() {
		bool result = EditorUtility.DisplayDialog(
						  "- CodeFormat -",
						  "全てのファイルをフォーマットします。\nよろしいですか？",
						  "OK",
						  "取消し"
					  );

		if (!result) {
			return;
		}

		files.ForEach((e) => {
			RunFormat(e);
		});
		AssetDatabase.Refresh();
	}

	/// <summary>
	/// 検索バーを描画。
	/// </summary>
	private void ShowSearchBar() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("search:");
		this.searchText = EditorGUILayout.TextField(searchText);
		EditorGUILayout.EndHorizontal();
	}

	/// <summary>
	/// Astyleの実行ファイルへのパスを設定するUI。
	/// </summary>
	private void ShowExecutableFileBar() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("astyle path:");
		var temp = astylePath;
		//値が変更されていたので更新
		this.astylePath = EditorGUILayout.TextField(astylePath);

		if (temp != astylePath) {
			PlayerPrefs.SetString(ASTYLE_PATH_KEY, astylePath);
			PlayerPrefs.Save();
		}

		EditorGUILayout.EndHorizontal();
	}

	/// <summary>
	/// フォーマット対象一覧の表示。
	/// </summary>
	private void ShowFileList() {
		foreach (var filename in files) {
			var pathname = Path.GetFileName(filename);

			if (searchText.Length > 0 && !pathname.Contains(searchText)) {
				continue;
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(pathname);

			if (GUILayout.Button("Format")) {
				RunFormat(filename);
			}

			EditorGUILayout.EndHorizontal();
		}
	}

	private void RunFormat(string filename) {
		try {
			var cmd = string.Format(astylePath + " --options={0} {1}", FORMAT_SETTING, filename);
			UnityEngine.Debug.Log(cmd);
			DoCrossPlatformCommand(cmd);
		} catch (System.Exception e) {
			UnityEngine.Debug.LogError(e);
		}
	}

	static void DoCrossPlatformCommand(string cmd) {
		#if UNITY_STANDALONE_OSX
		DoBashCommand(cmd);
		#else
		DoDOSCommand(cmd);
		#endif
	}

	static void DoBashCommand(string cmd) {
		var p = new Process();
		p.StartInfo.FileName = "/bin/bash";
		p.StartInfo.Arguments = "-c \" " + cmd + " \"";
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.RedirectStandardOutput = true;
		p.StartInfo.RedirectStandardError = true;
		p.Start();
		var output = p.StandardOutput.ReadToEnd();
		var error = p.StandardError.ReadToEnd();
		p.WaitForExit();
		p.Close();
		LogResult(output, error);
	}

	static void DoDOSCommand(string cmd) {
		var p = new Process();
		//p.StartInfo.FileName = "cmd.exe";
		p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
		p.StartInfo.Arguments = "/c \" " + cmd + " \"";
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.RedirectStandardOutput = true;
		p.StartInfo.RedirectStandardError = true;
		p.Start();
		var output = p.StandardOutput.ReadToEnd();
		var error = p.StandardError.ReadToEnd();
		p.WaitForExit();
		p.Close();
		LogResult(output, error);
	}

	private static void LogResult(string output, string error) {
		if (output.Length > 0) {
			UnityEngine.Debug.Log(output);
		}

		if (error.Length > 0) {
			UnityEngine.Debug.LogError(error);
		}
	}
}
