using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum SceneName
{
GamePlay,
}
public static class SceneNameManager
{
    public static Dictionary<SceneName, string> scenenames = new Dictionary<SceneName, string> 
{
    {SceneName.GamePlay,"GamePlay"},
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
