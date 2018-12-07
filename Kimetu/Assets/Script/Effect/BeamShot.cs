using UnityEngine;
using System.Collections;
//https://qiita.com/kuuki_yomenaio/items/85f0233822ccc5272d1e

public enum BeamMode {
	Up,
	Down,
	Left,
	Right,
	Auto,
}

public class BeamShot : MonoBehaviour {
	float effectDisplayTime = 0.2f;
	float range = 100.0f;
	Ray shotRay;
	RaycastHit shotHit;
	ParticleSystem beamParticle;
	LineRenderer lineRenderer;

	[SerializeField]
	private BeamMode mode = BeamMode.Up;
	[SerializeField]
	private bool startOnAwake = false;
	private bool beamActive;

	// Use this for initialization
	void Awake () {
		beamParticle = GetComponent<ParticleSystem> ();
		lineRenderer = GetComponent<LineRenderer> ();
		beamParticle.Stop();
	}

	private void Start() {
		if(startOnAwake) {
			StartShot();
		}
	}

	// Update is called once per frame
	void Update () {
		if (beamActive) {
			BeamLayout();
		}
	}

	private void OnDestroy() {
		EndShot();
	}

	/// <summary>
	/// ビームを発射します。
	/// </summary>
	public void StartShot() {
		this.beamActive = true;
		beamParticle.Stop ();
		beamParticle.Play ();
		lineRenderer.enabled = true;
		BeamLayout();
	}

	private void BeamLayout() {
		lineRenderer.SetPosition (0, transform.position);
		shotRay.origin = transform.position;
		shotRay.direction = GetDirection();

		int layerMask = 0;

		if (Physics.Raycast(shotRay, out shotHit, range, layerMask)) {
			// hit
		}

		lineRenderer.SetPosition(1, shotRay.origin + shotRay.direction * range);

	}

	/// <summary>
	/// ビームを消します。
	/// </summary>
	public void EndShot() {
		beamParticle.Stop ();
		lineRenderer.enabled = false;
		this.beamActive = false;
	}

	private Vector3 GetDirection() {
		switch (mode) {
			case BeamMode.Up: return Vector3.up;

			case BeamMode.Down: return Vector3.down;

			case BeamMode.Left: return Vector3.left;

			case BeamMode.Right: return Vector3.right;

			case BeamMode.Auto: return transform.forward;
		}

		return Vector3.zero;
	}
}