using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class InputDemoTest {

	[Test]
	public void InputDemoTestSimplePasses() {
		// Use the Assert class to test conditions.
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator InputDemoTestWithEnumeratorPasses() {
		var obj = new GameObject("obj");
		obj.AddComponent<InputDemo>();
		yield return new WaitForSeconds(5);
	}
}
