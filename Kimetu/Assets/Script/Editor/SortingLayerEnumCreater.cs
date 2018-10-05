using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SortingLayerEnumCreater : EditorWindow
{
    private static readonly string CLASSNAME = "SortingLayerName"; //クラス名
    private static readonly string FILEPATH = EnumCreaterSupporter.PATH + CLASSNAME + ".cs"; //ファイルパス

    /// <summary>
    /// 作成
    /// </summary>
    [MenuItem("Editor/SortingLayer")]
    private static void Create()
    {
        //作れなかったらリターン
        if (!CanCreate()) return;
        CreateScripts();
        Debug.Log("SortingLayerEnumの作成に成功しました");
    }

    /// <summary>
    /// スクリプトの作成
    /// </summary>
    private static void CreateScripts()
    {
        //全Layerをstringで取得
        var sortingLayers = SortingLayer.layers;

        string[] tagsArray = new string[sortingLayers.Length];
        for (int i = 0; i < tagsArray.Length; i++)
        {
            tagsArray[i] = sortingLayers[i].name;
        }
        //文字列結合用
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
    [MenuItem("Editor/SortingLayer", true)]
    private static bool CanCreate()
    {
        return EnumCreaterSupporter.CanCreate();
    }
}
