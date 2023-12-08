using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveInfo : MoveInfo
{
    [SerializeField]
    GameObject ai;

    const float MaxMoveNum = 0.3f;


    protected override float ToVertical()
    {
        return ai.GetComponent<EnemyAIInfo>().Dir.y;
    }

    protected override float ToHorizontal()
    {
        return ai.GetComponent<EnemyAIInfo>().Dir.x;
    }

}
