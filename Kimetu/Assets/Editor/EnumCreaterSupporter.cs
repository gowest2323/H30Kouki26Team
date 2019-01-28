using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class EnumCreaterSupporter {
	public static readonly string PATH = "Assets/Script/Define/"; //格納するパス

	// 無効な文字を管理する配列
	private static readonly string[] INVALUD_CHARS = {
		" ", "!", "\"", "#", "$",
		"%", "&", "\'", "(", ")",
		"-", "=", "^",  "~", "\\",
		"|", "[", "{",  "@", "`",
		"]", "}", ":",  "*", ";",
		"+", "/", "?",  ".", ">",
		",", "<",
	};

	/// <summary>
	/// 無効な文字の削除
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static string RemoveInvalidChars(string name) {
		//無効文字が含まれていたら削除する
		Array.ForEach(INVALUD_CHARS, c => name = name.Replace(c, string.Empty));
		return name;
	}

	public static bool CanCreate() {
		//デバッグ実行中ならfalse
		if (EditorApplication.isPlaying) return false;

		//アプリケーションが実行中ならfalse
		if (Application.isPlaying) return false;

		//コンパイル中ならfalse
		if (EditorApplication.isCompiling) return false;

		return true;
	}

	public static StringBuilder CreateSctipt(string className, string[] nameArray) {
		string filepath = PATH + className + ".cs";

		//0個だったら終了
		if (nameArray.Length == 0) {
			EditorUtility.DisplayDialog(filepath, className + "の中身が一つも存在しません", "OK");
			return null;
		}

		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("using System.Collections.Generic;");
		stringBuilder.AppendLine("using System.Linq;");
		//サマリの作成
		stringBuilder.AppendLine("/// <summary>");
		stringBuilder.AppendLine("/// タグのenum");
		stringBuilder.AppendLine("/// </summary>");
		//クラス名をつける
		stringBuilder.AppendFormat("public enum {0}", className).AppendLine();
		stringBuilder.AppendLine("{");

		//各要素をカンマ区切りで格納
		for (int i = 0; i < nameArray.Length; i++) {
			nameArray[i] = RemoveInvalidChars(nameArray[i]);
			stringBuilder.AppendLine(nameArray[i] + ",");
		}

		//enumの終わり
		stringBuilder.AppendLine("}");

		//classのManagerを作成
		stringBuilder.AppendFormat("public static class {0}Manager", className).AppendLine();
		stringBuilder.AppendLine("{");
		string classNameLower = className.ToLower();
		string dictionaryName = className.ToLower() + "s";
		//enumとstringのペアのディクショナリを作成
		stringBuilder.AppendFormat("    public static Dictionary<{0}, string> {1}s = new Dictionary<{0}, string> ", className, classNameLower).AppendLine();
		stringBuilder.AppendLine("{");

		foreach (var tag in nameArray) {
			stringBuilder.AppendLine("    {" + className + "." + tag + "," + "\"" + tag + "\"},");
		}

		stringBuilder.AppendLine("};");

		stringBuilder.AppendFormat("    public static bool Equals({0} {1}, string name)", className, classNameLower).AppendLine();
		stringBuilder.AppendLine("    {");
		stringBuilder.AppendFormat("        return {0}[{1}] == name;", dictionaryName, classNameLower).AppendLine();
		stringBuilder.AppendLine("    }");

		stringBuilder.AppendFormat("    public static bool Equals(string name, {0} {1})", className, classNameLower).AppendLine();
		stringBuilder.AppendLine("    {");
		stringBuilder.AppendFormat("        return name == {0}[{1}];", dictionaryName, classNameLower).AppendLine();
		stringBuilder.AppendLine("    }");

		stringBuilder.AppendLine("    public static bool Equals(string name1, string name2)");
		stringBuilder.AppendLine("    {");
		stringBuilder.AppendLine("        return name1 == name2;");
		stringBuilder.AppendLine("    }");

		stringBuilder.AppendFormat("    public static bool Equals({0} {1}1, {0} {1}2)", className, classNameLower).AppendLine();
		stringBuilder.AppendLine("    {");
		stringBuilder.AppendFormat("        return {0}1 == {0}2;", classNameLower).AppendLine();
		stringBuilder.AppendLine("    }");

		stringBuilder.AppendFormat("    public static string String(this {0} {1})", className, classNameLower).AppendLine();
		stringBuilder.AppendLine("    {");
		stringBuilder.AppendFormat("        return {0}[{1}];", dictionaryName, classNameLower).AppendLine();
		stringBuilder.AppendLine("    }");

		stringBuilder.AppendFormat("    public static {0} GetKeyByValue(string name)", className).AppendLine();
		stringBuilder.AppendLine("    {");
		stringBuilder.AppendFormat("        return {0}.FirstOrDefault(pair => pair.Value == name).Key;", dictionaryName).AppendLine();
		stringBuilder.AppendLine("    }");

		stringBuilder.AppendLine("}");
		return stringBuilder;
	}
}
