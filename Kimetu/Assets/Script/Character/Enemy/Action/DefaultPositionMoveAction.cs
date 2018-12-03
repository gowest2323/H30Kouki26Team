using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPositionMoveAction : MoveAction
{
    protected override void Start()
    {
        base.Start();
        movePosition = this.transform.position;
        moveRotation = this.transform.rotation;
    }
}
