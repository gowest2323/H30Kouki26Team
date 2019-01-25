using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour {
	private PlayerAction action;
	[SerializeField, Header("吸生コマンドを実行するのにボタンを長押しする時間")]
	private float pierceButtonPushTime;

	[SerializeField, Header("回避実行必要ボタン長押し秒数")]
	private float holdShort = 0.01f;
	[SerializeField, Header("ダッシュ実行必要ボタン長押し秒数")]
	private float holdLong = 0.5f;
	[SerializeField]
	private CameraController cameraController;
	private float pressButton;
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

	[SerializeField]
	private Kirinuke kirinuke;

	[SerializeField]
	private Rengeki rengeki;

    private bool isAvoid = false;//回避か
    private ChangeStage changeStage;

    // Use this for initialization
    void Start() {
		if(cameraController == null) {
			this.cameraController = Camera.main.GetComponent<CameraController>();
		}
		if(kirinuke == null) {
			this.kirinuke = GetComponent<Kirinuke>();
		}
		if (rengeki == null) {
			this.rengeki = GetComponent<Rengeki>();
		}
		Assert.IsTrue(pauseManager != null);
		this.action = GetComponent<PlayerAction>();
		longPressDetector.OnLongPressingOverTime += OnKyuuseiButtonPushStart;
		longPressDetector.OnLongPressEnd += OnKyuuseiButtonPushEnd;
		isKyuusei = false;
		status = GetComponent<PlayerStatus>();
        changeStage = FindObjectOfType<ChangeStage>();
	}

	private void OnKyuuseiButtonPushStart(float elapsed) {
		if (isKyuusei) return;

		action.PierceAndHeal();
		isKyuusei = true;
	}
	private void OnKyuuseiButtonPushEnd() {
		isKyuusei = false;
	}

	// Update is called once per frame
	void Update() {
		if(kirinuke.isRunning || rengeki.turnNow || rengeki.actionNow || rengeki.moveNow) {
			return;
		}
		if(SceneChanger.Instance().isChanging) {
			return;
		}
		if (inputDisableElapsed < inputDisableSeconds) {
			inputDisableElapsed += Time.deltaTime;
			return;
		}

		if (pauseManager.isPause) {
			return;
		}

		//スタミナ回復(ガード中は回復しない)
		if (!Input.GetButton(InputMap.Type.LButton.GetInputName())) {
			staminaTimeElapsed += Slow.Instance.PlayerDeltaTime();

			if (staminaTimeElapsed >= staminaHealTime) {
				status.RecoveryStamina();
				staminaTimeElapsed = 0;
			}
		}

		//Debug.Log(status.GetStamina());
		DashOrAvoid();

		//if (pressButton > 20) pressButton = 0;
		//        Debug.Log(pressButton);
		if (!action.IsAvoid()) {
			Guard();
			Move();
			Attack();
			PierceAndHeal();
		}

		//Avoid();
	}

	private void DashOrAvoid() {
		pressButton += (Input.GetButton(InputMap.Type.AButton.GetInputName())) ? Time.deltaTime : 0;

		if (Input.GetButton(InputMap.Type.AButton.GetInputName())) {
			if (holdLong <= pressButton) {
                isAvoid = false;
                Dash();
                //Debug.Log("長押し");
            } else if (holdShort <= pressButton) {
                isAvoid = true;
				//Debug.Log("押し");
			}
		} else {
#if UNITY_EDITOR
			if(Input.GetKeyDown(KeyCode.Space)) {
				isAvoid = true;
			}
#endif
		}

		if (Input.GetButtonUp(InputMap.Type.AButton.GetInputName())) {
            if (isAvoid && !pauseManager.isReturnFromPause) Avoid();
            pressButton = 0;
            isAvoid = false;
            pauseManager.isReturnFromPause = false;
        } else {
#if UNITY_EDITOR
			if (Input.GetKeyUp(KeyCode.Space)) {
				if (isAvoid && !pauseManager.isReturnFromPause) Avoid();
				pressButton = 0;
				isAvoid = false;
				pauseManager.isReturnFromPause = false;
			}
#endif
		}
	}

	private void Move() {
		if (holdLong <= pressButton) {
			//ダッシュ中
			return;
		}

		var dir = new Vector3(
			Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
			0,
			Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
		);
#if UNITY_EDITOR
		dir = GetKeyboardDirection(dir);
#endif
		action.Move(dir, !cameraController.IsLockOn());
		cameraController.playerDir = dir;
	}

	private Vector3 GetKeyboardDirection(Vector3 dir) {
		if (Input.GetKey(KeyCode.W)) {
			dir.z = 1;
		} else if (Input.GetKey(KeyCode.S)) {
			dir.z = -1;
		}
		if (Input.GetKey(KeyCode.A)) {
			dir.x = -1;
		} else if (Input.GetKey(KeyCode.D)) {
			dir.x = 1;
		}
		dir = dir.normalized;
		return dir;
	}

	private void Attack() {
		if (isKyuusei) return;
        //ボタンが押されている && (次のステージへ行くためのアレがない || アレに触れていない)
		if (Input.GetButtonUp(InputMap.Type.XButton.GetInputName()) && (changeStage == null || !changeStage.toNextStage)) {
			action.Attack();
		} else {
#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.Z) && (changeStage == null || !changeStage.toNextStage)) {
				action.Attack();
			}
#endif
		}
	}

	/// <summary>
	/// 吸生
	/// </summary>
	private void PierceAndHeal() {
		////攻撃ボタン長押しで発動
		//if (InputExtend.GetButtonState(InputExtend.Command.Attack, pierceButtonPushTime))
		//{
		//    action.PierceAndHeal();
		//}
	}
	private void Dash() {
		#if UNITY_EDITOR
		var dir = new Vector3(
			Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
			0,
			Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
		);
		action.Dash(dir * 1.05f);
		#endif
	}

	private void Avoid() {
		var dir = new Vector3(
			Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
			0,
			Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
		);

        action.Avoid(dir);
	}

	private void Guard() {
		var dir = new Vector3(
			Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
			0,
			Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
		);

		if (Input.GetButtonDown(InputMap.Type.LButton.GetInputName())) {
			//if (!cameraController.IsLockOn())
			//	cameraController.PositionToPlayerBack();
			action.GuardStart(dir);

		} else if (Input.GetButtonUp(InputMap.Type.LButton.GetInputName())) {
			action.GuardEnd();
		}
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			//if (!cameraController.IsLockOn())
			//	cameraController.PositionToPlayerBack();
			action.GuardStart(dir);

		} else if (Input.GetKeyUp(KeyCode.LeftShift)) {
			action.GuardEnd();
		}
#endif
	}
}
