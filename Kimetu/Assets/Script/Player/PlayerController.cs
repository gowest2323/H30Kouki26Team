using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerAction action;

    // Use this for initialization
    void Start()
    {
        this.action = GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
        Guard();
    }

    private void Move()
    {
        var dir = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );
        action.Move(dir);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            action.Attack();
        }
    }

    /// <summary>
    /// ガード処理
    /// </summary>
    private void Guard()
    {
        //押し初めにガード開始
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("guard start");
            action.GuardStart();            
        }
        //離した瞬間にガード終了
        else if (Input.GetKeyUp(KeyCode.G))
        {
            Debug.Log("guard end");
            action.GuardEnd();
        }
    }
}
