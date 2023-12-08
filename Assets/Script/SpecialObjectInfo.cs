using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialObjectInfo : MonoBehaviour
{
    [SerializeField]
    float special_value = 0f;
    [SerializeField]
    float max_special_value = 120f;

    bool canUseSpecial = false;
    public bool CanUseSpecial {get {return canUseSpecial;}}

    [SerializeField]
    GameManagement gameManagement = null;
    [SerializeField]
    GameObject slider;

    Animator ui_animator;

    [SerializeField]
    private WeaponButtonInfo weaponButtonInfo;

    //bool isPlay = false;



    // Start is called before the first frame update

    private void Awake()
    {
        if (gameManagement.gameMode == GameManagement.GameMode.play_game)
        {
            ui_animator = slider.GetComponent<Animator>();
        }
        //AnimationSet();

        
    }

    private void Initialize()
    {
        if (gameManagement.weaponState == GameManagement.WeaponState.normal)
        {
            if (ui_animator.gameObject.activeInHierarchy)
            {
                ui_animator.SetBool("IsSpecial", false);
            }
            AnimationSet();
        }
        else if (gameManagement.weaponState == GameManagement.WeaponState.special)
        {
            if (ui_animator.gameObject.activeInHierarchy)
            {
                ui_animator.SetBool("IsSpecial", true);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag.Equals("Player"))
        {
            if (gameManagement.gameMode == GameManagement.GameMode.play_game)
            {
                if (gameManagement.state == GameManagement.GameState.play)
                {
                    Initialize();
                    if (!canUseSpecial)
                    {
                        
                        if (gameManagement.weaponState == GameManagement.WeaponState.normal)
                        {
                            if (special_value >= max_special_value)
                            {
                                special_value = max_special_value;
                                canUseSpecial = true;
                                ui_animator.SetTrigger("FullChargeTrigger");
                            }
                            else
                            {
                                canUseSpecial = false;
                                special_value += Time.deltaTime;

                                slider.transform.Find("Slider").GetComponent<Slider>().value = GetSpecialValue();

                            }
                        }
                        else if (gameManagement.weaponState == GameManagement.WeaponState.special)
                        {
                            if (special_value <= 0)
                            {
                                special_value = 0;
                                ui_animator.SetTrigger("ChargeTrigger");
                                gameManagement.weaponState = GameManagement.WeaponState.normal;
                                weaponButtonInfo.ChangeSprite("normal");

                            }
                            else
                            {
                                special_value -= Time.deltaTime * 2;
                                slider.transform.Find("Slider").GetComponent<Slider>().value = GetSpecialValue();
                            }
                        }
                    }
                    else
                    {
                        //AnimationSet();
                        
                    }
                }
                else
                {
                    
                }
            }
        }
    }

    public void UseSpecialWeapon()
    {
        
        gameManagement.weaponState = GameManagement.WeaponState.special;
        weaponButtonInfo.ChangeSprite("special");
        ui_animator.SetTrigger("PushTrigger");
        
        canUseSpecial = false;
        AnimationSet();
        
    }

    public float GetSpecialValue()
    {
        

        return special_value / max_special_value;
    }

    public void AnimationSet()
    {
        if (ui_animator.gameObject.activeInHierarchy)
        {
            if (canUseSpecial)
            {
                ui_animator.SetBool("ChargeCheck", true);
            }
            else
            {
                ui_animator.SetBool("ChargeCheck", false);
            }
        }
    }
}
