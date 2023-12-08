using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInfo : MonoBehaviour
{
    [SerializeField]
    bool destroy_switch = false;
    [SerializeField]
    Vector2 vl;

    private void Start()
    {
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.GetComponent<Rigidbody2D>().velocity = vl;

        if (destroy_switch)
        {
            Destroy(this.gameObject);
        }
    }
}
