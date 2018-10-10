using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CharacterAnimation : MonoBehaviour {
	/// <summary>
	/// アニメーションの速度を 0-1 の範囲で設定します。
	/// </summary>
	/// <value></value>
	public float speed {
		set {
			Init();
			animator.speed = value;
		}
		get {
			Init();
			return animator.speed;
		}
	}
	private Animator animator;

	// Use this for initialization
	public void Start () {
		Init();
	}
	
	// Update is called once per frame
	public void Update () {
		
	}

	private void Init() {
		if(this.animator != null) {
			return;
		}
		this.animator = GetComponent<Animator>();
		Assert.IsTrue(animator != null, "Animatorコンポーネントがありません:" + name);
	}
}
