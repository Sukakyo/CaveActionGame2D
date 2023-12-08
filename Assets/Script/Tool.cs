using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create Tool")]
public class Tool : ScriptableObject
{
    [SerializeField]
    private string tool_name;

    [SerializeField]
    private Sprite tool_texture;
    public Sprite ToolTexture { get { return tool_texture; } }

    [SerializeField]
    private int tool_index;
    public int ToolIndex { get { return tool_index; } }

    [SerializeField]
    private int useItemIndex;

    public int UseItemIndex { get { return useItemIndex; } }

    [SerializeField]
    private int useItemNum;

    public int UseItemNum { get { return useItemNum; } }

    public TextAsset textAsset;

    [System.Serializable]
    public class UseSprites
    {
        public Sprite[] frontSprites = new Sprite[3];
        public Sprite[] leftSprites = new Sprite[3];
        public Sprite[] rightSprites = new Sprite[3];
        public Sprite[] backSprites = new Sprite[3];
    }

    public UseSprites useSprites = new UseSprites();
    

}
