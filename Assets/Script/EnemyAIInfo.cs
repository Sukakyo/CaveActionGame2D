using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIInfo : MonoBehaviour
{
    NavMeshAgent2D agent;
    [SerializeField]
    Transform target;

    private Vector2 _dir;
    public Vector2 Dir { get { return _dir; } }

    /*private float first_x_point;
    private float first_y_point;*/

    [SerializeField]
    private bool damaged_switch;



    private Vector2 _turnDirection = new Vector2(0, -1);
    public Vector2 turn_Direcion { private get { return _turnDirection; } set { _turnDirection = value; } }

    private Vector2 _defaultPosition;
    private Vector2 _offsetPosition;

    int count;

    [SerializeField]
    private float lock_on_distance;

    private bool lockOn = false;
    private bool stayLockOn = false;

    private int parentObjectLayer;


    public void Bake()
    {
        target = GameObject.Find("Player").transform;
    }


    void Start()
    {
        parentObjectLayer = this.gameObject.transform.parent.gameObject.layer;

        agent = GetComponentInChildren<NavMeshAgent2D>(); //agentにNavMeshAgent2Dを取得
        _defaultPosition = this.transform.position;
        
        count = Random.Range(0, 6);
        /*first_x_point = this.transform.position.x;
        first_y_point = this.transform.position.y;*/
    }

    void Update()
    {
        
        if (agent.lock_on && !damaged_switch && stayLockOn)
        {
            agent.destination = target.position;           //agentの目的地をtargetの座標にする
                                                           //agent.SetDestination(target.position); //こっちの書き方でもオッケー
        }
        else if (damaged_switch)
        {

        }
        else
        {
            switch (count)
            {
                case 0:
                    _offsetPosition = new Vector2(1, 0);
                    break;
                case 1:
                    _offsetPosition = new Vector2(1, 1);
                    break;
                case 2:
                    _offsetPosition = new Vector2(1, -1);
                    break;
                case 3:
                    _offsetPosition = new Vector2(-1, 0);
                    break;
                case 4:
                    _offsetPosition = new Vector2(-1, 1);
                    break;
                case 5:
                    _offsetPosition = new Vector2(-1, -1);
                    break;


            }
            agent.destination = _defaultPosition + _offsetPosition;

            if (((_defaultPosition + _offsetPosition) - new Vector2(this.transform.position.x, this.transform.position.y)).magnitude < 0.1f)
            {
                count++;
                count %= 6;
            }

            stayLockOn = false;

        }

        _dir = (agent.Corner - new Vector2(this.transform.position.x, this.transform.position.y)).normalized;


        //Debug.Log(target.gameObject.layer== parentObjectLayer);
        //Debug.Log((target.position - this.transform.position).magnitude < lock_on_distance);

        if ((target.gameObject.layer == parentObjectLayer))
        {
            lockOn = true;
        }
        else
        {
            lockOn = false;
        }

        if ((target.position - this.transform.position).magnitude < lock_on_distance && lockOn)
        {
            stayLockOn = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            count += 3;
            count %= 6;
        }
    }


}
