using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoTurnInfo : TurnInfo
{
    [SerializeField]
    Vector2 direction;

    protected override bool ToFront()
    {
        return direction.y < 0;
    }

    protected override bool ToLeft()
    {
        return direction.x < 0;
    }

    protected override bool ToRight()
    {
        return direction.x > 0;
    }

    protected override bool ToBack()
    {
        return direction.y > 0;
    }
}
