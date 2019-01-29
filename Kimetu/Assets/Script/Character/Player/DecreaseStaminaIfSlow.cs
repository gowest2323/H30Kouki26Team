using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseStaminaIfSlow : MonoBehaviour {
	[SerializeField, Header("1スタミナが減少する時間（秒）")]
	private float decreaseSlowSeconds = 0.3f;
	[SerializeField, Header("スロー中に減るスタミナ量")]
	private float decreaseSlowStamina = 1;

	private PlayerStatus status;
	private float slowElapsed;
	// Use this for initialization
	void Start () {
		if (status == null) {
			this.status = GetComponent<PlayerStatus>();
		}
	}

	// Update is called once per frame
	void Update () {
		DecreaseSlowStamina();
	}

	private void DecreaseSlowStamina() {
		if (!Slow.Instance.isSlowNow) {
			return;
		}

		this.slowElapsed += Time.deltaTime;

		if (slowElapsed >= decreaseSlowSeconds) {
			slowElapsed = 0;
			status.DecreaseStamina(decreaseSlowStamina);
		}
	}
}
