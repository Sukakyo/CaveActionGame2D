using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Serialization;

public class BoxInfo : TouchObjectInfo
{
    public enum BoxType
    {
        item,
        tool,
        heal
    }

    
    public BoxType boxType = BoxType.item;


    [System.Serializable]
    public class ItemInBox
    {

        [SerializeField]
        ItemStorage itemStorage;
        public ItemStorage ItemStorageGet { get { return itemStorage; } set { itemStorage = value; } }
    }

    [System.Serializable]
    public class ToolInBox
    {
        [SerializeField]
        Tool tool;

        public Tool ToolGet { get { return tool; } set { tool = value; } }
    }

    [HideInInspector]
    public ItemInBox itemInBox = new ItemInBox();

    [HideInInspector]
    public ToolInBox toolInBox = new ToolInBox();

    [HideInInspector]
    public int healNum;

    [HideInInspector]
    public Sprite healSprite;

    [FormerlySerializedAs("sprite_num")]
    [SerializeField]
    int spriteNum = 0;

    [FormerlySerializedAs("sprites")]
    [SerializeField]
    Sprite[] _sprites;

    Animator _animator;

    [FormerlySerializedAs("rocket")]
    [SerializeField]
    GameObject _rocket;

    private bool openedSwitch = false;
    public bool OpenedSwitch { get { return openedSwitch; } }

    
    private GameObject _touchPlayerHolder;
    
    private GameObject _player;

    public override void CollisionEnterEvent(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if (boxType == BoxType.item)
        {
            _touchPlayerHolder = collision.gameObject.transform.Find("ItemHolder").gameObject;
            _player = collision.gameObject;
            
        }
        else if(boxType == BoxType.tool)
        {
            _touchPlayerHolder = collision.gameObject.transform.Find("ToolHolder").Find("Tool").gameObject;
            _player = collision.gameObject;
        }
        else if(boxType == BoxType.heal)
        {
            _touchPlayerHolder = collision.gameObject.transform.Find("HP").gameObject;
            _player = collision.gameObject;
        }
    }

    public override void CollisionExitEvent(Collider2D collision)
    {
        _touchPlayerHolder = null;
        _player = null;
    }


    public override void FireEvent()
    {
        if (touch_switch 
            && !openedSwitch)
        {
            if (boxType == BoxType.item)
            {
                int index = itemInBox.ItemStorageGet.It.ItemIndex;
                int num = itemInBox.ItemStorageGet.ItemNum;
                _touchPlayerHolder.GetComponent<ItemHolderInfo>().PlusItem(index, _player.GetComponent<TopDownCharacterController>().current_tool, num);
                
            }
            else if (boxType == BoxType.tool)
            {
                int index = toolInBox.ToolGet.ToolIndex;
                ToolSet toolSet = _touchPlayerHolder.GetComponent<ToolInfo>().Tools[index];
                _player.GetComponent<AudioSource>().Play();
                if (toolSet.tool_on_off == false)
                {
                    toolSet.tool_on_off = true;
                }
                _touchPlayerHolder.GetComponent<ToolInfo>().Bake();
                gameManagement.TalkStart(toolSet.ToolGet.textAsset);
                
                _player.GetComponent<Animator>().SetBool("IsGetTool", true);
                _player.GetComponent<TopDownCharacterController>().current_tool = toolSet.ToolGet.ToolIndex;
                
            }
            else if(boxType == BoxType.heal)
            {
                HPInfo hpInfo = _touchPlayerHolder.GetComponent<HPInfo>();
                if(hpInfo.Hp + healNum <= hpInfo.max_hp)
                {
                    hpInfo.Hp += healNum;
                }
                else
                {
                    hpInfo.Hp = hpInfo.max_hp;
                }
                ((PlayerHPInfo)hpInfo).UpdateHP();
            }
            OpenBox();
            openedSwitch = true;
        }
    }

    public void Bake()
    {
        gameManagement = GameObject.Find("GameManager").GetComponent<GameManagement>();

        
    }

    protected void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenBox()
    {
        if (!openedSwitch) 
        {
            _animator.SetBool("opened_check", true);
            GameObject rocketIns = Instantiate(_rocket,this.transform.position,Quaternion.identity);
            if (boxType == BoxType.item)
            {
                rocketIns.GetComponent<SpriteRenderer>().sprite = itemInBox.ItemStorageGet.It.ItemTexture;
            }
            else if (boxType == BoxType.tool)
            {
                rocketIns.GetComponent<SpriteRenderer>().sprite = toolInBox.ToolGet.ToolTexture;
            }
            else if (boxType == BoxType.heal)
            {
                rocketIns.GetComponent<SpriteRenderer>().sprite = healSprite;
            }
            rocketIns.transform.position = this.gameObject.transform.position;

            openedSwitch = true;
        }
    }

    private void Update()
    {
        
        this.gameObject.GetComponent<SpriteRenderer>().sprite = _sprites[spriteNum];

    }

    

}
