using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : SingletonMonoBehaviour<CoroutineManager>
{
    /// <summary>
    /// IEnumeratorとCoroutineのペア
    /// </summary>
    public class IEnumCorPair
    {
        public IEnumerator enumerator;
        public Coroutine co;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="coroutine"></param>
        public IEnumCorPair(IEnumerator enumerator, Coroutine coroutine)
        {
            this.enumerator = enumerator;
            this.co = coroutine;
        }
    }
    private List<IEnumCorPair> coroutines = new List<IEnumCorPair>(); //コルーチンリスト

    /// <summary>
    /// コルーチンの開始
    /// </summary>
    /// <param name="enumerator">開始するコルーチンのIEnumerator</param>
    /// <returns>開始されたコルーチン</returns>
    public Coroutine StartCoroutineEx(IEnumerator enumerator)
    {
        Debug.Log("coroutine start");
        //コルーチンとIEnumeratorのペアを生成
        IEnumCorPair pair = new IEnumCorPair(enumerator, StartCoroutine(enumerator));
        coroutines.Add(pair);
        return pair.co;
    }

    /// <summary>
    /// コルーチンの停止
    /// </summary>
    /// <param name="coroutine">停止させるコルーチン</param>
    public void StopCoroutineEx(Coroutine coroutine)
    {
        Debug.Log("coroutine stop");
        if (coroutine == null) return;
        StopCoroutine(coroutine);
    }

    /// <summary>
    /// すべてのコルーチンの停止
    /// </summary>
    /// <param name="destroy">コルーチンを完全に停止させるならtrue</param>
    public void StopAllCoroutineEx(bool destroy)
    {
        Debug.Log("all coroutine stop");

        foreach (var cor in coroutines)
        {
            StopCoroutineEx(cor.co);
        }
        if (destroy)
        {
            coroutines.Clear();
        }
    }

    /// <summary>
    /// コルーチンの再開
    /// </summary>
    public void StartAllCoroutineEx()
    {
        Debug.Log("all coroutine start");
        for (int i = 0; i < coroutines.Count; i++)
        {
            coroutines[i].co = StartCoroutine(coroutines[i].enumerator);
        }
    }
}
