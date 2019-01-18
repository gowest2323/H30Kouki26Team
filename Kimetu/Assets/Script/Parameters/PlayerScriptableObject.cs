using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/PlayerStatus")]
public class PlayerScriptableObject : StatusScriptableObject {
	[SerializeField]
	private int _maxStamina;
	public int maxStamina { get { return _maxStamina; } }
	[SerializeField]
	private int _normalAttackPower;
	public int normalAttackPower { get { return _normalAttackPower; } }
	[SerializeField]
	private int _kirinukePower;
	public int kirinukePower { get { return _kirinukePower; } }
}
