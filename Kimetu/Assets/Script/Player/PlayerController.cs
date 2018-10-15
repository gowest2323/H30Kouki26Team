using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerAction action;
    [SerializeField, Header("回生コマンドを実行するのにボタンを長押しする時間")]
    private float pierceButtonPushTime;
	private bool guardTriggered;
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
			Guard();
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

    /// <summary>
    /// 回生
    /// </summary>
    private void PierceAndHeal()
    {
        //攻撃ボタン長押しで発動
        if (InputExtend.GetButtonState(InputExtend.Command.Attack, pierceButtonPushTime))
        {
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

        if (Input.GetButton(InputMap.Type.AButton.GetInputName()))
        {
            action.Avoid(dir);
        }
    }

	private void Guard() {
		if(Input.GetButton(InputMap.Type.LButton.GetInputName()) && !guardTriggered) {
			action.GuardStart();
			PrivateLogger.KOYA.Log("guard start");
			this.guardTriggered = true;
		} else if (!Input.GetButton(InputMap.Type.LButton.GetInputName()) && guardTriggered) {
			action.GuardEnd();
			PrivateLogger.KOYA.Log("guard end");
			this.guardTriggered = false;
		}
	}
}
