using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInfo : MonoBehaviour
{
    [SerializeField]
    DoorInfo[] doorInfo;

    private enum ToggleSwitch
    {
        left,right
    }

    [SerializeField]
    private ToggleSwitch toggleSwitch = ToggleSwitch.left;

    [SerializeField]
    private bool toggle_use_switch = true;

    [SerializeField]
    private Sprite[] sprites;

    int spriteNum = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Animator toggleAnimator = GetComponent<Animator>();
        if (toggle_use_switch)
        {
            if (collision.tag == "Player" || collision.tag == "Fire") {
                if (toggleSwitch == ToggleSwitch.left) {
                    toggleAnimator.SetInteger("Inclination", 1);
                    toggleAnimator.SetTrigger("Collision");
                    foreach(DoorInfo door in doorInfo) {
                        door.OpenDoor();
                    }
                }
                else if (toggleSwitch == ToggleSwitch.right)
                {
                    toggleAnimator.SetInteger("Inclination", 0);
                    toggleAnimator.SetTrigger("Collision");
                    foreach (DoorInfo door in doorInfo)
                    {
                        door.CloseDoor();
                    }
                   
                }
            }
        }
        else if (!toggle_use_switch)
        {

        }
        collision.transform.GetComponent<TouchEventObjectInfo>().DestroySelf();
    }

    public void ChangeSprite(int num)
    {
        spriteNum = num;
        GetComponent<SpriteRenderer>().sprite = sprites[spriteNum];
    }

    public void AudioPlay()
    {
        GetComponent<AudioSource>().Play();
    }
}
