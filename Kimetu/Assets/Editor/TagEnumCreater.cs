using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System;

public class TagEnumCreater : EditorWindow
{
    private static readonly string CLASSNAME = "TagName"; //クラス名
    private static readonly string FILEPATH = EnumCreaterSupporter.PATH + CLASSNAME + ".cs"; //ファイルパス

    /// <summary>
    /// 作成
    /// </summary>
    [MenuItem("Editor/Tag")]
    private static void Create()
    {
        //作れなかったらリターン
        if (!CanCreate()) return;
        CreateScripts();
        Debug.Log("TagEnumの作成に成功しました");
    }

    /// <summary>
    /// スクリプトの作成
    /// </summary>
    private static void CreateScripts()
    {
        //全タグをstringで取得
        string[] tagsArray = UnityEditorInternal.InternalEditorUtility.tags;
        //スクリプトを生成しstringBuilderにいれる
        StringBuilder stringBuilder = EnumCreaterSupporter.CreateSctipt(CLASSNAME, tagsArray);
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
    [MenuItem("Editor/Tag", true)]
    private static bool CanCreate()
    {
        return EnumCreaterSupporter.CanCreate();
    }
}