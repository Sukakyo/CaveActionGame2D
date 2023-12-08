using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveInfo : MonoBehaviour
{
    protected Vector2 dir = Vector2.zero;
    public Vector2 Dir
    {
        get
        {
            return dir;
        }
    }

    public Vector2 move_dir;

    protected virtual void Update()
    {

        dir.x = ToHorizontal();

        dir.y = ToVertical();

        if (dir.magnitude > 0)
        {
            dir = dir.normalized;
        }
    }

    protected abstract float ToVertical();
    protected abstract float ToHorizontal();


    public void SetMove(Vector2 mv_dir)
    {
        move_dir = mv_dir;
    }

}
