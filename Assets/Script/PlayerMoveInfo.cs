using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Cainos.PixelArtTopDown_Basic;

public class PlayerMoveInfo : MoveInfo
{
    public bool auto_move = false;


    private TopDownCharacterController playerController;
    //private TopDownCharacterController.State state;



    private void Awake()
    {
        playerController = this.transform.parent.GetComponent<TopDownCharacterController>();
    }

    


    public void OnMove(InputAction.CallbackContext context)
    {
        if(playerController != null)
        {
            if (playerController.state != TopDownCharacterController.State.start)
            {
                if (!auto_move)
                {
                    Debug.Log(context.ReadValue<Vector2>());
                    move_dir = context.ReadValue<Vector2>();
                    if (move_dir.magnitude > 0)
                    {
                        move_dir = move_dir.normalized;
                    }
                }
            }
        }
        


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
   
}
