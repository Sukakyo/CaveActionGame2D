using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInfo : MonoBehaviour
{
    [SerializeField]
    private int sprite_num;

    [SerializeField]
    Sprite[] sprites;

    public bool isOpen = false;

    private void OnEnable()
    {
        if (isOpen)
        {
            GetComponent<Animator>().SetBool("OpenOnOff", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("OpenOnOff", false);
        }
    }

    public void ChangeSprite(int num)
    {
        sprite_num = num;
        GetComponent<SpriteRenderer>().sprite = sprites[sprite_num];
    }

    public void AudioPlay()
    {
        GetComponent<AudioSource>().Play();
    }

    public void OpenDoor()
    {
        GetComponent<Animator>().SetBool("OpenOnOff",true);
        GetComponent<BoxCollider2D>().enabled = false;
        Debug.Log(false);
        this.transform.Find("Delete").GetComponent<BoxCollider2D>().enabled = false;
        isOpen = true;
        //Start();
    }

    public void CloseDoor()
    {
        GetComponent<Animator>().SetBool("OpenOnOff", false);
        GetComponent<BoxCollider2D>().enabled = true;
        Debug.Log(true);
        this.transform.Find("Delete").GetComponent<BoxCollider2D>().enabled = true;
        isOpen = false;
        //Start();
    }
}
