using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class TouchObjectInfo : MonoBehaviour
{
    [SerializeField]
    protected GameManagement gameManagement;

    [SerializeField]
    protected bool touch_switch;

    protected virtual void Awake()
    {
        GameObject gb = new GameObject(gameObject.name + " Collider");
        gb.transform.parent = transform;
        gb.transform.position = transform.position;
        gb.AddComponent<BoxCollider2D>();
        BoxCollider2D bc2d_child = gb.GetComponent<BoxCollider2D>();
        BoxCollider2D bc2d_this = GetComponent<BoxCollider2D>();
        bc2d_child.offset = bc2d_this.offset;
        bc2d_child.size = bc2d_this.size;
        bc2d_child.isTrigger = false;
        gb.layer = LayerMask.NameToLayer("Invisible Wall Layer");
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            gameManagement.playerMode = GameManagement.PlayerMode.touch;
            gameManagement.touch_object = this.gameObject;
            CollisionEnterEvent(collision);
            touch_switch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManagement.playerMode = GameManagement.PlayerMode.none;
            gameManagement.touch_object = null;
            CollisionExitEvent(collision);
            touch_switch = false;
        }
    }

    public abstract void FireEvent();

    public abstract void CollisionEnterEvent(Collider2D collision);

    public abstract void CollisionExitEvent(Collider2D collision);

}
