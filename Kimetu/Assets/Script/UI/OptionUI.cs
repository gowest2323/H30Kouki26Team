using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour {

    [SerializeField]
    private MenuUI menu;
    [SerializeField]
    private Toggle cameraUpDown;
    [SerializeField]
    private Toggle cameraLeftRight;

    
    
    void Start()
    {
        cameraUpDown.interactable = OptionDataPrefs.GetUpDownBool(false);
        cameraLeftRight.interactable = OptionDataPrefs.GetLeftRightBool(false);
    }
    // Update is called once per frame
    void Update()
    {

        if (!Input.GetButton(InputMap.Type.BButton.GetInputName()))
        {
            return;
        }

        var selected = menu.selected;
        if (selected == 0)
        {//カメラ上下反転
            CameraUpDownInverted();
        }
        else if (selected == 1)
        {//カメラ左右反転
            CameraLeftRightInverted();
        }

    }

    //カメラ上下反転するか
    private void CameraUpDownInverted()
    {

        if (Input.GetButtonDown(InputMap.Type.BButton.GetInputName()))
        {
            cameraUpDown.interactable = !cameraUpDown.interactable;
            OptionDataPrefs.SetUpDownInvert(cameraUpDown.interactable);
        }

        
    }
    //カメラ左右反転するか
    private void CameraLeftRightInverted()
    {

        if (Input.GetButtonDown(InputMap.Type.BButton.GetInputName()))
        {
            cameraLeftRight.interactable = !cameraLeftRight.interactable;
            OptionDataPrefs.SetLeftRightInvert(cameraLeftRight.interactable);
        }       
    }
}
