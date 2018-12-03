using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    private PlayerAction action;
    [SerializeField, Header("吸生コマンドを実行するのにボタンを長押しする時間")]
    private float pierceButtonPushTime;

    [SerializeField, Header("回避コマンドを実行するのにボタンを長押しする時間")]
    private int holdShort = 5;
    [SerializeField, Header("ダッシュ状態になるのにボタンを長押しする時間")]
    private int holdLong = 15;
    [SerializeField]
    private CameraController cameraController;
    private int pressButton;
    [SerializeField]
    private LongPressDetector longPressDetector;
    private bool isKyuusei;

    [SerializeField]
    private float staminaHealTime = 0.2f;
    private float staminaTimeElapsed;
    private PlayerStatus status;
    [SerializeField]
    private PauseManager pauseManager;

    [SerializeField]
    private float inputDisableSeconds = 0.5f;
    private float inputDisableElapsed;
    // Use this for initialization
    void Start()
    {
        Assert.IsTrue(pauseManager != null);
        this.action = GetComponent<PlayerAction>();
        longPressDetector.OnLongPressingOverTime += OnKyuuseiButtonPushStart;
        longPressDetector.OnLongPressEnd += OnKyuuseiButtonPushEnd;
        isKyuusei = false;
        status = GetComponent<PlayerStatus>();
    }

    private void OnKyuuseiButtonPushStart(float elapsed)
    {
        if (isKyuusei) return;
        action.PierceAndHeal();
        isKyuusei = true;
    }
    private void OnKyuuseiButtonPushEnd()
    {
        isKyuusei = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(inputDisableElapsed < inputDisableSeconds) {
            inputDisableElapsed += Time.deltaTime;
            return;
        }
        if(pauseManager.isPause) {
            return;
        }
        //スタミナ回復(ガード中は回復しない)
        if (!Input.GetButton(InputMap.Type.LButton.GetInputName()))
        {
            staminaTimeElapsed += Slow.Instance.PlayerDeltaTime();
            if (staminaTimeElapsed >= staminaHealTime)
            {
                status.RecoveryStamina();
                staminaTimeElapsed = 0;
            }
        }
        //Debug.Log(status.GetStamina());
        DashOrAvoid();
        //if (pressButton > 20) pressButton = 0;
        //        Debug.Log(pressButton);
        if (!action.IsAvoid())
        {
            Guard();
            Move();
            Attack();
            PierceAndHeal();
        }
        //Avoid();
    }

    private void DashOrAvoid()
    {
        pressButton += (Input.GetButton(InputMap.Type.AButton.GetInputName())) ? 1 : 0;
        if (Input.GetButton(InputMap.Type.AButton.GetInputName()))
        {
            if (holdLong <= pressButton)
            {
                Dash();
                //Debug.Log("長押し");
            }
            else if (holdShort <= pressButton)
            {
                Avoid();
                //Debug.Log("押し");
            }
        }
        if (Input.GetButtonUp(InputMap.Type.AButton.GetInputName())) pressButton = 0;
    }

    private void Move()
    {
        if (holdLong <= pressButton)
        {
            //ダッシュ中
            return;
        }
        var dir = new Vector3(
            Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
            0,
            Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
        );
        action.Move(dir, !cameraController.IsLockOn());
        cameraController.playerDir = dir;
    }

    private void Attack()
    {
        if (isKyuusei) return;
        if (Input.GetButtonUp(InputMap.Type.XButton.GetInputName()))
        {
            action.Attack();
        }
    }

    /// <summary>
    /// 吸生
    /// </summary>
    private void PierceAndHeal()
    {
        ////攻撃ボタン長押しで発動
        //if (InputExtend.GetButtonState(InputExtend.Command.Attack, pierceButtonPushTime))
        //{
        //    action.PierceAndHeal();
        //}
    }
    private void Dash()
    {
        var dir = new Vector3(
            Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
            0,
            Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
        );
        action.Dash(dir * 1.05f);
    }

    private void Avoid()
    {
        var dir = new Vector3(
            Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
            0,
            Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
        );
        if (Input.GetButton(InputMap.Type.AButton.GetInputName()))
        {
            action.Avoid(dir);
        }
    }

    private void Guard()
    {
        var dir = new Vector3(
            Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
            0,
            Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
        );

        if (Input.GetButtonDown(InputMap.Type.LButton.GetInputName()))
        {
            if (!cameraController.IsLockOn())
                cameraController.CameraToPlayerBack();

            action.GuardStart(dir);
        }
        else if (Input.GetButtonUp(InputMap.Type.LButton.GetInputName()))
        {
            action.GuardEnd();
        }
    }
}
