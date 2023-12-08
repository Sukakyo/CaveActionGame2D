using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShadowManageInfo : MonoBehaviour
{
    [SerializeField]
    List<BonFireInfo> list = new List<BonFireInfo>();

    [SerializeField]
    public GameObject darkShadow;
    [SerializeField]
    public GameObject lightShadow;

    [SerializeField]
    public bool isInvisible = false;

    //int count = 0;

    public void ChangeShadow()
    {
        int num = list.Count(n => n.FireOnOff);

        if (!isInvisible)
        {
            if (num == 0)
            {
                darkShadow.SetActive(true);
                lightShadow.SetActive(false);
            }
            else if (num == 1)
            {
                darkShadow.SetActive(false);
                lightShadow.SetActive(true);
            }
            else
            {
                darkShadow.SetActive(false);
                lightShadow.SetActive(false);
            }
        }
        else
        {
            darkShadow.SetActive(false);
            lightShadow.SetActive(false);
        }
    }

    public void Update()
    {
        ChangeShadow();
        /*
        if (count > 10)
        {
            ChangeShadow();
            count = 0;
        }
        else
        {
            count++;
        }
        */
    }
}
