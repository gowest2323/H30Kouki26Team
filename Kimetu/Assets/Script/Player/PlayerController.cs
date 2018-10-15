using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerAction action;
    [SerializeField, Header("回生コマンドを実行するのにボタンを長押しする時間")]
    private float pierceButtonPushTime;
    // Use this for initialization
    void Start()
    {
        this.action = GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!action.IsAvoid())
        {
            Move();
            Attack();
            PierceAndHeal();
        }
        Avoid();
    }

    private void Move()
    {
        var dir = new Vector3(
            Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
            0,
            Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
        );
        action.Move(dir);
    }

    private void Attack()
    {
        if (Input.GetButton(InputMap.Type.XButton.GetInputName()))
        {
            action.Attack();
        }
    }

    private void PierceAndHeal()
    {
        if (InputExtend.GetButtonState(InputExtend.Command.Attack, pierceButtonPushTime))
        {
            Debug.Log("回生start");
            action.PierceAndHeal();
        }
    }

    private void Avoid()
    {
        var dir = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        if (Input.GetKeyDown(KeyCode.X))
        {
            action.Avoid(dir);
        }
    }
}
