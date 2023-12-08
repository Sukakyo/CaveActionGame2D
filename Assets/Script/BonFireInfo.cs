using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BonFireInfo : MonoBehaviour
{
    //left -> on
    //right -> off
    [FormerlySerializedAs("fire_on_off")]
    [SerializeField]
    bool fireOnOff;
    public bool FireOnOff { get { return fireOnOff; } }

    [FormerlySerializedAs("fire_use_switch")]
    [SerializeField]
    bool fireUseSwitch;

    [FormerlySerializedAs("sprites")]
    [SerializeField]
    private Sprite[] _sprites;

    int spriteNum = 0;

    [SerializeField]
    bool isFireAlready;

    public bool autoExtinguish = false;

    [SerializeField]
    float extinguishTime = 0.5f;

    private float currentTime = 0f;

    //public byte a = 255;

    private void Start()
    {
        GetComponent<Animator>().SetBool("IsStartFire",isFireAlready);
        GetComponent<Animator>().SetBool("Fire", isFireAlready);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Animator bonFireAnimator = GetComponent<Animator>();
        if (fireUseSwitch)
        {
            if (collision.tag == "Fire")
            {
                if (!fireOnOff)
                {
                    bonFireAnimator.SetBool("Fire", true);
                    bonFireAnimator.SetTrigger("Collision");

                }
                
            }
        }
        else if (!fireUseSwitch)
        {

        }
        collision.transform.GetComponent<TouchEventObjectInfo>().DestroySelf();

    }

    private void TurnOff()
    {
        Animator bonFireAnimator = GetComponent<Animator>();
        bonFireAnimator.SetBool("Fire", false);
        bonFireAnimator.SetTrigger("Collision");
    }
    


    public void ChangeSprite(int num)
    {
        spriteNum = num;
        GetComponent<SpriteRenderer>().sprite = _sprites[spriteNum];
        //GetComponent<SpriteRenderer>().color = new Color32(255,255,255,a);

        if(this.transform.parent.gameObject.name=="Sort2_17")
        Debug.Log(GetComponent<SpriteRenderer>().color);
    }

    public void AudioPlay()
    {
        GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        if (autoExtinguish)
        {
            if (fireOnOff)
            {
                if(currentTime > extinguishTime)
                {
                    currentTime = 0;
                    TurnOff();
                }
                else
                {
                    currentTime += Time.deltaTime;
                }
            }
            else
            {
                currentTime = 0f;
            }
        }
    }
}
