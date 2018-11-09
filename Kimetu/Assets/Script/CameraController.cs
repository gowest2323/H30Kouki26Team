using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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

    [SerializeField]
    private float cameraHighestAngle = 60.0f;   // カメラ垂直最高角度
    [SerializeField]
    private float cameraLowestAngle =-10.0f;    // カメラ垂直最低角度

    private float nowDistance = 0;  //今カメラとプレイヤーの距離

    [HideInInspector]
    public Vector3 playerDir;   //プレイヤーの向き

    // Use this for initialization
    private void Start()
    {
        offset = transform.position - player.transform.position;
        isLockOn = false;
        interval = false;
        intervalTime = 0;
        // 回転の初期化
        vRotation = Quaternion.Euler(20, 0, 0);         // 垂直回転(X軸を軸とする回転)は、30度見下ろす回転
        hRotation = Quaternion.identity;                // 水平回転(Y軸を軸とする回転)は、無回転
        transform.rotation = hRotation * vRotation;     // 最終的なカメラの回転は、垂直回転してから水平回転する合成回転

        nowDistance = distance;

        // 位置の初期化
        // player位置から距離distanceだけ手前に引いた位置を設定します
        transform.position = player.transform.position - transform.rotation * Vector3.forward * nowDistance;

    }

    // Update is called once per frame
    private void Update()
    {
        DefaultControl();
        IsLockOnChange();
        LockOn();
        Interval();
    }

    private void DefaultControl()
    {
        if (coroutineCount > 0 || finished)
        {
            return;
        }
        //*
        Debug.Log("DefaultControl");
        float hor = Input.GetAxis(InputMap.Type.RStick_Horizontal.GetInputName());
        float ver = Input.GetAxis(InputMap.Type.RStick_Vertical.GetInputName());

        // カメラの回転(transform.rotation)の更新
        // 垂直回転してから水平回転する合成回転とします

        //回転計算
        if (Mathf.Abs(hor) >= 0.1f)//カメラの水平操作がある時
        {
            transform.RotateAround(player.transform.position, Vector3.up, hor * turnSpeed);

            //プレイヤーの向きの計算（水平）
            hRotation *= Quaternion.Euler(0, hor * turnSpeed, 0);
        }
        else//カメラの水平操作がない時
        {
            //プレイヤーの向きに沿って自動水平回転（速度半分）
            if (Mathf.Abs(playerDir.x) >= 0.1f)
                transform.RotateAround(player.transform.position, Vector3.up, playerDir.x * (turnSpeed/2));

            //プレイヤーの向きの計算（水平）
            hRotation *= Quaternion.Euler(0, playerDir.x * (turnSpeed/2), 0);
        }

        if (Mathf.Abs(ver) >= 0.05f)//カメラの垂直操作がある時
            transform.Rotate(new Vector3(ver * turnSpeed, 0, 0));

        //プレイヤーの向きの計算（垂直）
        vRotation *= Quaternion.Euler(ver * turnSpeed, 0, 0);


        //角度制限
        float rotationX = transform.eulerAngles.x;
        rotationX = (rotationX > 180) ? rotationX -= 360 : rotationX;
        if (rotationX > cameraHighestAngle)
        {
            rotationX = cameraHighestAngle;
        }
        else if (rotationX < cameraLowestAngle)
        {
            rotationX = cameraLowestAngle;
        }
        transform.eulerAngles = new Vector3(rotationX, transform.eulerAngles.y, 0);


        // カメラの位置(transform.position)の更新
        // player位置から距離distanceだけ手前に引いた位置を設定します

        // 位置を壁の内側に
        nowDistance = Distance_PlayertoWall() > 0f ? Distance_PlayertoWall() : distance;
        if (nowDistance <= 0f) nowDistance = 0f;
        transform.position = player.transform.position + new Vector3(0, 1, 0) - transform.rotation * Vector3.forward * nowDistance;
        //*/
    }

    public bool IsLockOn()
    {
        return isLockOn;
    }

    private void LockOn()
    {
        if (!isLockOn || nearObj == null)
        {
            StopLockOnStart();
            this.finished = false;
            return;
        }
        if (this.rotateCoroutine != null)
        {
            if (!finished) { return; }
            StopLockOnStart();
        }
        this.finished = false;
        this.rotateCoroutine = StartCoroutine(LockOnStart(nearObj.transform.position));
    }

    private void StopLockOnStart()
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            this.rotateCoroutine = null;
            this.coroutineCount = 0;
        }
    }

    private IEnumerator LockOnStart(Vector3 nearPos)
    {
        this.finished = false;
        this.coroutineCount++;
        var waitOne = new WaitForEndOfFrame();
        var selfPos = transform.position;
        var playerPos = player.transform.position;
        var playerLookNearPos = nearPos;
        //nearPos.y = selfPos.y ;
        playerLookNearPos.y = selfPos.y;
        playerPos.y = selfPos.y;
        var selfStartRotation = transform.rotation;
        var selfEndRotation = Quaternion.LookRotation(nearPos - selfPos, Vector3.up);
        var playerStartRotation = player.transform.rotation;
        var playerEndRotation = Quaternion.LookRotation(playerLookNearPos - playerPos, Vector3.up);
        if (Quaternion.Angle(selfStartRotation, selfEndRotation) < 0.1f &&
           Quaternion.Angle(playerStartRotation, playerEndRotation) < 0.1f)
        {
            this.coroutineCount--;
            this.finished = true;
            Debug.Log("BREAK");
            yield break;
        }
        var offset = 0f;
        var seconds = 0.2f;
        while (offset < seconds)
        {
            var t = Time.time;
            yield return waitOne;
            var diff = (Time.time - t);
            offset += diff;
            offset = Mathf.Clamp(offset, 0, seconds);
            var selfProgRot = Quaternion.Slerp(selfStartRotation, selfEndRotation, offset / seconds);
            var playerProgRot = Quaternion.Slerp(playerStartRotation, playerEndRotation, offset / seconds);
            transform.rotation = selfProgRot;
            player.transform.rotation = playerProgRot;

            // 位置を壁の内側に
            nowDistance = Distance_PlayertoWall() > 0f ? Distance_PlayertoWall() : distance;
            if (nowDistance <= 0f) nowDistance = 0f;
            transform.position = player.transform.position + new Vector3(0, 1, 0) - transform.rotation * Vector3.forward * nowDistance;

            //微妙に見下ろすように
            if (nowDistance < distance)
                transform.position += (Vector3.up * angleY * 1.5f);
            else
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

            if (lockRange > tmpDis)
            {
                targetObj = obs;
            }

        }

        return targetObj;
    }

    private float Distance_PlayertoWall()
    {
        Ray ray = new Ray(player.transform.position + new Vector3(0, 1, 0), -transform.forward);

        RaycastHit hit;
        float distanceW = 0;
        if(Physics.Raycast(ray,out hit, 4.0f, LayerMask.GetMask("Stage")))
        {
            distanceW = Vector3.Distance(new Vector3(player.transform.position.x, transform.position.y,player.transform.position.z),
                                        new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
        else
        {
            distanceW = 0;
        }

        //Debug.Log("distanceFromWall >> " + distanceW);

        return distanceW;
    }

    //カメラがプレイヤーの背後に戻す
    public void CameraToPlayerBack()
    {
        vRotation = Quaternion.Euler(20, 0, 0);
        hRotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0);//プレイヤーの向きに合わせ

        //回転
        transform.rotation = hRotation * vRotation;

        // 位置
        nowDistance = distance;
        transform.position = player.transform.position - transform.rotation * Vector3.forward * nowDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Ray ray = new Ray(player.transform.position + new Vector3(0, 1, 0), -transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4.0f, LayerMask.GetMask("Stage")))
        {

            Gizmos.DrawLine(transform.position, hit.point);

        }
    }
}