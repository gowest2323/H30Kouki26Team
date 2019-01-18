using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Status")]
public class StatusScriptableObject : ScriptableObject
{
    [SerializeField]
    protected int _maxHP;
    public int maxHP { get { return _maxHP; } }

}
