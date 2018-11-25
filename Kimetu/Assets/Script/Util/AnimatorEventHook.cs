using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using System.Linq;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

[DisallowMultipleComponent]
public class AnimatorEventHook : MonoBehaviour {
	#if UNITY_EDITOR

	[SerializeField]
	private AnimatorController controller;
	#endif

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#if UNITY_EDITOR
	public AnimatorController GetController() {
		return controller;
	}
	#endif
}
#if UNITY_EDITOR
[CustomEditor(typeof(AnimatorEventHook))]
public class AnimatorEventHookEditor : Editor {
	private static readonly string PATH = "Assets/Script/Events/"; //格納するパス
	private AnimatorEventHook self = null;
	private AnimatorController controller;

    void OnEnable () {
        this.self = (AnimatorEventHook) target;
		this.controller = self.GetController();
		if(!Directory.Exists(PATH)) {
			Directory.CreateDirectory(PATH);
		}
		CreateInterface();
    }

    public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		if(this.controller == null) {
			this.controller = self.GetController();
		}
		if(IsProgress()) {
			return;
		}
		var animator = self.gameObject.GetComponent<Animator>();
		EditorGUILayout.BeginVertical();
		foreach(var clip in GetAllClips()) {
			DoLayout(clip);
		}
		if(GUILayout.Button("CreateAll")) {
			CreateAllAsset();
		}
		if(GUILayout.Button("DeleteAll")) {
			DeleteAllAsset();
		}
		if(GUILayout.Button("Update")) {
			UpdateAllAsset();
		}
		EditorGUILayout.EndVertical();
    }

	private void DoLayout(AnimationClip clip) {
		var e = new AnimationEvent();
		var fn = GetHookFileName(clip.name);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(GetHookClassName(clip.name));
		if(File.Exists(fn)) {
			if(GUILayout.Button("Delete")) { DeleteAsset(clip, fn); }
		} else {
			if(GUILayout.Button("Create")) { CreateAsset(clip, fn); }
		}
		EditorGUILayout.EndHorizontal();
	}

	private bool IsProgress() {
 		return (EditorApplication.isPlaying) ||
            (Application.isPlaying) ||
            (EditorApplication.isCompiling);
	}

	private void CreateAllAsset() {
		foreach(var clip in GetAllClips()) {
			var fn = GetHookFileName(clip.name);
			if(File.Exists(fn)) continue;
			CreateNewHook(clip, fn);
		}
		AssetDatabase.Refresh();
	}

	private void CreateAsset(AnimationClip clip, string fn) {
		CreateNewHook(clip, fn);
		AssetDatabase.Refresh();
		var type = System.Type.GetType(GetHookClassName(clip.name));
		self.gameObject.AddComponent(type);
	}

	private void DeleteAllAsset() {
		foreach(var clip in GetAllClips()) {
			var fn = GetHookFileName(clip.name);
			if(!File.Exists(fn)) continue;
			File.Delete(fn);
		}
		AssetDatabase.Refresh();
	}

	private void DeleteAsset(AnimationClip clip, string fn) {
		File.Delete(fn);
		AssetDatabase.Refresh();
	}

	private void UpdateAllAsset() {
		foreach(var clip in GetAllClips()) {
			var fn = GetHookFileName(clip.name);
			UpdateAsset(clip, fn);
		}
		AssetDatabase.Refresh();
	}

	private void UpdateAsset(AnimationClip clip, string fn) {
		CreateInterface();
		CreateNewHook(clip, fn, false);
	}

	private void CreateNewHook(AnimationClip clip, string fn, bool updateEvent=true) {
		if(!Directory.Exists(PATH)) {
			Directory.CreateDirectory(PATH);
		}
		//クリア
		if(updateEvent) {
			AnimationUtility.SetAnimationEvents(clip, new AnimationEvent[0]);
		}
		//開始と終了でイベントを追加
		var startEvent = new AnimationEvent();
		var endEvent = new AnimationEvent();
		startEvent.functionName = GetAnimationName(clip.name) + "Start";
		endEvent.functionName = GetAnimationName(clip.name) + "End";
		startEvent.time = 0;
		endEvent.time = clip.length;
		if(updateEvent) {
			AnimationUtility.SetAnimationEvents(clip,
			new AnimationEvent[]{startEvent, endEvent});
		}
		//自動生成
		var src = CreateSourceCode(
			GetHookClassName(clip.name),
			startEvent.functionName,
			endEvent.functionName
		);
		File.WriteAllText(fn, src, Encoding.UTF8);
	}

	private string CreateSourceCode(
		string className,
		string startName,
		string endName) {
		var temp = GetTemplate();
		temp = temp.Replace("START_TEXT", Quote(startName));
		temp = temp.Replace("END_TEXT", Quote(endName));
		temp = temp.Replace("CLASSNAME", className);
		temp = temp.Replace("START", startName);
		temp = temp.Replace("END", endName);
		return temp;
	}

	private string Quote(string name) {
		return "\"" + name + "\"";
	}

	private void CreateInterface() {
		var path = PATH + "IEventHook.cs";
		if(!File.Exists(path)) {
			File.WriteAllText(path, GetInterface(), Encoding.UTF8);
		}
	}

	private string GetInterface() {
		return 
@"//AUTO-GENERATED-CODE
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public interface IEventHook {
	bool animationNow { get; }
	IObservable<bool> trigger { get; }
	IEnumerator Wait();
}";
	}

	private string GetTemplate() {
		return 
@"//AUTO-GENERATED-CODE
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

[DisallowMultipleComponent]
public class CLASSNAME : MonoBehaviour, IEventHook {
	public bool animationNow { private set; get; }
	public IObservable<bool> trigger { get { return subject; }}
	private Subject<bool> subject;

	void Awake() {
		this.subject = new Subject<bool>();
	}

	void Start() {
	}

	public void START() {
		Debug.Log(START_TEXT);
		this.animationNow = true;
		subject.OnNext(true);
	}

	public void END() {
		Debug.Log(END_TEXT);
		this.animationNow = false;
		subject.OnNext(false);
	}

	public IEnumerator Wait() {
		yield return new WaitWhile(() => !animationNow);
		yield return new WaitWhile(() => animationNow);
	}
}
";
	}

	private string GetHookFileName(string clipName) {
		return PATH + GetHookClassName(clipName) + ".cs";
	}

	private string GetHookClassName(string clipName) {
		return GetAnimationName(clipName) + "Hook";
	}

	private string GetAnimationName(string clipName) {
		var pos = clipName.IndexOf("@");
		var head = clipName.Substring(0, pos);
		var foot = clipName.Substring(pos + 1, clipName.Length - pos - 1);
		//アニメーション名 + Hook
		return UpperCase(head) + UpperCase(foot);
	}

	private string UpperCase(string name) {
		var car = char.ToUpper(name[0]);
		var cdr = name.Substring(1, name.Length - 1);
		return car + cdr;
	}

	public List<AnimatorControllerParameter> GetAllParameters() {
		if(controller == null) { return new List<AnimatorControllerParameter>(); }
		return controller.parameters.ToList();
	}

	public List<AnimationClip> GetAllClips() {
		if(controller == null) { return new List<AnimationClip>(); }
		return controller.animationClips.ToList();
	}
}
#endif