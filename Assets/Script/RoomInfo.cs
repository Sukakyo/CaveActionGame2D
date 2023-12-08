using Cainos.PixelArtTopDown_Basic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomInfo : MonoBehaviour
{
    [SerializeField]
    Tilemap sort0;

    [SerializeField]
    private NavMeshSourceTag2D sort0_nav;

    [SerializeField] 
    Tilemap sort1;

    [SerializeField]
    List<GameObject> sort2;
    /*[SerializeField]
    List<SpriteRenderer> sort2_clear;*/

    [SerializeField] 
    Tilemap sort4;

    [SerializeField]
    ShadowManageInfo shadowManage;


    [SerializeField]
    GameObject invisibleWall;
    [SerializeField]
    GameObject damageObjectWall;
    

    

    /*[SerializeField]
    bool is_on_room;*/

    [SerializeField]
    public List<RoomInfo> nextRooms = new List<RoomInfo>();
    [SerializeField]
    public List<RoomInfo> upRooms = new List<RoomInfo>();
    [SerializeField]
    public List<RoomInfo> generateRooms = new List<RoomInfo>();


    private void Awake()
    {
        sort0_nav = sort0.GetComponent<NavMeshSourceTag2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*if (is_on_room)
        {
            RoomOn();
        }
        else
        {
            RoomOff();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomOn()
    {
        
        sort0.color = Color.white;
        sort0_nav.enabled = true;
        sort1.color = Color.white;
        sort4.color = Color.white;
        sort2.RemoveAll(x => x == null);
        //sort2_clear.RemoveAll(x => x == null);

        foreach (GameObject gb in sort2)
        {

            if (gb != null)
            {

                if (gb.tag.Equals("Rock"))
                {
                    gb.SetActive(true);
                    //gb.GetComponent<BoxCollider2D>().enabled = true;

                }
                else if (gb.CompareTag("Toggle"))
                {
                    gb.GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (gb.CompareTag("Door"))
                {
                    /*
                    if (gb.GetComponent<DoorInfo>().isOpen)
                    {
                        Debug.Log(false);
                        gb.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    else
                    {
                        Debug.Log(true);
                        gb.GetComponent<BoxCollider2D>().enabled = true;
                    }
                    gb.GetComponentInChildren<BoxCollider2D>().enabled = true;
                    gb.GetComponent<SpriteRenderer>().color = Color.white;
                    */
                    gb.SetActive(true);
                }
                else if (gb.CompareTag("BonFire"))
                {
                    //gb.GetComponent<BonFireInfo>().a = 255;
                    gb.GetComponent<SpriteRenderer>().enabled = true;
                    gb.GetComponent<BoxCollider2D>().enabled = true;
                    
                }
                else if (gb.CompareTag("Handrail"))
                {
                    gb.SetActive(true);

                }
                else if (gb.tag.Equals("Enemy"))
                {
                    gb.GetComponent<TopDownCharacterController>().TurnOnObjects();
                }
                else if (gb.tag.Equals("TouchObject"))
                {
                    gb.transform.Find(gb.name + " Collider").GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (gb.tag.Equals("Box"))
                {
                    gb.GetComponent<BoxCollider2D>().enabled = true;
                    gb.GetComponent<SpriteRenderer>().color = Color.white;
                    gb.transform.Find(gb.name + " Collider").GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (gb.tag.Equals("Generator"))
                {
                    ObjectGeneratorInfo objectGeneratorInfo = gb.GetComponent<ObjectGeneratorInfo>();
                    if (objectGeneratorInfo.gb == null)
                    {
                        //objectGeneratorInfo.Generate();
                    }
                    else
                    {
                        //Enemy‚Ì‚Ý
                        objectGeneratorInfo.gb.GetComponent<TopDownCharacterController>().TurnOnObjects();
                    }
                }
                else if (gb.tag.Equals("RoomChangeTrigger"))
                {
                    gb.GetComponent<EdgeCollider2D>().enabled = true;
                }
                else
                {
                    gb.SetActive(true);
                }
            }
        }
        /*foreach (SpriteRenderer sr in sort2_clear)
        {
            sr.color = new Color32(255,255,255,255);
        }*/

        shadowManage.isInvisible = false;

        invisibleWall.SetActive(true);
        damageObjectWall.SetActive(true);
        //enemyWall.SetActive(true);
    }

    public void RoomOff()
    {

        /*sort0.color = new Color32(255,255,255,150);
        sort1.color = new Color32(255, 255, 255, 150);
        sort4.color = new Color32(255, 255, 255, 150);*/

        sort0.color = Color.white;
        sort0_nav.enabled = false;
        sort1.color = Color.white;
        sort4.color = Color.white;

        sort2.RemoveAll(x => x == null);
        //sort2_clear.RemoveAll(x => x == null);

        foreach (GameObject gb in sort2)
        {

            if (gb != null)
            {
                if (gb.tag.Equals("Rock"))
                {
                    gb.SetActive(false);
                    //gb.GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (gb.CompareTag("Toggle"))
                {
                    gb.GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (gb.CompareTag("Door"))
                {
                    /*
                    Debug.Log(false);
                    gb.GetComponent<BoxCollider2D>().enabled = false;
                    gb.GetComponentInChildren<BoxCollider2D>().enabled = false;
                    gb.GetComponent<SpriteRenderer>().color = Color.white;
                    */
                    gb.SetActive(false);
                }
                else if (gb.CompareTag("BonFire"))
                {
                    //gb.GetComponent<BonFireInfo>().a = 255;
                    gb.GetComponent<SpriteRenderer>().enabled = true;
                    gb.GetComponent<BoxCollider2D>().enabled = false;

                }
                else if (gb.CompareTag("Handrail"))
                {
                    gb.SetActive(false);

                }
                else if (gb.tag.Equals("Enemy"))
                {
                    gb.GetComponent<TopDownCharacterController>().TurnOffObjects();
                }
                else if (gb.tag.Equals("TouchObject"))
                {
                    gb.transform.Find(gb.name + " Collider").GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (gb.tag.Equals("Box"))
                {
                    gb.GetComponent<BoxCollider2D>().enabled = false;
                    gb.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
                    gb.transform.Find(gb.name + " Collider").GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (gb.tag.Equals("Generator"))
                {
                    ObjectGeneratorInfo objectGeneratorInfo = gb.GetComponent<ObjectGeneratorInfo>();
                    if (objectGeneratorInfo.gb == null)
                    {

                    }
                    else
                    {
                        //Enemy‚Ì‚Ý
                        objectGeneratorInfo.gb.GetComponent<TopDownCharacterController>().TurnOffObjects();
                    }
                }
                else if (gb.tag.Equals("RoomChangeTrigger"))
                {
                    gb.GetComponent<EdgeCollider2D>().enabled = false;
                }
                else
                {
                    gb.SetActive(false);
                }
            }
        }
        /*foreach (SpriteRenderer sr in sort2_clear)
        {
            sr.color = new Color32(255, 255, 255, 150);
        }*/

        shadowManage.isInvisible = false;

        invisibleWall.SetActive(false);
        damageObjectWall.SetActive(false);
        //enemyWall.SetActive(false);
    }

    public void RoomInvisible()
    {
        sort0.color = new Color32(255, 255, 255, 0);
        sort0_nav.enabled = false;
        sort1.color = new Color32(255, 255, 255, 0);
        sort4.color = new Color32(255, 255, 255, 0);

        sort2.RemoveAll(x => x == null);
        //sort2_clear.RemoveAll(x => x == null);

        foreach (GameObject gb in sort2)
        {
            if (gb != null)
            {
                if (gb.tag.Equals("Rock"))
                {
                    gb.SetActive(false);
                    //gb.GetComponent<BoxCollider2D>().enabled = false;
                }
                else if(gb.CompareTag("Toggle"))
                {
                    gb.GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (gb.CompareTag("Door"))
                {
                    /*
                    gb.GetComponent<BoxCollider2D>().enabled = false;
                    gb.GetComponentInChildren<BoxCollider2D>().enabled = false;
                    gb.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,0);
                    */
                    gb.SetActive(false);
                }
                else if (gb.CompareTag("BonFire"))
                {
                    //gb.GetComponent<BonFireInfo>().a = 0;
                    gb.GetComponent<SpriteRenderer>().enabled = false;
                    gb.GetComponent<BoxCollider2D>().enabled = false;

                }
                else if (gb.CompareTag("Handrail"))
                {
                    gb.SetActive(false);

                }
                else if (gb.tag.Equals("Enemy"))
                {
                    gb.GetComponent<TopDownCharacterController>().InvisibleObjects();
                }
                else if (gb.tag.Equals("TouchObject"))
                {
                    gb.transform.Find(gb.name + " Collider").GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (gb.tag.Equals("Box"))
                {
                    gb.GetComponent<BoxCollider2D>().enabled = false;
                    gb.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
                    gb.transform.Find(gb.name + " Collider").GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (gb.tag.Equals("Generator"))
                {
                    ObjectGeneratorInfo objectGeneratorInfo = gb.GetComponent<ObjectGeneratorInfo>();
                    if (objectGeneratorInfo.gb == null)
                    {
                        //objectGeneratorInfo.Generate();
                    }
                    else
                    {
                        //Enemy‚Ì‚Ý
                        objectGeneratorInfo.gb.GetComponent<TopDownCharacterController>().InvisibleObjects();
                    }
                }
                else if (gb.tag.Equals("RoomChangeTrigger"))
                {
                    gb.GetComponent<EdgeCollider2D>().enabled = false;
                }
                else
                {
                    gb.SetActive(false);
                }
            }
        }
        /*foreach (SpriteRenderer sr in sort2_clear)
        {
            sr.color = new Color32(255, 255, 255, 0);
        }*/

        shadowManage.isInvisible = true;

        invisibleWall.SetActive(false);
        damageObjectWall.SetActive(false);
        //enemyWall.SetActive(false);
    }


    public void GenerateObject()
    {
        foreach (GameObject gb in sort2)
        {
            if (gb != null)
            {
                if (gb.tag.Equals("Generator"))
                {
                    ObjectGeneratorInfo objectGeneratorInfo = gb.GetComponent<ObjectGeneratorInfo>();
                    if (objectGeneratorInfo.gb == null)
                    {
                        objectGeneratorInfo.Generate();
                    }
                }
            }
        }
    }

    public void DeleteObject()
    {
        foreach (GameObject gb in sort2)
        {
            if (gb != null)
            {
                if (gb.tag.Equals("Generator"))
                {
                    ObjectGeneratorInfo objectGeneratorInfo = gb.GetComponent<ObjectGeneratorInfo>();
                    Destroy(objectGeneratorInfo.gb);
                }
            }
        }
    }
}
