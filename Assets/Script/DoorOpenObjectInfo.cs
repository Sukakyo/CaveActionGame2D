using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenObjectInfo : MonoBehaviour
{
    [SerializeField]
    DoorInfo doorInfo;

    [SerializeField]
    List<GameObject> objects = new List<GameObject>();


    bool doorOpen = false;

    private void Update()
    {
        if (!doorOpen)
        {
            if (Check())
            {
                doorInfo.OpenDoor();
                foreach (GameObject obj in objects)
                {
                    if (obj.tag.Equals("BonFire"))
                    {
                        obj.GetComponent<BonFireInfo>().autoExtinguish = false;
                    }
                }
                doorOpen = true;
            }
        }
    }

    private bool Check()
    {
        foreach (GameObject go in objects)
        {
            if (go.tag.Equals("Enemy"))
            {
                if(go == null)
                {
                    return false;
                }
            }
            else if (go.tag.Equals("BonFire"))
            {
                if (!go.GetComponent<BonFireInfo>().FireOnOff)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
