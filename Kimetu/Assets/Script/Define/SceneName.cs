using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum SceneName
{
Title,
Stage01,
Stage01_Boss,
Stage02,
Stage02_Boss,
Stage03,
Stage03_Boss,
Pause,
Control,
Credit,
Option,
OpeningMovie,
LastBossMovie,
}
public static class SceneNameManager
{
    public static Dictionary<SceneName, string> scenenames = new Dictionary<SceneName, string> 
{
    {SceneName.Title,"Title"},
    {SceneName.Stage01,"Stage01"},
    {SceneName.Stage01_Boss,"Stage01_Boss"},
    {SceneName.Stage02,"Stage02"},
    {SceneName.Stage02_Boss,"Stage02_Boss"},
    {SceneName.Stage03,"Stage03"},
    {SceneName.Stage03_Boss,"Stage03_Boss"},
    {SceneName.Pause,"Pause"},
    {SceneName.Control,"Control"},
    {SceneName.Credit,"Credit"},
    {SceneName.Option,"Option"},
    {SceneName.OpeningMovie,"OpeningMovie"},
    {SceneName.LastBossMovie,"LastBossMovie"},
};

	public static Dictionary<SceneName, string> sceneUInames = new Dictionary<SceneName, string> {
		{SceneName.Stage01, "一階"},
		{SceneName.Stage01_Boss, "一階　ボス"},
		{SceneName.Stage02, "二階"},
		{SceneName.Stage02_Boss, "二階　ボス"},
		{SceneName.Stage03, "三階"},
		{SceneName.Stage03_Boss, "三階　ボス"},
	};

	public static bool Equals(SceneName scenename, string name)
    {
        return scenenames[scenename] == name;
    }
    public static bool Equals(string name, SceneName scenename)
    {
        return name == scenenames[scenename];
    }
    public static bool Equals(string name1, string name2)
    {
        return name1 == name2;
    }
    public static bool Equals(SceneName scenename1, SceneName scenename2)
    {
        return scenename1 == scenename2;
    }
    public static string String(this SceneName scenename)
    {
        return scenenames[scenename];
    }
    public static SceneName GetKeyByValue(string name)
    {
        return scenenames.FirstOrDefault(pair => pair.Value == name).Key;
    }
}
