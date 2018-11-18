using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class ButtonEnumCreater : EditorWindow
{
    private static readonly string CLASSNAME = "ButtonName"; //クラス名
    private static readonly string FILEPATH = EnumCreaterSupporter.PATH + CLASSNAME + ".cs"; //ファイルパス

    /// <summary>
    /// 作成
    /// </summary>
    [MenuItem("Editor/Button")]
    private static void Create()
    {
        //作れなかったらリターン
        if (!CanCreate()) return;
        CreateScripts();
        Debug.Log("ButtonEnumの作成に成功しました");
    }

    /// <summary>
    /// スクリプトの作成
    /// </summary>
    private static void CreateScripts()
    {
        string[] buttonList = GetButtonString();
        //文字列結合用
        StringBuilder stringBuilder = EnumCreaterSupporter.CreateSctipt(CLASSNAME, buttonList);
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
    [MenuItem("Editor/Button", true)]
    private static bool CanCreate()
    {
        return EnumCreaterSupporter.CanCreate();
    }

    /// <summary>
    /// InputManagerに登録されたボタンの名前を取得
    /// </summary>
    /// <returns>重複を削除されたリストを返す</returns>
    private static string[] GetButtonString()
    {
        List<string> buttonStringList = new List<string>();
        //ProjectSettingにあるInputManagerをシリアライズオブジェクトとして開く
        SerializedObject buttonObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        //ボタンの定義部分のプロパティを取得
        SerializedProperty buttonProperty = buttonObject.FindProperty("m_Axes");
        for (int i = 0; i < buttonProperty.arraySize; i++)
        {
            //一つずづ名前を取得
            SerializedProperty prop = buttonProperty.GetArrayElementAtIndex(i);
            buttonStringList.Add(GetChildProperty(prop, "m_Name").stringValue);
        }
        //重複を削除し返す
        return buttonStringList.Distinct().ToArray();
    }

    /// <summary>
    /// 子プロパティを取得
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        } while (child.Next(false));
        return null;
    }
}
