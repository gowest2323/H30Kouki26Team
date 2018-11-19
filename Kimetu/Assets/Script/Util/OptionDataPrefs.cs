using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OptionDataPrefs  {

    public static bool GetUpDownBool( bool defalutValue)
    {
        var value = PlayerPrefs.GetInt("camera_UpDown", defalutValue ? 1 : 0);
        return value == 1;
    }

    public static bool GetLeftRightBool( bool defalutValue)
    {
        var value = PlayerPrefs.GetInt("camera_LeftRight", defalutValue ? 1 : 0);
        return value == 1;
    }

    public static void SetUpDownInvert(bool value)
    {
        PlayerPrefs.SetInt("camera_UpDown", value ? 1 : 0);
        PlayerPrefs.Save();
    }
    public static void SetLeftRightInvert(bool value)
    {
        PlayerPrefs.SetInt("camera_LeftRight", value ? 1 : 0);
        PlayerPrefs.Save();
    }

}

