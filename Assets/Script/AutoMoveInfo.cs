using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AutoMoveInfo : MonoBehaviour
{
    [SerializeField]
    Transform parentTransform;

    PlayerMoveInfo _moveInfo;
    PlayerTurnInfo _turnInfo;


    
    [SerializeField]
    Vector2[] positionList;



    int count = 0;

    private bool _runAuto = false;

    
    [SerializeField]
    private bool autoContinue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RunAuto();
    }


    private void Start()
    {
        _moveInfo = parentTransform.Find("PlayerMove").GetComponent<PlayerMoveInfo>();
        _turnInfo = parentTransform.Find("PlayerTurn").GetComponent<PlayerTurnInfo>();
    }

    private void Update()
    {
        if (_runAuto)
        {
            if (count < positionList.Length)
            {
                Vector2 current_position = parentTransform.position;
                Vector2 dir = (positionList[count] - current_position).normalized;
                _moveInfo.SetMove(dir);
                _turnInfo.SetTurn(dir);

                if (Vector2.Distance(parentTransform.position, positionList[count]) < 0.1f)
                {
                    _moveInfo.SetMove(Vector2.zero);
                    count++;
                }
            }
            else
            {
                if (!autoContinue)
                {
                    _runAuto = false;
                    _moveInfo.auto_move = false;
                    _turnInfo.auto = false;
                    parentTransform.GetComponent<Animator>().SetBool("IsAuto", false);
                }
            }
        }
    }


    public void RunAuto()
    {
        _runAuto = true;
        _moveInfo.auto_move = true;
        _turnInfo.auto = true;
        parentTransform.GetComponent<Animator>().SetBool("IsAuto",true);
    }
}
