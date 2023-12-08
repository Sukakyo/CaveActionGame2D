using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoMoveInfo : MoveInfo
{
    [SerializeField]
    Vector2 direction;

    protected override float ToVertical()
    {
        return direction.y;
    }

    protected override float ToHorizontal()
    {
        return direction.x;
    }

    
}
