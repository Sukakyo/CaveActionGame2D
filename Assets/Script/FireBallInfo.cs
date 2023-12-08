using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallInfo : MonoBehaviour
{
    
    public Vector2 startDirection;
    [SerializeField]
    private float speed;

    [SerializeField]
    private Sprite[] sprites;
    private int spriteNum = 0;

    float time = 0;
    [SerializeField]
    float max_time = 2f;

    

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    private void Update()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = startDirection * speed;
        this.gameObject.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(new Vector2(0, -1), startDirection));


        if(time > max_time)
        {
            Destroy(this.gameObject);
        }
        else
        {
            time += Time.deltaTime;
        }

    }


    public void ChangeSprite()
    {


        spriteNum++;
        spriteNum %= sprites.Length;
        GetComponent<SpriteRenderer>().sprite = sprites[spriteNum];
    }
}
