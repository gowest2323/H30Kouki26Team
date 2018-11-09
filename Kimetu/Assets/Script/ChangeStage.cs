﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChangeStage : MonoBehaviour {
    private bool playerStay;
    public GameObject canvas;
    [SerializeField, Header("再生するタイムライン")]
    private PlayableDirector playableDirector;
    [SerializeField]
    private SceneName nextSceneName;

	// Use this for initialization
	void Start () {
        playerStay = false;
        canvas.gameObject.SetActive(false);
        GetComponent<LongPressDetector>();
        GetComponent<LongPressDetector>().OnLongPressEnd += () => {
            Debug.Log("stay " + playerStay);
            if (playerStay == true)
            {
                SceneChanger.Instance().Change(nextSceneName, new FadeData(1, 1, Color.black));
            }
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerStay = true;
            canvas.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerStay = false;
            canvas.gameObject.SetActive(false);
        }
    }
}
