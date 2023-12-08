using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectInfo : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.transform.parent.tag.Equals("Fire"))
        {
            if (!collision.tag.Equals("Player"))
            {
                Destroy(this.transform.parent.gameObject);
            }
        }
        else if(this.transform.parent.tag.Equals("Scythe Effect"))
        {
            if (!collision.tag.Equals("Player") && !collision.tag.Equals("Barrier"))
            {
                Destroy(this.transform.parent.gameObject);
            }
            else if (collision.tag.Equals("Barrier"))
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}
