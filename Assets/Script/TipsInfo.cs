using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TipsInfo : TouchObjectInfo
{
    [SerializeField]
    private TextAsset textAsset;

    public TextAsset TextAssetGet { get { return textAsset; } }
    
    public override void CollisionEnterEvent(Collider2D collision)
    {
        gameManagement.touch_object = this.gameObject;
    }

    public override void CollisionExitEvent(Collider2D collision)
    {
        
    }

    public override void FireEvent()
    {
        if (touch_switch)
        {
            
                if (gameManagement.state == GameManagement.GameState.play)
                {
                    
                    gameManagement.TalkStart(this.textAsset);
                    
                }
            
        }
    }

}
