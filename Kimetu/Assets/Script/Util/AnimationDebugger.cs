using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

/// <summary>
/// アニメーションに与える引数の型。
/// </summary>
public enum AnimationArgumentKind {
	Int,
	Float,
	Bool,
	Trigger,
}

/// <summary>
/// アニメーションを起動するための引数。
/// </summary>
[System.Serializable]
public struct AnimationArgument {
	public string name;
	public AnimationArgumentKind kind;
	public int intValue;
	public float floatValue;
	public bool boolValue;

	public void Apply(Animator animator) {
		switch (kind) {
			case AnimationArgumentKind.Int:
				animator.SetInteger(name, intValue);
				break;

			case AnimationArgumentKind.Float:
				animator.SetFloat(name, floatValue);
				break;

			case AnimationArgumentKind.Bool:
				animator.SetBool(name, boolValue);
				break;

			case AnimationArgumentKind.Trigger:
				animator.SetTrigger(name);
				break;
		}
	}
}

/// <summary>
/// アニメーションを確認するためのデバッガ。
/// </summary>
public class AnimationDebugger : MonoBehaviour {
	[SerializeField]
	private Animator animator;
	#if UNITY_EDITOR

	[SerializeField]
	private AnimatorController controller;
	#endif
	[SerializeField]
	private AnimationArgument argument;
	[SerializeField]
	private bool isLoop;
	[SerializeField]
	private float speed = 1f;

	private string current;


	// Use this for initialization
	void Start () {
		if (this.animator == null) {
			this.animator = GetComponent<Animator>();
		}

		#if UNITY_EDITOR
		Apply();
		#endif
		//AnimatorController animatorController = AnimatorController.GetEffectiveAnimatorController(animator);
	}

	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		animator.speed = speed;

		if ((isLoop && IsFinished()) || current != argument.name) {
			Apply();
		}

		#endif
	}

	private void Apply() {
		this.current = argument.name;
		argument.Apply(animator);
	}

	private bool IsFinished() {
		//CharacterAnimationより
		AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
		return animatorInfo.normalizedTime > 1.0f - Mathf.Epsilon;
	}

	public AnimationArgument GetArgument() {
		return argument;
	}
	#if UNITY_EDITOR
	public List<AnimatorControllerParameter> GetAllParameters() {
		if (controller == null) { return new List<AnimatorControllerParameter>(); }

		return controller.parameters.ToList();
	}

	public List<AnimationClip> GetAllClips() {
		if (controller == null) { return new List<AnimationClip>(); }

		return controller.animationClips.ToList();
	}
	#endif
}
#if UNITY_EDITOR
[CustomEditor (typeof(AnimationDebugger))]
public class AnimationDebuggerEditor : Editor　 {
	private AnimationDebugger self = null;

	void OnEnable () {
		this.self = (AnimationDebugger) target;
	}

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		var parameters = self.GetAllParameters();
		var arg = self.GetArgument();

		if (parameters.Count == 0) {
			EditorGUILayout.LabelField("Controllerを設定してください。");
			return;
		}

		if (!parameters.Any((e) => e.name == arg.name)) {
			EditorGUILayout.LabelField(string.Format("({0})は存在しません。", arg.name));
		}

		var parameterStr = parameters
						   .Select((e) => e.name + "(" + e.type + ")")
						   .Aggregate((a, b) => a + "\n" + b);
		parameterStr = string.Format("使用可能なパラメーター\n{0}", parameterStr);
		EditorGUILayout.TextArea(parameterStr);
	}
}
#endif