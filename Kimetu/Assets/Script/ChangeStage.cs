using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStage : MonoBehaviour {
    private bool playerStay;
    public GameObject canvas;

	// Use this for initialization
	void Start () {
        playerStay = false;
        canvas.gameObject.SetActive(false);
        GetComponent<LongPressDetector>();
        GetComponent<LongPressDetector>().OnLongPressEnd += () => {
            if (playerStay == true)
            {
                Debug.Log("ステージ移行できます");
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
