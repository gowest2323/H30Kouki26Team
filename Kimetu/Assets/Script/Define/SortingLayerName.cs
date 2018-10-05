using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum SortingLayerName
{
Default,
}
public static class SortingLayerNameManager
{
    public static Dictionary<SortingLayerName, string> sortinglayernames = new Dictionary<SortingLayerName, string> 
{
    {SortingLayerName.Default,"Default"},
};
    public static bool Equals(SortingLayerName sortinglayername, string name)
    {
        return sortinglayernames[sortinglayername] == name;
    }
    public static bool Equals(string name, SortingLayerName sortinglayername)
    {
        return name == sortinglayernames[sortinglayername];
    }
    public static bool Equals(string name1, string name2)
    {
        return name1 == name2;
    }
    public static bool Equals(SortingLayerName sortinglayername1, SortingLayerName sortinglayername2)
    {
        return sortinglayername1 == sortinglayername2;
    }
    public static string String(this SortingLayerName sortinglayername)
    {
        return sortinglayernames[sortinglayername];
    }
    public static SortingLayerName GetKeyByValue(string name)
    {
        return sortinglayernames.FirstOrDefault(pair => pair.Value == name).Key;
    }
}
