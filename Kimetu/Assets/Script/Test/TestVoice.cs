using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestVoice : MonoBehaviour {
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Button sampleAButton;

	[SerializeField]
	private Button sampleBButton;


	private bool playing;
	private bool playVoiceAtAnimation;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void PlayVoice() {
		if (playVoiceAtAnimation) {
			AudioManager.Instance.PlayPlayerSE(AudioName.oni_oaa_preAttack_03.String());
		}
	}

	//ボタンの処理
	public void PushSampleA() {
		this.playVoiceAtAnimation = true;
		sampleAButton.enabled = false;
		sampleBButton.enabled = false;
		StartCoroutine(PlaySampleA());
	}

	private IEnumerator PlaySampleA() {
		animator.SetTrigger(EnemyAttackTypeDictionary.dictionary[EnemyAttackType.SwingDown]);
		yield return new WaitForSeconds(1);
		sampleAButton.enabled = true;
		sampleBButton.enabled = true;
	}

	public void PushSampleB() {
		this.playVoiceAtAnimation = false;
		sampleAButton.enabled = false;
		sampleBButton.enabled = false;
		StartCoroutine(PlaySampleB());
	}

	private IEnumerator PlaySampleB() {
		AudioManager.Instance.PlayPlayerSE(AudioName.oni_oaa_preAttack_03.String());
		yield return new WaitForSeconds(1);
		animator.SetTrigger(EnemyAttackTypeDictionary.dictionary[EnemyAttackType.SwingDown]);
		yield return new WaitForSeconds(1);
		sampleAButton.enabled = true;
		sampleBButton.enabled = true;
	}
}
