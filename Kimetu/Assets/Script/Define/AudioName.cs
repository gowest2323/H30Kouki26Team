using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum AudioName
{
SE_BOUGYOKAMAE,
SE_BOUGYOUKE,
SE_CLUB,
SE_CLUB_SHORT,
SE_CUT,
SE_DASH,
SE_DASH_SHORT,
SE_DODGE,
SE_DODGE_SHORT,
SE_DRAWING,
SE_HAJIKI,
SE_HIPDROP,
SE_KAIHI,
SE_KOUGEKI,
SE_KYUSEI,
SE_LANCE,
SE_LANCE2,
SE_REPEL,
SE_TACKLE,
SE_WALK,
SE_WALK_SHORT,
}
public static class AudioNameManager
{
    public static Dictionary<AudioName, string> audionames = new Dictionary<AudioName, string> 
{
    {AudioName.SE_BOUGYOKAMAE,"SE_BOUGYOKAMAE"},
    {AudioName.SE_BOUGYOUKE,"SE_BOUGYOUKE"},
    {AudioName.SE_CLUB,"SE_CLUB"},
    {AudioName.SE_CLUB_SHORT,"SE_CLUB_SHORT"},
    {AudioName.SE_CUT,"SE_CUT"},
    {AudioName.SE_DASH,"SE_DASH"},
    {AudioName.SE_DASH_SHORT,"SE_DASH_SHORT"},
    {AudioName.SE_DODGE,"SE_DODGE"},
    {AudioName.SE_DODGE_SHORT,"SE_DODGE_SHORT"},
    {AudioName.SE_DRAWING,"SE_DRAWING"},
    {AudioName.SE_HAJIKI,"SE_HAJIKI"},
    {AudioName.SE_HIPDROP,"SE_HIPDROP"},
    {AudioName.SE_KAIHI,"SE_KAIHI"},
    {AudioName.SE_KOUGEKI,"SE_KOUGEKI"},
    {AudioName.SE_KYUSEI,"SE_KYUSEI"},
    {AudioName.SE_LANCE,"SE_LANCE"},
    {AudioName.SE_LANCE2,"SE_LANCE2"},
    {AudioName.SE_REPEL,"SE_REPEL"},
    {AudioName.SE_TACKLE,"SE_TACKLE"},
    {AudioName.SE_WALK,"SE_WALK"},
    {AudioName.SE_WALK_SHORT,"SE_WALK_SHORT"},
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
