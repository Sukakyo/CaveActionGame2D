using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandrailTriggerInfo : MonoBehaviour
{
    enum TriggerType
    {
        start,
        end
    }

    [SerializeField]
    TriggerType triggerType;

    [SerializeField]
    HandrailInfo parent;

    [SerializeField]
    GameManagement gameManagement;

    private void Awake()
    {
        try
        {
            gameManagement = GameObject.Find("GameManager").GetComponent<GameManagement>();
        }
        catch
        {
            gameManagement = null;
        }
    }

    public void Bake(int num)
    {
        parent = GetComponentInParent<HandrailInfo>();
        

        if(num == 0)
        {
            triggerType = TriggerType.start;
        }
        else
        {
            triggerType = TriggerType.end;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(triggerType == TriggerType.start)
            {
                gameManagement.touch_object = parent.gameObject;
                gameManagement.playerMode = GameManagement.PlayerMode.touch;
                parent.SetTouchSwitch(true);
            }
            else if(triggerType == TriggerType.end)
            {
                gameManagement.touch_object = null;
                gameManagement.playerMode = GameManagement.PlayerMode.none;
                parent.SetTouchSwitch(false);
                parent.Cancel();
            }
        }
    }
}
