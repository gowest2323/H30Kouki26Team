using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStatusScriptableObject : StatusScriptableObject {
	public abstract int GetPower(EnemyAttackType type);
}
