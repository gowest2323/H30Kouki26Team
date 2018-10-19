using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private PlayerAction action;
    int pressButton = 0, holdShort = 5, holdLong = 15;

    // Use this for initialization
    void Start () {
		this.action = GetComponent<PlayerAction>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        pressButton += (Input.GetButton(InputMap.Type.AButton.GetInputName())) ? 1 : 0;

        if (Input.GetButton(InputMap.Type.AButton.GetInputName()))
        {
            if (holdLong <= pressButton)
            {
                Dash();
                Debug.Log("長押し");
            }
            else if (holdShort <= pressButton)
            {
                Avoid();
                Debug.Log("押し");
            }
        }

        if (Input.GetButtonUp(InputMap.Type.AButton.GetInputName())) pressButton = 0;
        //if (pressButton > 20) pressButton = 0;
        Debug.Log(pressButton);

        if (!action.IsAvoid())
        {
            Move();
            Attack();
        }
        //Avoid();
    }

	private void Move() {
		var dir = new Vector3(
			Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
			0,
			Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
		);
		action.Move(dir);

        //if (Input.GetButton("WIN_AButton"))
        //{
        //    action.Move(dir * 1.05f);
        //}
    }

    private void Dash()
    {
        var dir = new Vector3(
            Input.GetAxis(InputMap.Type.LStick_Horizontal.GetInputName()),
            0,
            Input.GetAxis(InputMap.Type.LStick_Vertical.GetInputName())
        );
        action.Move(dir * 1.05f);
    }
    private void Attack() {
		if(Input.GetKey(KeyCode.Z)||Input.GetButton("WIN_XButton")) {
			action.Attack();
		}
	}

    private void Avoid()
    {
        var dir = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );
        action.Avoid(dir);
    }
}
