using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private PlayerAction action;

	// Use this for initialization
	void Start () {
		this.action = new PlayerAction(new PlayerAnimation(GetComponent<Animator>()), transform);
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		Attack();
	}

	private void Move() {
		var dir = new Vector3(
			Input.GetAxis("Horizontal"),
			0,
			Input.GetAxis("Vertical")
		);
		action.Move(dir);
	}

	private void Attack() {
	}
}
