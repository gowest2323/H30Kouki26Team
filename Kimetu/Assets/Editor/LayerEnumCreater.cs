using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class LayerEnumCreater : EditorWindow
{
    private static readonly string CLASSNAME = "LayerName"; //クラス名
    private static readonly string FILEPATH = EnumCreaterSupporter.PATH + CLASSNAME + ".cs"; //ファイルパス

    /// <summary>
    /// 作成
    /// </summary>
    [MenuItem("Editor/Layer")]
    private static void Create()
    {
        //作れなかったらリターン
        if (!CanCreate()) return;
        CreateScripts();
        Debug.Log("LayerEnumの作成に成功しました");
    }

    /// <summary>
    /// スクリプトの作成
    /// </summary>
    private static void CreateScripts()
    {
        //全レイヤーをstringで取得
        string[] layersList = UnityEditorInternal.InternalEditorUtility.layers;
        //文字列結合用
        StringBuilder stringBuilder = EnumCreaterSupporter.CreateSctipt(CLASSNAME, layersList);
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
    [MenuItem("Editor/Layer", true)]
    private static bool CanCreate()
    {
        return EnumCreaterSupporter.CanCreate();
    }
}