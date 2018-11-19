using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System;
using System.Linq;

public class SceneEnumCreater : EditorWindow
{
    private static readonly string CLASSNAME = "SceneName"; //クラス名
    private static readonly string FILEPATH = EnumCreaterSupporter.PATH + CLASSNAME + ".cs"; //ファイルパス

    /// <summary>
    /// 作成
    /// </summary>
    [MenuItem("Editor/Scene")]
    private static void Create()
    {
        //作れなかったらリターン
        if (!CanCreate()) return;
        CreateScripts();
        Debug.Log("SceneEnumの作成に成功しました");
    }

    /// <summary>
    /// スクリプトの作成
    /// </summary>
    private static void CreateScripts()
    {
        //全シーンをstringで取得
        string[] scenesList = EditorBuildSettings.scenes
             .Select(scene => Path.GetFileNameWithoutExtension(scene.path))
             .Distinct()
             .ToArray();
        //文字列結合用
        StringBuilder stringBuilder = EnumCreaterSupporter.CreateSctipt(CLASSNAME, scenesList);

        //ディレクトリのパスを取得し、そこにファイルがなければ新しく作成
        string directoryName = Path.GetDirectoryName(FILEPATH);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
        //書き込み
        File.WriteAllText(FILEPATH, stringBuilder.ToString(), Encoding.UTF8);
    }

    /// <summary>
    /// 作成できるか
    /// </summary>
    /// <returns></returns>
    [MenuItem("Editor/Scene", true)]
    private static bool CanCreate()
    {
        return EnumCreaterSupporter.CanCreate();
    }
}