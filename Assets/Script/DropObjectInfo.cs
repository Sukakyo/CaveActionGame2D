using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropObjectInfo : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            PlayerTriggerMovement(collision);
        }
    }

    protected abstract void PlayerTriggerMovement(Collider2D collision);
}
