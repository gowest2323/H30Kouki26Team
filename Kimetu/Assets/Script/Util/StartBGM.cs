using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBGM : MonoBehaviour {
	[SerializeField]
	private AudioName name;

	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayBGM(name.String());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy() {
		AudioManager.Instance.StopBGM();
	}
}
