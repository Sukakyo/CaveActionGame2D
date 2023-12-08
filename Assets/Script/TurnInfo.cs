using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnInfo : MonoBehaviour
{
    protected int dir_num = 0;
    public int DirNum
    {
        get { return dir_num; }
    }

    public Vector2 move_dir;


    // Update is called once per frame
    protected void Update()
    {
        if (this.transform.parent.tag.Equals("Boss"))
        {
            Debug.Log(move_dir);
        }

        if (ToLeft())
        {
            dir_num = 1;
        }
        else if (ToRight())
        {
            dir_num = 2;
        }

        if (ToBack())
        {
            dir_num = 3;
        }
        else if (ToFront())
        {
            dir_num = 0;
        }
    }

    protected abstract bool ToFront();
    protected abstract bool ToLeft();
    protected abstract bool ToRight();
    protected abstract bool ToBack();

    public void SetTurn(Vector2 mv_dir)
    {
        move_dir = mv_dir;
    }
}
