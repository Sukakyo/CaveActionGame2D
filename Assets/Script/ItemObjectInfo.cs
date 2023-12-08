using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObjectInfo : DropObjectInfo
{
    [SerializeField]
    Item item;

    [SerializeField]
    int item_num;

    /*[SerializeField]
    bool go_to_player = false;*/

    public GameObject player;

    [SerializeField]
    float speed;

    [SerializeField]
    GameObject audioPrehab;

    

    protected override void PlayerTriggerMovement(Collider2D collision)
    {
        
        collision.GetComponent<ItemHolderInfo>().PlusItem(item.ItemIndex, player.GetComponent<TopDownCharacterController>().current_tool, item_num);
        Instantiate(audioPrehab);
        
        Destroy(gameObject);
    }

    public void Bake()
    {
        player = GameObject.Find("Player");
        GetComponent<SpriteRenderer>().sprite = item.ItemTexture;
    }

    public void Update()
    {
        GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * speed;
    }
}

