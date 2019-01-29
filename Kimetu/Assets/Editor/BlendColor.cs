using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BlendColor {

	[MenuItem("GameObject/BlendColor", false, 20)]
	public static void Execute() {
		//作業用のディレクトリを作る
		CreateDirectoryIfNotExists("Assets/Material");
		CreateDirectoryIfNotExists("Assets/Material/BlendColor");
		var gameObject = Selection.activeGameObject;
		var renderers = gameObject.GetComponentsInChildren<Renderer>();
		DeleteDirectoryIfNotExists("Assets/Material/BlendColor/" + gameObject.name);
		CreateDirectoryIfNotExists("Assets/Material/BlendColor/" + gameObject.name);

		//全てのマテリアルを複製する
		foreach (var renderer in renderers) {
			Debug.Log("renderer: " + renderer.gameObject.name);
			var sharedMaterials = renderer.sharedMaterials;
			var baseName = renderer.gameObject.name;

			foreach (var material in sharedMaterials) {
				Debug.Log("    material: " + material.name);
				var mymat = new Material(Shader.Find("Custom/BlendColor"));
				var color = material.color;
				//mymat.SetFloatArray("_Color", new float[]{color.r, color.g, color.b, 1});
				//こちらだとシェアードマテリアルににならない
				mymat.CopyPropertiesFromMaterial(material);
				AssetDatabase.CreateAsset(mymat, "Assets/Material/BlendColor/" + gameObject.name + "/" + GetSafeFileName(baseName) + "@" + material.name + ".mat");
			}
		}

		AssetDatabase.Refresh();
	}
	private static string GetSafeFileName(string s) {
		return s.Replace(":", "_").Replace(";", "_").Replace("/", "_");
	}
	private static void CreateDirectoryIfNotExists(string path) {
		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}
	}

	//http://kan-kikuchi.hatenablog.com/entry/DirectoryProcessor
	//Assetsディレクトリ以下にあるTestディレクトリを削除
	/// <summary>
	/// 指定したディレクトリとその中身を全て削除する
	/// </summary>
	private static void DeleteDirectoryIfNotExists(string targetDirectoryPath) {
		if (!Directory.Exists (targetDirectoryPath)) {
			return;
		}

		//ディレクトリ以外の全ファイルを削除
		string[] filePaths = Directory.GetFiles(targetDirectoryPath);

		foreach (string filePath in filePaths) {
			File.SetAttributes(filePath, FileAttributes.Normal);
			File.Delete(filePath);
		}

		//ディレクトリの中のディレクトリも再帰的に削除
		string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);

		foreach (string directoryPath in directoryPaths) {
			DeleteDirectoryIfNotExists(directoryPath);
		}

		//中が空になったらディレクトリ自身も削除
		Directory.Delete(targetDirectoryPath, false);
	}
}
