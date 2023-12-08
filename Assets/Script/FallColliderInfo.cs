using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallColliderInfo : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private bool isOn;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isOn)
        {
            tilemap.color = Color.white;
        }
        else
        {
            tilemap.color = new Color32(255, 255, 255, 200);
        }
    }
}
