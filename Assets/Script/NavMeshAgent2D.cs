using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgent2D : MonoBehaviour
{
    [Header("Steering")]
    public float speed = 1.0f;
    public float stoppingDistance = 0;

    [HideInInspector]//���Unity�G�f�B�^�����\��
    private Vector2 trace_area = Vector2.zero;

    private Vector2 _corner;
    public Vector2 Corner { get { return _corner; } set { _corner = value; } }

    public bool lock_on = false;

    

    public Vector2 destination
    {
        get { return trace_area; }
        set
        {
            trace_area = value;
            Trace(transform.position, value);
        }
    }
    public bool SetDestination(Vector2 target)
    {
        destination = target;
        return true;
    }

    private void Trace(Vector2 current, Vector2 target)
    {
        if (Vector2.Distance(current, target) <= stoppingDistance)
        {
            return;
        }

        // NavMesh �ɉ����Čo�H�����߂�
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path);

        if(path.corners.Length > 0)
        {
            Corner = path.corners[0];

            if (Vector2.Distance(current, _corner) <= 0.05f)
            {
                Corner = path.corners[1];
            }

            //Debug.Log(Corner);

            //transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);

            
                lock_on = true;
            
        }
        else
        {
            
                lock_on = false;
            
        }


    }
}