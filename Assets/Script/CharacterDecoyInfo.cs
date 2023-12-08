using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtTopDown_Basic;
using System;
using Unity.VisualScripting;

public class CharacterDecoyInfo : MonoBehaviour
{
    [SerializeField]
    private Transform trans;
    [SerializeField]
    private SpriteRenderer spriteRenderer;


    [SerializeField]
    private Vector2[] desternation;
    private int desternationNum = 0;
    public float lerpSpeed = 1.0f;
    [SerializeField]
    private bool away = false;

    [SerializeField]
    private bool fall = false;
    private Vector3 _thisPos;

    [SerializeField]
    private ValueList<Sprite>[] sprites;

    public int turn_num = 0;
    public int move_num = 0;


    [SerializeField]
    private Animator animator;

    public void ChangeTurn(int index)
    {
        turn_num = index;
    }

    public void ChangeMove(int index)
    {
        move_num = index;
    }

    public void StartMove()
    {
        animator.SetBool("move" , true);
    }

    public void EndMove()
    {
        animator.SetBool("move", false);
    }

    public void StartFast()
    {
        animator.SetBool("fast_move", true);
    }

    public void EndFast()
    {
        animator.SetBool("fast_move", false);
    }

    public void StartMagic()
    {
        animator.SetTrigger("magic");
    }

    

    
    public void ChangeAway(bool away)
    {
        this.away = away;
    }



    public void Update()
    {
        if (desternationNum < desternation.Length)
        {
            if (away)
            {
                Vector3 tmp_dest = desternation[desternationNum];
                trans.position = Vector3.MoveTowards(trans.position, tmp_dest, lerpSpeed * Time.deltaTime);

                if (Vector3.Distance(trans.position, tmp_dest) < 0.1f)
                {
                    away = false;
                    desternationNum++;
                }

            }
            
        }

        if (fall)
        {
            Vector3 tmp_dest = _thisPos + new Vector3(0,-100f,0);
            trans.position = Vector3.MoveTowards(trans.position, tmp_dest, 100 * Time.deltaTime);
            if (Vector3.Distance(trans.position, tmp_dest) < 0.1f)
            {
                fall = false;
            }
        }


        spriteRenderer.sprite = sprites[turn_num].List[move_num];
    }


    public void Fall()
    {
        this.fall = true;
        _thisPos = this.transform.position;
    }
}
