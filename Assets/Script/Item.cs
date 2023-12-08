using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    private string item_name;

    [SerializeField]
    private Sprite item_texture;
    public Sprite ItemTexture { get { return item_texture; } }

    [SerializeField]
    private int item_index;
    public int ItemIndex { get { return item_index; } }
}
