using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAura : AuraParticle {
	[SerializeField]
	private ParticleSystem auraBossDyingState;
	private bool isSwitchParticle;

	protected override void Start() {
		base.Start();
		isSwitchParticle = false;
	}

	protected override void Update() {
		base.Update();

		if (isSwitchParticle) return;

		if (enemyStatus.GetHP() == 1) {
			//Destroy(auraParticle);
			//GameObject.Destroy
			auraParticle.Stop();
			Destroy(auraParticle.gameObject, 5f);
			auraParticle = GameObject.Instantiate(auraBossDyingState, transform.FindRec(auraPositionName).transform);
			//auraParticle.Play();
			isSwitchParticle = true;
		}
	}
}
