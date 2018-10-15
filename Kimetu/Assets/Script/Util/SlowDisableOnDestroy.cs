using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトが破棄される前に Slow から自身を削除します。
/// </summary>
public class SlowDisableOnDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDestroy() {
		Slow.Instance.Remove(GetComponent<ICharacterAnimationProvider>().characterAnimation);
	}
}
