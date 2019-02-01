using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneLazy : MonoBehaviour {
	[SerializeField]
	private SceneName name;

	[SerializeField]
	private float wait = 1f;

	// Use this for initialization
	void Start () {
		StartCoroutine(Wait());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds(wait);
		SceneChanger.Instance().Change(name, new FadeData(1, 1, Color.black));
	}
}
