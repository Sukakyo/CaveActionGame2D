using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnInfo : TurnInfo
{
    [SerializeField]
    GameObject ai;

    const float MaxMoveNum = 0.3f;


    protected override bool ToFront()
    {
        return ai.GetComponent<EnemyAIInfo>().Dir.y < -MaxMoveNum;
    }

    protected override bool ToLeft()
    {
        return ai.GetComponent<EnemyAIInfo>().Dir.x < -MaxMoveNum;
    }

    protected override bool ToRight()
    {
        return ai.GetComponent<EnemyAIInfo>().Dir.x > MaxMoveNum;
    }

    protected override bool ToBack()
    {
        return ai.GetComponent<EnemyAIInfo>().Dir.y > MaxMoveNum;
    }
}
