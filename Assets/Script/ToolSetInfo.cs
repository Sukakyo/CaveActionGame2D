using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolSetInfo : MonoBehaviour
{
    [SerializeField]
    Image weaponImage;
    [SerializeField]
    Image toolImage;
    [SerializeField]
    TextMeshProUGUI toolNumText;

    
    public void SetWeaponImage(Sprite sp)
    {
        weaponImage.sprite = sp;
    }

    public void SetToolImage(Sprite sp,bool use_on_off)
    {
        toolImage.enabled = use_on_off;
        toolImage.sprite = sp;
    }

    public void SetToolNumText(string str,bool use_on_off)
    {
        if (str.Equals("000"))
        {
            toolNumText.color = (new Color32(178,34,34,255));
        }
        else
        {
            toolNumText.color = Color.black;
        }
        toolNumText.enabled = use_on_off;
        toolNumText.text = str;
    }

    public void SetToolNumText(string str)
    {
        if (str.Equals("000"))
        {
            toolNumText.color = (new Color32(178, 34, 34, 255));
        }
        else
        {
            toolNumText.color = Color.black;
        }
        toolNumText.text = str;
    }
}
