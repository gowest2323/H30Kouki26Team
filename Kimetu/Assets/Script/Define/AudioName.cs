using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum AudioName
{
SE_CLUB,
SE_CUT,
SE_DASH,
SE_DODGE,
SE_DRAWING,
SE_HIPDROP,
SE_LANCE,
SE_LANCE2,
SE_REPEL,
SE_TACKLE,
SE_WALK,
}
public static class AudioNameManager
{
    public static Dictionary<AudioName, string> audionames = new Dictionary<AudioName, string> 
{
    {AudioName.SE_CLUB,"Club_Short"},
    {AudioName.SE_CUT,"Cut"},
    {AudioName.SE_DASH,"Dash_Short"},
    {AudioName.SE_DODGE,"Dodge_Short"},
    {AudioName.SE_DRAWING,"SE_DRAWING"},
    {AudioName.SE_HIPDROP,"SE_HIPDROP"},
    {AudioName.SE_LANCE,"SE_LANCE"},
    {AudioName.SE_LANCE2,"SE_LANCE2"},
    {AudioName.SE_REPEL,"SE_REPEL"},
    {AudioName.SE_TACKLE,"SE_TACKLE"},
    {AudioName.SE_WALK,"Walk_Short"},
};
    public static bool Equals(AudioName audioname, string name)
    {
        return audionames[audioname] == name;
    }
    public static bool Equals(string name, AudioName audioname)
    {
        return name == audionames[audioname];
    }
    public static bool Equals(string name1, string name2)
    {
        return name1 == name2;
    }
    public static bool Equals(AudioName audioname1, AudioName audioname2)
    {
        return audioname1 == audioname2;
    }
    public static string String(this AudioName audioname)
    {
        return audionames[audioname];
    }
    public static AudioName GetKeyByValue(string name)
    {
        return audionames.FirstOrDefault(pair => pair.Value == name).Key;
    }
}
