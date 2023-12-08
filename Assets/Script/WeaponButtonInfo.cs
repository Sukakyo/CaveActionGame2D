using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonInfo : MonoBehaviour
{
    [SerializeField]
    Sprite normal_weapon;
    [SerializeField]
    Sprite special_weapon;

    
    public void ChangeSprite(string type)
    {
        if (type.Equals("normal"))
        {
            GetComponent<Image>().sprite = normal_weapon;
        }
        else if (type.Equals("special"))
        {
            GetComponent<Image>().sprite = special_weapon;
        }
    }
}
