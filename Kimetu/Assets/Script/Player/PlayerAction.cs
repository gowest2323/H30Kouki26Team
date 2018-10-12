using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {
	private PlayerAnimation playerAnimation;
	private PlayerState state;
	private bool isGuard;
	private float counterTime;
	private float counterOccuredTime;
    //回避中か
    private bool isAvoid;
    //回避秒数(回避アニメーション移動部分の時間)
    private float avoidMoveTime;

	void Start() {
		this.playerAnimation = new PlayerAnimation(GetComponent<Animator>());
		this.isGuard = false;
		this.counterTime = -1;
		this.counterOccuredTime = -1;
		this.state = PlayerState.Idle;
        this.isAvoid = false;
        this.avoidMoveTime = 0.5f;
    }


	public void Move(Vector3 dir) {
		//こうしないとコントローラのスティックがニュートラルに戻った時、
		//勝手に前を向いてしまう
		if(dir == Vector3.zero) {
			playerAnimation.StopRunAnimation();
			return;
		}
		playerAnimation.StartRunAnimation();
		var pos = transform.position;
		//transform.position += dir * 10 * Slow.Instance.playerDeltaTime;
		transform.position += dir * 10 * Time.deltaTime;
		transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
	}

	/// <summary>
	/// 攻撃を開始します。
	/// </summary>
	public void Attack() {
		playerAnimation.StartAttackAnimation();
	}

	/// <summary>
	/// ガードを開始します。
	/// </summary>
	public void GuardStart() {
		this.isGuard = true;
		this.state = PlayerState.Defence;
		//TODO:ここでガードモーションに入る
	}

	/// <summary>
	/// ガードを終了します。
	/// </summary>
	public void GuardEnd() {
		this.isGuard = false;
		this.state = PlayerState.Idle;
	}

	/// <summary>
	/// 回避行動を実行します。
	/// 数秒後に自動で回避状態は解除されます。
	/// </summary>
	public void Avoid(Vector3 dir)
    {
		this.state = PlayerState.Avoid;

        //回避コルーチンを開始する
        StartCoroutine(AvoidCoroutine(dir));

        //回避行動中は他のアクションを実行できないように
        //PlayerControllerでisAvoidがtrueの時他のメソッドのUPDATEを停止
    }

    private IEnumerator AvoidCoroutine(Vector3 dir)
    {
        if (isAvoid) yield break;

        isAvoid = true;

        //何の方向もない時
        if (dir == Vector3.zero)
        {
            //後ろ回避アニメーション
            playerAnimation.StartBackAvoidAnimation();
            //後ろに移動
            for(float i = 0; i <= avoidMoveTime; i += Time.deltaTime)
            {
                transform.position += -transform.forward * 5 * Time.deltaTime;
                yield return null;
            }

            //TODO:ここに体制立ち直る隙間時間？

            isAvoid = false;
            yield break;
        }

        //方向入力がある時(四方向個別にアニメーションあり)、rotation維持
        //Dot()->同じ方向1、垂直0、正反対-1
        //前
        if(Vector3.Dot(transform.forward, dir) >= 0.3f)
        {
            //前進回避アニメーション
            playerAnimation.StartForwardAvoidAnimation();
        }
        //横
        else if(Vector3.Dot(transform.forward, dir) < 0.3f &&
                Vector3.Dot(transform.forward, dir) > -0.3f)
        {
            //右回避アニメーション
            if (dir.x > 0) playerAnimation.StartRightAvoidAnimation();
            //左回避アニメーション
            if (dir.x < 0) playerAnimation.StartLeftAvoidAnimation();
        }
        //後ろ
        else//Vector3.Dot(transform.forward, dir) <= -0.3f
        {
            //後ろ回避アニメーション
            playerAnimation.StartBackAvoidAnimation();
        }

        //向いている方向(正規化)に移動
        for (float i = 0; i <= avoidMoveTime; i += Time.deltaTime)
        {
            transform.position += dir.normalized * 5 * Time.deltaTime;
            yield return null;
        }

        //TODO:ここに体制立ち直る隙間時間？

        isAvoid = false;
        yield break;
    }

    public bool IsAvoid()
    {
        return isAvoid;
    }

    public void OnHit(Weapon weapon) {
		//TODO:ここでダメージアニメーションを開始する
		//TODO:HPを減らす
	}
}
