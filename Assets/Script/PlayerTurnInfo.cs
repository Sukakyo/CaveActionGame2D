using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurnInfo : TurnInfo
{
    public bool auto = false;

    private TopDownCharacterController _playerController;
    //private TopDownCharacterController.State state;


    private void Awake()
    {
        _playerController = this.transform.parent.GetComponent<TopDownCharacterController>();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (_playerController != null)
        {
            if (_playerController.state != TopDownCharacterController.State.start)
            {
                if (!auto)
                {
                    //Debug.Log(context.ReadValue<Vector2>());
                    move_dir = context.ReadValue<Vector2>();
                }
            }
        }
    }

    protected override bool ToFront()
    {
        //return Input.GetKey(KeyCode.S);
        return move_dir.y < 0;
    }
    protected override bool ToLeft()
    {
        //return Input.GetKey(KeyCode.A);
        return move_dir.x < 0;
    }
    protected override bool ToRight()
    {
        //return Input.GetKey(KeyCode.D);
        return move_dir.x > 0;
    }
    protected override bool ToBack()
    {
        //return Input.GetKey(KeyCode.W);
        return move_dir.y > 0;
    }
}
