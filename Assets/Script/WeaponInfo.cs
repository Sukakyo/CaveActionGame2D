using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    

    public void ChangeDir(int dir)
    {
        if (gameObject.tag == "Player") {
            PolygonCollider2D collider2D = GetComponent<PolygonCollider2D>();
            PolygonCollider2D collider2DChildren = transform.Find("TouchEventWeapon").GetComponent<PolygonCollider2D>();
            switch (dir)
            {
                case 0:
                    collider2D.points = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(-0.45f,-0.2f),
                    new Vector2(0.05f,-0.7f),
                    new Vector2(0.46f,-0.7f),
                    new Vector2(0.6f,0.2f)
                };
                    collider2DChildren.points = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(-0.45f,-0.2f),
                    new Vector2(0.05f,-0.7f),
                    new Vector2(0.46f,-0.7f),
                    new Vector2(0.6f,0.2f)
                };

                    break;
                case 1:
                    collider2D.points = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(-0.15f,0.45f),
                    new Vector2(-0.7f,0),
                    new Vector2(-0.7f,-0.5f),
                    new Vector2(0.1f,-0.6f)
                };
                    collider2DChildren.points = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(-0.15f,0.45f),
                    new Vector2(-0.7f,0),
                    new Vector2(-0.7f,-0.5f),
                    new Vector2(0.1f,-0.6f)
                };
                    break;
                case 2:
                    collider2D.points = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(0,-0.5f),
                    new Vector2(0.7f,-0.25f),
                    new Vector2(0.7f,0.35f),
                    new Vector2(-0.25f,0.5f)
                };
                    collider2DChildren.points = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(0,-0.5f),
                    new Vector2(0.7f,-0.25f),
                    new Vector2(0.7f,0.35f),
                    new Vector2(-0.25f,0.5f)
                };
                    break;
                case 3:
                    collider2D.points = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(0.55f,0.2f),
                    new Vector2(0.1f,0.7f),
                    new Vector2(-0.38f,0.7f),
                    new Vector2(-0.6f,-0.2f)
                };
                    collider2DChildren.points = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(0.55f,0.2f),
                    new Vector2(0.1f,0.7f),
                    new Vector2(-0.38f,0.7f),
                    new Vector2(-0.6f,-0.2f)
                };
                    break;
            }
            
        }
        else if (gameObject.tag == "Boss")
        {
            
        }
    }

    public void PlayAudio()
    {
        GetComponent<AudioSource>().Play();
    }
}
