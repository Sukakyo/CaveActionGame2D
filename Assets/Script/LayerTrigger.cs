using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //when object exit the trigger, put it to the assigned layer and sorting layers
    //used in the stair objects for player to travel between layers
    public class LayerTrigger : MonoBehaviour
    {
        public string layer;
        public string sortingLayer;

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Fire") || other.CompareTag("Scythe Effect")) {
                if (other.CompareTag("Player"))
                {
                    other.gameObject.layer = LayerMask.NameToLayer(layer);
                    ToolInfo tool_info = other.transform.Find("ToolHolder").Find("Tool").GetComponent<ToolInfo>();

                    tool_info.SetLayer(sortingLayer);
                }

                other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
                SpriteRenderer[] srs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sr in srs)
                {
                    sr.sortingLayerName = sortingLayer;
                }

               
                
            }
        }

    }
}
