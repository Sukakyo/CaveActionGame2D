using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class BossMoveInfo : MoveInfo
{
    //[SerializeField]
    //private Vector2 goToPosition;

    [FormerlySerializedAs("positionList")]
    [SerializeField]
    private Vector2[] _positionList = new Vector2[4];

    private int currentPosition = 0;
    public int CurrentPosition { get { return currentPosition; } }

    [FormerlySerializedAs("parentTransform")]
    [SerializeField]
    Transform _parentTransform;

    [FormerlySerializedAs("target")]
    [SerializeField]
    Transform _target;

    Vector2 _destination;

    

    private int count = 0;

    public bool goToCheck = false;

    public bool stopCheck = false;
    private float time = 0f;
    private float stopTime = 0.5f;

    public bool stopCheckLong = false;

    

    protected override void Update()
    {
        base.Update();

        if (stopCheck)
        {
            if (time > stopTime)
            {
                stopCheck = false;
                time = 0;

            }
            else
            {
                time += Time.deltaTime;
            }
        }


        if (!stopCheck && !stopCheckLong)
        {
            if (count == 0)
            {
                dir = Vector2.zero;
            }
            else if (count == 1)
            {
                if (Vector2.Distance(_destination, _parentTransform.position) < 1f)
                {
                    count = 2;
                    if (currentPosition % 2 == 0)
                    {
                        _destination = new Vector2(_target.position.x, _positionList[currentPosition].y);
                    }
                    else
                    {
                        _destination = new Vector2(_positionList[currentPosition].x, _target.position.y);
                    }
                }
                else
                {
                    if (currentPosition % 2 == 0)
                    {
                        _destination = new Vector2(_parentTransform.position.x, _positionList[currentPosition].y);
                    }
                    else
                    {
                        _destination = new Vector2(_positionList[currentPosition].x, _parentTransform.position.y);
                    }
                }
                dir = (_destination - new Vector2(_parentTransform.position.x, _parentTransform.position.y)).normalized;
            }
            else
            {
                if (currentPosition % 2 == 0)
                {
                    _destination = new Vector2(_target.position.x, _positionList[currentPosition].y);
                }
                else
                {
                    _destination = new Vector2(_positionList[currentPosition].x, _target.position.y);
                }

                if (Vector2.Distance(_destination, _parentTransform.position) < 1f)
                {
                    goToCheck = false;
                    count++;
                    count %= 3;
                }
                dir = (_destination - new Vector2(_parentTransform.position.x, _parentTransform.position.y)).normalized;
            }
        }
        else
        {
            dir = Vector2.zero;
        }
        //Debug.Log(parentTransform.position);
        //Debug.Log(destination);
    }



    protected override float ToHorizontal()
    {
        //return Input.GetKey(KeyCode.S);
        return move_dir.x;
    }
    protected override float ToVertical()
    {
        return move_dir.y;
    }




    // n‚Í0,1
    public void ChangeGoTo(int n)
    {
        int num = n;
        if(currentPosition%2==0)
        {
            num = num * 2 + 1;
        }
        else
        {
            num = num * 2;
        }

        currentPosition = num;
        count = 1;
        if(currentPosition % 2 == 0)
        {
            _destination = new Vector2(_parentTransform.position.x, _positionList[currentPosition].y);
        }
        else
        {
            _destination = new Vector2(_positionList[currentPosition].x, _parentTransform.position.y);
        }

        goToCheck = true;

    }
    //  0
    //3   1
    //  2
    
}
