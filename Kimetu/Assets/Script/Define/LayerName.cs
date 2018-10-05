using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum LayerName
{
Default,
TransparentFX,
IgnoreRaycast,
Water,
UI,
}
public static class LayerNameManager
{
    public static Dictionary<LayerName, string> layernames = new Dictionary<LayerName, string> 
{
    {LayerName.Default,"Default"},
    {LayerName.TransparentFX,"TransparentFX"},
    {LayerName.IgnoreRaycast,"IgnoreRaycast"},
    {LayerName.Water,"Water"},
    {LayerName.UI,"UI"},
};
    public static bool Equals(LayerName layername, string name)
    {
        return layernames[layername] == name;
    }
    public static bool Equals(string name, LayerName layername)
    {
        return name == layernames[layername];
    }
    public static bool Equals(string name1, string name2)
    {
        return name1 == name2;
    }
    public static bool Equals(LayerName layername1, LayerName layername2)
    {
        return layername1 == layername2;
    }
    public static string String(this LayerName layername)
    {
        return layernames[layername];
    }
    public static LayerName GetKeyByValue(string name)
    {
        return layernames.FirstOrDefault(pair => pair.Value == name).Key;
    }
}
