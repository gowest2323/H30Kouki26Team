using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePart : MoviePart {
	[SerializeField]
	private GameObject particle;

	[SerializeField]
	private GameObject point;

	[SerializeField]
	private MoviePart next;

	public override IEnumerator MovieUpdate() {
		var obj = GameObject.Instantiate(particle);
		obj.transform.position = point.transform.position;
		var part = obj.GetComponent<ParticleSystem>();

		if (!part.main.playOnAwake) {
			part.Play();
		}

		yield return next.MovieUpdate();
		part.Stop();
		yield break;
	}
}
