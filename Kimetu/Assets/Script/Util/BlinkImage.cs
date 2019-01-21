﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour {
	[SerializeField]
	private Image image;

	[SerializeField]
	private float blinkRate = 1f;

	// Use this for initialization
	void Start () {
		StartCoroutine(Blink());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator Blink() {
		while(true) {
			yield return new WaitForSeconds(blinkRate);
			image.gameObject.SetActive(!image.gameObject.activeSelf);
		}
	}
}