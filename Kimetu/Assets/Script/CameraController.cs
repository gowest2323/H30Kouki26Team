using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 offset;

	// Use this for initialization
	private void Start ()
    {
        offset = transform.position - player.transform.position;
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
        float hor = Input.GetAxis("Horizontal_R");
        float ver = Input.GetAxis("Vertical_R");
      //  Debug.Log(hor);
        Debug.Log(ver);
        //*/

        if (Input.GetKey(KeyCode.DownArrow))
        {
            cameraAngle.x += 5f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cameraAngle.x -= 5f;
        }
        if (Input.GetKey(KeyCode.RightArrow))//右
        {
            cameraAngle.y += 5f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))//左
        {
            cameraAngle.y -= 5f;
        }

        cameraTransform.eulerAngles = cameraAngle;
    }

    //private void CameraRotate()
    //{

    //    //cameraAngle.x += 5f;
    //    //cameraAngle.y += 5f;
    //    //cameraAngle.z += 5f;

    //}
}
