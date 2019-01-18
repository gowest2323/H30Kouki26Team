using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/PlayerStatus")]
public class PlayerScriptableObject : StatusScriptableObject
{
    [SerializeField]
    private int _maxStamina;
    public int maxStamina { get { return _maxStamina; } }
}
