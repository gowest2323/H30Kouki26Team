using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 offset;
    private GameObject nearObj;
    [SerializeField]
    private float lockRange = 10.0f;
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

    private Coroutine rotateCoroutine;
    private bool finished;
    private int coroutineCount;

    [SerializeField]
    private float angleY = 1.0f;


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
        DefaultControl();
        IsLockOnChange();
        LockOn();
        Interval();
    }

    private void DefaultControl() {
        if(coroutineCount > 0 || finished) {
            return;
        }
        //*
        Debug.Log("DefaultControl");
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
        //*/
    }

    public bool IsLockOn()
    {
        return isLockOn;
    }

    private void LockOn()
    {
        if(!isLockOn || nearObj == null) {
            StopLockOnStart();
            this.finished = false;
            return;
        }
        if(this.rotateCoroutine != null) {
            if(!finished) { return; }
            StopLockOnStart();
        }
        this.finished = false;
        this.rotateCoroutine = StartCoroutine(LockOnStart(nearObj.transform.position));
    }

    private void StopLockOnStart() {
        if(rotateCoroutine != null) {
            StopCoroutine(rotateCoroutine);
            this.rotateCoroutine = null;
            this.coroutineCount = 0;
        }
    }

    private IEnumerator LockOnStart(Vector3 nearPos) {
        this.finished = false;
        this.coroutineCount++;
        var waitOne = new WaitForEndOfFrame();
        var selfPos = transform.position;
        var playerPos = player.transform.position;
        nearPos.y = selfPos.y ;
        playerPos.y = selfPos.y;
        var selfStartRotation = transform.rotation;
        var selfEndRotation = Quaternion.LookRotation(nearPos - selfPos, Vector3.up);
        var playerStartRotation = player.transform.rotation;
        var playerEndRotation = Quaternion.LookRotation(nearPos - playerPos, Vector3.up);
        if(Quaternion.Angle(selfStartRotation, selfEndRotation) < 0.1f &&
           Quaternion.Angle(playerStartRotation, playerEndRotation) < 0.1f) {
            this.coroutineCount--;
            this.finished = true;
            Debug.Log("BREAK");
            yield break;
        }
        var offset = 0f;
        var seconds = 0.25f;
        while(offset < seconds) {
            var t = Time.time;
            yield return waitOne;
            var diff = (Time.time - t);
            offset += diff;
            offset = Mathf.Clamp(offset, 0, seconds);
            var selfProgRot = Quaternion.Slerp(selfStartRotation, selfEndRotation, offset / seconds);
            var playerProgRot = Quaternion.Slerp(playerStartRotation, playerEndRotation, offset / seconds);
            transform.rotation = selfProgRot;
            player.transform.rotation = playerProgRot;
            transform.position = player.transform.position + new Vector3(0, 1, 0) - transform.rotation * Vector3.forward * distance;
            //微妙に見下ろすように
            transform.position += (Vector3.up * angleY);
            //回転をプレイヤーへ適用
            var euler = transform.rotation.eulerAngles;
            hRotation = Quaternion.Euler(0, euler.y, 0);
            vRotation = Quaternion.Euler(euler.x, 0, 0);
        }
        yield return waitOne;
        this.coroutineCount--;
        this.finished = true;
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