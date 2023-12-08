using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolObjectInfo : DropObjectInfo
{
    private GameManagement _gameManagement;

    [SerializeField]
    Tool tool;

    private void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tool.ToolTexture;
        _gameManagement = GameObject.Find("GameManager").GetComponent<GameManagement>();
    }

    protected override void PlayerTriggerMovement(Collider2D collision)
    {
        
            ToolSet toolset = collision.transform.Find("ToolHolder").Find("Tool").GetComponent<ToolInfo>().Tools[tool.ToolIndex];
            Debug.Log(toolset);
            collision.GetComponent<AudioSource>().Play();
            if (toolset.tool_on_off == false)
            {
                toolset.tool_on_off = true;
            }
            Destroy(this.gameObject);
            _gameManagement.TalkStart(tool.textAsset);
            
            collision.GetComponent<Animator>().SetBool("IsGetTool", true);
            collision.GetComponent<TopDownCharacterController>().current_tool = tool.ToolIndex;
        
    }
}
