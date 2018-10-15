using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 offset;
    public List<GameObject> targetObjects;
    private GameObject nearObj;
    [SerializeField]
    private float lockRange;
    bool isLockOn;




    // Use this for initialization
    private void Start ()
    {
        offset = transform.position - player.transform.position;
        isLockOn = false;
	}
	
	// Update is called once per frame
	private void Update ()
    {
        transform.position = player.transform.position + offset;
        Transform cameraTransform = transform;
        Vector3 cameraAngle = cameraTransform.eulerAngles;
        float angle_x = cameraAngle.x;
        float angle_y = cameraAngle.y;
        float angle_z = cameraAngle.z;
        //*
        float hor = Input.GetAxis(InputMap.Type.RStick_Horizontal.GetInputName());
        float ver = Input.GetAxis(InputMap.Type.RStick_Vertical.GetInputName());
      //  Debug.Log(hor);
       // Debug.Log(ver);
        //*/

        if (ver > 0)
        {
            cameraAngle.x += 5f;
        }
        if (ver < 0)
        {
            cameraAngle.x -= 5f;
        }
        if (hor > 0)//右
        {
            cameraAngle.y += 5f;
        }
        if (hor < 0)//左
        {
            cameraAngle.y -= 5f;
        }

        cameraTransform.eulerAngles = cameraAngle;
        IsLockOnChange();
        LockOn();
        
    }

    private void LockOn()
    {     
        if (isLockOn==true && nearObj != null)
        {
            transform.LookAt(nearObj.transform);
            player.transform.LookAt(nearObj.transform);
        }
    }


    private void IsLockOnChange()
    {
        if(isLockOn == true)
        {
            nearObj = SearchTag(gameObject, "Enemy");
            if(nearObj == null)
            {
                isLockOn = false;
            }
        }
        if (Input.GetButton(InputMap.Type.RStickClick.GetInputName()))
        {
            isLockOn = true;
        }

    }

  

    /// <summary>
    /// 近くにあるtagnameのオブジェクト取得
    /// </summary>
    /// <param name="nowObj"></param>
    /// <param name="tagName"></param>
    /// <returns></returns>
    GameObject SearchTag(GameObject nowObj, string tagName)
    {
        float tmpDis = 0;  

        GameObject targetObj = null;

        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            if(lockRange > tmpDis)
            {
                targetObj = obs;
            }
  
        }
       
        return targetObj;
    }

    //private void CameraRotate()
    //{

    //    //cameraAngle.x += 5f;
    //    //cameraAngle.y += 5f;
    //    //cameraAngle.z += 5f;

    //}
}
