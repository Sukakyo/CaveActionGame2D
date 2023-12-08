using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine.Tilemaps;
using System;
using System.Linq;
//using System.Numerics;

public class HandrailInfo : TouchObjectInfo
{
    [SerializeField]
    CameraFollow cameraFollow;

    [SerializeField]
    Vector3 plusPos;

    [SerializeField]
    bool isHorizontal;

    [SerializeField]
    int size;

    [SerializeField]
    float[] offset;

    [SerializeField]
    Tile handrailTile;

    private Tilemap _tilemap;

    private BoxCollider2D _boxCollider;
    private EdgeCollider2D[] _startEdgeCollider;
    private EdgeCollider2D[] _endEdgeCollider;

    [SerializeField]
    string layerName;

    enum Direction
    {
        front,
        right,
        back,
        left
    }

    [SerializeField]
    Direction direction;


    


    protected override void Awake()
    {
        
    }

    public void Bake()
    {
        _tilemap = this.transform.Find("Tilemap").GetComponent<Tilemap>();
        _boxCollider = this.transform.Find("Tilemap").GetComponent<BoxCollider2D>();
        _startEdgeCollider = this.transform.Find("StartChangeTrigger").GetComponents<EdgeCollider2D>();
        _endEdgeCollider = this.transform.Find("EndChangeTrigger").GetComponents<EdgeCollider2D>();


        _tilemap.ClearAllTiles();
        Tile[] tiles = new Tile[size];
        Array.Fill<Tile>(tiles, handrailTile);
        UnityEngine.Vector3Int[] pos = new UnityEngine.Vector3Int[size];
        for (int i = 0; i < size; i++)
        {
            pos[i] = Vector3Int.RoundToInt(UnityEngine.Quaternion.Euler(0, 0, 90 * (int)direction) * (new UnityEngine.Vector3(0, -1, 0) * i ));
        }
         
        _tilemap.SetTiles(pos, tiles);

        _boxCollider.offset = new Vector2( size / (float) 2, _boxCollider.offset.y);
        _boxCollider.size = new Vector2(size,_boxCollider.size.y);

        float[] shift = new float[2] {-0.2f,0.2f };
        
        for(int i = 0; i < _startEdgeCollider.Length;i++) {
            _startEdgeCollider[i].points = new Vector2[2] { new Vector2(0, offset[i]), new Vector2(size, offset[i]) };
        }
        
        for(int i = 0; i < _endEdgeCollider.Length;i++)
        {
            _endEdgeCollider[i].points = new Vector2[2] { new Vector2(0, offset[i] + shift[i]), new Vector2(size, offset[i] + shift[i]) };
        }

        this.transform.Find("StartChangeTrigger").GetComponent<HandrailTriggerInfo>().Bake(0);
        this.transform.Find("EndChangeTrigger").GetComponent<HandrailTriggerInfo>().Bake(1);

        this.gameObject.layer = LayerMask.NameToLayer(layerName);
        foreach(Transform t in this.transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer(layerName);
        }


    }

    public void SetTouchSwitch(bool bit)
    {
        touch_switch = bit;
    }
    

    public override void FireEvent()
    {
        if (touch_switch)
        {
            if (cameraFollow.plusPos.Equals(Vector3.zero))
            {
                cameraFollow.PlusPosition(plusPos);
            }
            else
            {
                cameraFollow.PlusPosition(Vector3.zero);
            }
        }
    }

    public void Cancel()
    {
        cameraFollow.PlusPosition(Vector3.zero);
    }

    public override void CollisionEnterEvent(Collider2D collision)
    {
        
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 0)
        {
            if (isHorizontal)
            {
                float distance = collision.gameObject.transform.position.y - this.gameObject.transform.position.y;
                if(distance > 0)
                {
                    collision.gameObject.transform.position += new Vector3(0,0.01f,0);
                }
                else
                {
                    collision.gameObject.transform.position += new Vector3(0, -0.01f, 0);
                }
            }
            else
            {
                float distance = collision.gameObject.transform.position.x - this.gameObject.transform.position.x;
                if (distance > 0)
                {
                    collision.gameObject.transform.position += new Vector3(0.01f, 0, 0);
                }
                else
                {
                    collision.gameObject.transform.position += new Vector3(-0.01f, 0, 0);
                }
            }
        }*/
    }

    public override void CollisionExitEvent(Collider2D collision)
    {
        //camera.PlusPosition(Vector3.zero);
    }
    
}
