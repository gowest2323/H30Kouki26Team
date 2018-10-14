using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private PlayerAction action;

	// Use this for initialization
	void Start () {
		this.action = GetComponent<PlayerAction>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!action.IsAvoid())
        {
            Move();
            Attack();
        }
        Avoid();
    }

	private void Move() {
		var dir = new Vector3(
			Input.GetAxis("Horizontal_L"),
			0,
			Input.GetAxis("Vertical_L")
		);
		action.Move(dir);
	}

	private void Attack() {
		if(Input.GetKey(KeyCode.Z)) {
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            action.Avoid(dir);
        }
    }
}
