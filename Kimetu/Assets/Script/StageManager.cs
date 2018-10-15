using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 public class StageManager : MonoBehaviour {
    private Vector3 restartPosition;//
    [SerializeField]
    private EnemySpawnerManager manager;
     public void Pass(Vector3 position)
    {
        restartPosition = position;
    }
     public Vector3 RestartPosition()
    {
        manager.Init();
        return restartPosition;
    }
}