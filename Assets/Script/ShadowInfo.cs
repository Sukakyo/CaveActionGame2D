using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShadowInfo : MonoBehaviour
{
    List<GameObject> _collisionObjects = new List<GameObject>();

    int count = 0;

    Color _color;

    private void Start()
    {
        _color = this.GetComponent<SpriteRenderer>().color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != null)
        {
            _collisionObjects.Add(collision.gameObject);
        }

        
        ShadowCheck();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _collisionObjects.Remove(collision.gameObject);
        
        ShadowCheck();
    }

    public void ShadowCheck()
    {
        _collisionObjects.RemoveAll(s => s == null);
        if (_collisionObjects.Count == 0)
        {
            GetComponent<SpriteRenderer>().color = _color;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);
        }
    }

    public void Update()
    {
        if(count > 10) 
        {
            ShadowCheck();
            count = 0;
        }
        else
        {
            count++;
        }
    }
}
