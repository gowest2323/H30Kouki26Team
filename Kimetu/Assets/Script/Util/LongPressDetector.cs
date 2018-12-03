using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 長押しの最初で呼ばれるデリゲート。
/// </summary>
public delegate void LongPressBegin();

/// <summary>
/// 長押しの間ずっと呼ばれるデリゲート。
/// </summary>
/// <param name="elapsed"></param>
public delegate void LongPressing(float elapsed);

/// <summary>
/// 一定秒数以上押されている間ずっと呼ばれるデリゲート
/// </summary>
/// <param name="elapsed"></param>
public delegate void LongPressingOverTime(float elapsed);

/// <summary>
/// 長押しが正常に完了すると呼ばれるデリゲート。
/// </summary>
public delegate void LongPressComplete();

/// <summary>
/// 長押しが終わると呼ばれるデリゲート。
/// </summary>
public delegate void LongPressEnd();

/// <summary>
/// 長押しを検出するクラス。
/// </summary>
public class LongPressDetector : MonoBehaviour {
	public event LongPressBegin OnLongPressBegin = delegate { };
	public event LongPressing OnLongPressing = delegate { };
	public event LongPressingOverTime OnLongPressingOverTime = delegate { };
	public event LongPressComplete OnLongPressComplete = delegate { };
	public event LongPressEnd OnLongPressEnd = delegate { };

	[SerializeField]
	private InputMap.Type type;
	public InputMap.Type buttonType { private set { type = value; } get { return type; } }
	[SerializeField]
	private float seconds;
	public float pushSeconds { private set { seconds = value; } get { return seconds; } }
	private float elapsed;
	public float progress { get { return Mathf.Clamp01(elapsed / pushSeconds); }}

	private void Start() {
	}

	public void Update() {
		//押された瞬間
		if (Input.GetButtonDown(type.GetInputName())) {
			this.elapsed = 0;
			OnLongPressBegin();
		}
		//押されている
		else if (Input.GetButton(type.GetInputName())) {
			this.elapsed += Time.deltaTime;

			if (elapsed > pushSeconds) {
				OnLongPressingOverTime(elapsed);
			}

			OnLongPressing(elapsed);
		}
		//離された瞬間
		else if (Input.GetButtonUp(type.GetInputName())) {
			if (elapsed > pushSeconds) { OnLongPressComplete(); }

			this.elapsed = 0;
			OnLongPressEnd();
		}
	}
}