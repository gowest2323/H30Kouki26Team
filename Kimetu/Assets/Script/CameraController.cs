using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 offset;
    private GameObject nearObj;
    [SerializeField]
    private float lockRange;
    bool isLockOn;
    bool interval;
    float intervalTime;
    

    [SerializeField]
    private float distance = 3.0f;    // 注視対象プレイヤーからカメラを離す距離
    [SerializeField]
    private Quaternion vRotation;      // カメラの垂直回転(見下ろし回転)
    [SerializeField]
    public Quaternion hRotation;      // カメラの水平回転
    [SerializeField]
    private float turnSpeed = 10.0f;   // 回転速度
  

    // Use this for initialization
    private void Start ()
    {
        offset = transform.position - player.transform.position;
        isLockOn = false;
        interval = false;
        intervalTime = 0;
        // 回転の初期化
        vRotation = Quaternion.Euler(30, 0, 0);         // 垂直回転(X軸を軸とする回転)は、30度見下ろす回転
        hRotation = Quaternion.identity;                // 水平回転(Y軸を軸とする回転)は、無回転
        transform.rotation = hRotation * vRotation;     // 最終的なカメラの回転は、垂直回転してから水平回転する合成回転

        // 位置の初期化
        // player位置から距離distanceだけ手前に引いた位置を設定します
        transform.position = player.transform.position - transform.rotation * Vector3.forward * distance;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = player.transform.position - transform.rotation * Vector3.forward * distance;
        transform.position = player.transform.position + offset;

        float hor = Input.GetAxis(InputMap.Type.RStick_Horizontal.GetInputName());
        float ver = Input.GetAxis(InputMap.Type.RStick_Vertical.GetInputName());

        hRotation *= Quaternion.Euler(0, hor * turnSpeed, 0);

        vRotation *= Quaternion.Euler(ver * turnSpeed, 0, 0);



        // カメラの回転(transform.rotation)の更新
        // 垂直回転してから水平回転する合成回転とします
        transform.rotation = hRotation * vRotation;

        // カメラの位置(transform.position)の更新
        // player位置から距離distanceだけ手前に引いた位置を設定します
        transform.position = player.transform.position + new Vector3(0, 1, 0) - transform.rotation * Vector3.forward * distance;

        IsLockOnChange();
        LockOn();
       Interval();
    }

    private void LockOn()
    {     
        if (isLockOn==true && nearObj != null)
        {
            transform.LookAt(nearObj.transform,Vector3.up);
            transform.position = player.transform.position + new Vector3(0, 1, 0) - transform.rotation * Vector3.forward * distance;
            player.transform.LookAt(nearObj.transform,Vector3.up);           
        }
    }

    private void IsLockOnChange()
    {
        if (Input.GetButtonDown(InputMap.Type.RStickClick.GetInputName())) 
        {
            if (interval == true) return;       
            isLockOn = !isLockOn;
            interval = true;
        }

        if (isLockOn == true)
        {
            nearObj = SearchTag(gameObject, "Enemy");
            if (nearObj == null)
            {        
                isLockOn = false;
                interval = false;
            }
        }
    }

    private void Interval()
    {
        if (interval)
        {
            intervalTime += 0.1f;
            if (intervalTime >= 0.6f)
            {
                interval = false;
                intervalTime = 0;
            }
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
}
