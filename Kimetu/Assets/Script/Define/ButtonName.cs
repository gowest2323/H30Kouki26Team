using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum ButtonName
{
Horizontal,
Vertical,
Fire1,
Fire2,
Fire3,
Jump,
MouseX,
MouseY,
MouseScrollWheel,
Submit,
Cancel,
}
public static class ButtonNameManager
{
    public static Dictionary<ButtonName, string> buttonnames = new Dictionary<ButtonName, string> 
{
    {ButtonName.Horizontal,"Horizontal"},
    {ButtonName.Vertical,"Vertical"},
    {ButtonName.Fire1,"Fire1"},
    {ButtonName.Fire2,"Fire2"},
    {ButtonName.Fire3,"Fire3"},
    {ButtonName.Jump,"Jump"},
    {ButtonName.MouseX,"MouseX"},
    {ButtonName.MouseY,"MouseY"},
    {ButtonName.MouseScrollWheel,"MouseScrollWheel"},
    {ButtonName.Submit,"Submit"},
    {ButtonName.Cancel,"Cancel"},
};
    public static bool Equals(ButtonName buttonname, string name)
    {
        return buttonnames[buttonname] == name;
    }
    public static bool Equals(string name, ButtonName buttonname)
    {
        return name == buttonnames[buttonname];
    }
    public static bool Equals(string name1, string name2)
    {
        return name1 == name2;
    }
    public static bool Equals(ButtonName buttonname1, ButtonName buttonname2)
    {
        return buttonname1 == buttonname2;
    }
    public static string String(this ButtonName buttonname)
    {
        return buttonnames[buttonname];
    }
    public static ButtonName GetKeyByValue(string name)
    {
        return buttonnames.FirstOrDefault(pair => pair.Value == name).Key;
    }
}
