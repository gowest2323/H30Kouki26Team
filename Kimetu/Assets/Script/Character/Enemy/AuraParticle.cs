using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AuraParticle : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem auraParticle;
    [SerializeField]
    protected string auraPositionName = "mixamorig:Neck";
    protected EnemyStatus enemyStatus;

    protected virtual void Start()
    {
        SetAura();
        enemyStatus = GetComponentInParent<EnemyStatus>();
        //スロー開始時、終了時にパーティクルの再生、停止処理
        Slow.Instance.onStart.TakeUntilDestroy(this).Subscribe(_ => auraParticle.Stop());
        Slow.Instance.onEnd.TakeUntilDestroy(this).Subscribe(_ => auraParticle.Play());
    }

    private void SetAura()
    {
        //オーラ発生場所を探してその場所に生成
        Transform auraPosition = transform.FindRec(auraPositionName).transform;
        auraParticle = GameObject.Instantiate(auraParticle, auraPosition);
        auraParticle.Play();
    }

    protected virtual void Update()
    {
        //死亡したら再生終了
        if (enemyStatus.IsDead())
        {
            if (auraParticle.isPlaying)
            {
                auraParticle.Stop();
            }
            return;
        }
        PauseManager instance = PauseManager.GetInstance();
        //ポーズ中かつパーティクル再生中なら停止させる
        if (instance.isPause && auraParticle.isPlaying)
        {
            auraParticle.Stop();
        }
        //ポーズから戻ったら生成再開
        else if (instance.isReturnFromPause)
        {
            auraParticle.Play();
        }
    }
}
