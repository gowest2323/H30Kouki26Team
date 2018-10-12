using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 public class StageManager : MonoBehaviour {
    private Vector3 restartPosition;
     public void Pass(Vector3 position)
    {
        restartPosition = position;
    }
     public Vector3 RestartPosition()
    {
        return restartPosition;
    }
}