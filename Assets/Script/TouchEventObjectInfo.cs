using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEventObjectInfo : MonoBehaviour
{
    [SerializeField]
    string object_mode;


    public void DestroySelf()
    {
        if (object_mode == "destroy")
        {
            Destroy(this.transform.parent.gameObject);
        }
        else if (object_mode == "weapon")
        {

        }
    }
}
