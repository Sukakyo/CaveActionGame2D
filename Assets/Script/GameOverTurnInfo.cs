using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTurnInfo : TurnInfo
{
    [SerializeField]
    private Vector2 auto_dir;

    protected override bool ToFront()
    {
        //return Input.GetKey(KeyCode.S);
        return auto_dir.y < 0;
    }
    protected override bool ToLeft()
    {
        //return Input.GetKey(KeyCode.A);
        return auto_dir.x < 0;
    }
    protected override bool ToRight()
    {
        //return Input.GetKey(KeyCode.D);
        return auto_dir.x > 0;
    }
    protected override bool ToBack()
    {
        //return Input.GetKey(KeyCode.W);
        return auto_dir.y > 0;
    }

}
