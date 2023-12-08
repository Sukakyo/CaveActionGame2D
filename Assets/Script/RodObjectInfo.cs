using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RodObjectInfo : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigid;

    [SerializeField]
    Sprite[] sprites;

    [SerializeField]
    GameObject prehab;

    Vector2[] _goToList;
    int currentDes = 0;
    bool loopExist;

    [SerializeField]
    float speed;

    Transform _target;
    float angle;

    public bool level_up = false;
    bool fireSwitch = false;
    float createTime = 0;
    [SerializeField]
    float max_create_time = 0.1f;

    int count = 0;

    bool canCreate = true;


    private bool fireOn = false;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void ChangeSprites(int num)
    {
        _spriteRenderer.sprite = sprites[num];
    }

    public void Initialize(Vector2[] goToPos,bool loop,Transform _target)
    {
        _goToList = goToPos;
        loopExist = loop;
        this._target = _target;
    }

    public void Update()
    {
        float dist = Vector2.Distance(this.transform.position, _goToList[currentDes]);
        if(dist < 1f)
        {
            currentDes++;
            if(loopExist)
            {
                currentDes %= _goToList.Length;
            }
        }


        if(currentDes >= _goToList.Length)
        {
            Destroy(gameObject);
        }

        Vector2 dir = _target.position - this.transform.position;
        angle = Vector2.SignedAngle(new Vector2(0,-1),dir);

        this.transform.rotation = Quaternion.Euler(0,0,angle);

        Debug.Log(_goToList[currentDes]);
        Debug.Log(new Vector2(this.transform.position.x, this.transform.position.y));
        _rigid.velocity = (_goToList[currentDes] - new Vector2(this.transform.position.x,this.transform.position.y)).normalized * speed;


        if (!fireOn)
        {

            if (fireSwitch && canCreate)
            {

                GameObject go = Instantiate(prehab, this.transform);
                go.transform.parent = null;
                go.GetComponent<FireBallInfo>().startDirection = Quaternion.Euler(0, 0, angle) * new Vector2(0, -1);
                canCreate = false;
                fireOn = true;

                if (count >= 5)
                {
                    count = 0;
                    fireSwitch = false;
                    
                }
                else
                {
                    count++;
                }


            }

            // 作成時のクールタイム
            if (!canCreate)
            {
                if (createTime >= max_create_time)
                {
                    canCreate = true;
                    createTime = 0f;
                }
                else
                {
                    createTime += Time.deltaTime;
                }
            }
        } 
    }

    private void LateUpdate()
    {
        fireOn = false;
    }

    public void Fire()
    {
        GameObject go = Instantiate(prehab, this.transform);
        go.transform.parent = null;
        go.GetComponent<FireBallInfo>().startDirection = Quaternion.Euler(0, 0, angle) * new Vector2(0, -1);

        if (level_up)
        {
            fireSwitch = true;
        }
    }
}
