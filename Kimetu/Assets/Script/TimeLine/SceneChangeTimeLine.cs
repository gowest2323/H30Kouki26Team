using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class SceneChangePlayableAsset : PlayableAsset
{
    [SerializeField]
    private SceneName nextScene;
    [SerializeField]
    private FadeData sceneFade;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        SceneChangePlayableBehaviour behaviour = new SceneChangePlayableBehaviour();
        behaviour.nextScene = nextScene;
        behaviour.sceneFade = sceneFade;
        return ScriptPlayable<SceneChangePlayableBehaviour>.Create(graph, behaviour);
    }
}

public class SceneChangePlayableBehaviour : PlayableBehaviour
{
    public SceneName nextScene;
    public FadeData sceneFade;

    // タイムライン開始時実行
    public override void OnGraphStart(Playable playable)
    {
    }

    // タイムライン停止時実行
    public override void OnGraphStop(Playable playable)
    {
    }

    // PlayableTrack再生時実行
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (Application.isPlaying)
        {
            SceneChanger.Instance().Change(nextScene, sceneFade);
        }
    }
    // PlayableTrack停止時実行
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {

    }
    // PlayableTrack再生時毎フレーム実行
    public override void PrepareFrame(Playable playable, FrameData info)
    {
    }

}
