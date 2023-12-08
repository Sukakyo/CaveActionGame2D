using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderInfo : MonoBehaviour
{
    public float speed;

    [SerializeField]
    public Vector2 desternation;

    // Update is called once per frame
    void Update()
    {
        Vector2 pos_tmp = this.transform.position;
        this.transform.position = Vector2.MoveTowards(pos_tmp, desternation,speed * Time.deltaTime);

        if(Vector2.Distance(pos_tmp,desternation) < 0.1f)
        {
            Destroy(this.gameObject);
        }
    }
}
