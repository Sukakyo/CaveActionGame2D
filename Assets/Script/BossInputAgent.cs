using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Cainos.PixelArtTopDown_Basic;
using Unity.MLAgents.Sensors;
using UnityEngine.Rendering;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;

public class BossInputAgent : Agent
{
    TopDownCharacterController.State state;
    Vector2 move_dir = Vector2.zero;
    Vector2 move_pos = Vector2.zero;
    Vector2 turn_dir = Vector2.zero;

    [SerializeField]
    Transform parentTransform;


    [SerializeField]
    Transform target; //Playerであること

    [SerializeField]
    BossMoveInfo moveInfo;
    [SerializeField]
    TurnInfo turnInfo;
    [SerializeField]
    ToolInfo toolInfo;

    [SerializeField]
    GameObject barrier;



    private TopDownCharacterController _controller;
    private Rigidbody2D _rigid;

    HPInfo _targetHPInfo;
    private int preTargetHp;

    HPInfo _pHpiSelf;
    private int preSelfHp;

    [SerializeField]
    Vector3 firstPos;

    private bool damageCheck = false;
    private bool endEpisode = false;

    private Vector2 pre_vel = Vector2.zero;

    [SerializeField]
    private RenderTexture _target;
    private Vector4[] color_vectors;


    private bool firstSpecial = true;

    private bool first = false;
    private float firstTime = 0f;
    private float maxFirstTime = 0.7f;


    private int coolCount = 0;
    private bool coolDown = true;



    private bool level2 = true;

    private bool level3 = true;

    private bool death = false;
    private bool fadeEnd = false;
    //private float load_scene_time = 0;
    /*[SerializeField]
    private float maxLoadSceneTime = 3f;*/

    
    [SerializeField]
    GameObject rodObject;
    [SerializeField]
    Vector2[] des_list = new Vector2[4];

    List<RodObjectInfo> rodList = new List<RodObjectInfo>();

    [SerializeField]
    FadeScreenInfo fadeScreenInfo;

    [SerializeField]
    string scene_name;

    

    private Color[] GetPixels()
    {
        // アクティブなレンダーテクスチャをキャッシュしておく
        RenderTexture currentRT = RenderTexture.active;

        // アクティブなレンダーテクスチャを一時的にTargetに変更する
        RenderTexture.active = _target;

        // Texture2D.ReadPixels()によりアクティブなレンダーテクスチャのピクセル情報をテクスチャに格納する
        Texture2D texture = new Texture2D(_target.width, _target.height);
        texture.ReadPixels(new Rect(0, 0, _target.width, _target.height), 0, 0);
        texture.Apply();

        // ピクセル情報を取得する
        Color[] colors = texture.GetPixels();

        // アクティブなレンダーテクスチャを元に戻す
        RenderTexture.active = currentRT;

        return colors;
    }

    private void Start()
    {
        _targetHPInfo = target.Find("HP").GetComponent<HPInfo>();

        preTargetHp = _targetHPInfo.max_hp;

        _pHpiSelf = parentTransform.Find("HP").GetComponent<HPInfo>();
        preSelfHp = _pHpiSelf.max_hp;

        _controller = parentTransform.GetComponent<TopDownCharacterController>();
        _rigid = parentTransform.GetComponent<Rigidbody2D>();

        //controller.OnSelect();

        
        
    }


    public void GetParam(TopDownCharacterController.State st, Vector2 mv_dir, Vector2 mv_pos, Vector2 tr_dir)
    {
        state = st;
        move_dir = mv_dir;
        move_pos = mv_pos;
        turn_dir = tr_dir;
    }

    public override void OnEpisodeBegin()
    {
        /*
        targetHPInfo.Start();

        int currentTargetHp = targetHPInfo.GetHP();
        preTargetHp = currentTargetHp;
        int currentSelfHp = pHpiSelf.GetHP();
        preSelfHp = currentSelfHp;
        parentTransform.position = firstPos;
        */
        //parentTransform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float targetPosX;
        float targetPosY;
        if (target != null)
        {
            targetPosX = target.position.x;
            targetPosY = target.position.y;
        }
        else
        {
            targetPosX = 0;
            targetPosY = 0;
        }

        GetParam(_controller.state,moveInfo.move_dir,parentTransform.position,turnInfo.move_dir);
        Color[] colors = GetPixels();
        color_vectors = new Vector4[colors.Length];
        for(int i = 0; i < colors.Length; i++)
        {
            color_vectors[i] = colors[i];
        }


        //Target情報の格納
        sensor.AddObservation((targetPosX - (-68f)) / 26f);
        sensor.AddObservation((targetPosY - 15f) / 16f);
        

        //Boss情報の格納
        sensor.AddObservation((int)state / 7f);
        sensor.AddObservation(move_dir.normalized);
        sensor.AddObservation((move_pos.x - (-68f)) / 26f);
        sensor.AddObservation((move_pos.y - 15f) / 16f);
        sensor.AddObservation(turn_dir.normalized);

        sensor.AddObservation(moveInfo.goToCheck);

        /*foreach (Vector4 vec in color_vectors)
        {
            sensor.AddObservation(new Vector3(vec[0], vec[1], vec[2]));
        }*/
    }

    private void Update()
    {

        if(!first)
        {
            if (firstTime > maxFirstTime)
            {
                first = true;
            }
            else
            {
                firstTime += Time.deltaTime;
            }
        }


        if (damageCheck)
        {
            endEpisode = true;
            damageCheck = false;
        }



        /*if (fade_end)
        {
            if (load_scene_time > maxLoadSceneTime)
            {
                SceneManager.LoadScene(scene_name);
            }
            else
            {
                load_scene_time += Time.deltaTime;
            }
        }*/
    }
    

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(this.transform.position);
        //Debug.Log(preTargetHp > 0);
        if (first && preTargetHp > 0)
        {
            moveInfo.stopCheckLong = false;


            rodList.RemoveAll(x => x == null);

            Debug.Log(this.StepCount);


            GetParam(_controller.state, moveInfo.move_dir, parentTransform.position, turnInfo.move_dir);

            if (firstSpecial)
            {
                turnInfo.SetTurn(new Vector2(0,-1));
                parentTransform.GetComponent<TopDownCharacterController>().UseSpecial();
                firstSpecial = false;
            }
            else
            {




                if (endEpisode)
                {
                    EndEpisode();
                    endEpisode = false;
                }


                //int num = Random.Range(0, 10);

                int num = actions.DiscreteActions[0];

                


                if (!moveInfo.goToCheck)
                {


                    //actions.DiscreteActions[0] < 1
                    if (num == 0)
                    {

                        
                    }
                    else
                    {
                        barrier.SetActive(false);

                        int current_pos = moveInfo.CurrentPosition;
                        Vector2 tool_dir = Vector2.zero;
                        switch (current_pos)
                        {
                            case 0:
                                turnInfo.SetTurn(new Vector2(0, -1));
                                parentTransform.GetComponent<TopDownCharacterController>().turn_dir_num = 0;
                                tool_dir = new Vector2(0, -1);
                                break;
                            case 1:
                                turnInfo.SetTurn(new Vector2(-1, 0));
                                parentTransform.GetComponent<TopDownCharacterController>().turn_dir_num = 1;
                                tool_dir = new Vector2(-1, 0);
                                break;
                            case 2:
                                turnInfo.SetTurn(new Vector2(0, 1));
                                parentTransform.GetComponent<TopDownCharacterController>().turn_dir_num = 3;
                                tool_dir = new Vector2(0, 1);
                                break;
                            case 3:
                                turnInfo.SetTurn(new Vector2(1, 0));
                                parentTransform.GetComponent<TopDownCharacterController>().turn_dir_num = 2;
                                tool_dir = new Vector2(1, 0);
                                break;
                        }
                        toolInfo.ChangeDirection(tool_dir);


                        if (coolDown)
                        {
                            _controller.OnActionSum(2);
                            foreach (RodObjectInfo roi in rodList)
                            {
                                roi.Fire();
                            }
                            AddReward(-0.2f);
                            coolDown = false;
                        }

                    }

                    moveInfo.ChangeGoTo(Random.Range(0, 2));
                }
                else
                {
                    if (coolDown)
                    {
                        barrier.SetActive(true);
                    }

                    Vector2 dir = target.transform.position - this.transform.position;

                    float angle = Vector2.SignedAngle(new Vector2(0, -1), dir);
                    BarrierInfo barrierInfo = barrier.GetComponent<BarrierInfo>();
                    if (angle < 45f && angle > -45f)
                    {
                        barrierInfo.ChangeDirection(0);
                    }
                    else if (angle < 135f && angle > 45f)
                    {
                        barrierInfo.ChangeDirection(3);
                    }
                    else if (angle < -45f && angle > -135f)
                    {
                        barrierInfo.ChangeDirection(1);
                    }
                    else if (angle > 135f || angle < -135f)
                    {
                        barrierInfo.ChangeDirection(2);
                    }



                    if (moveInfo.Dir.y < 0.3f && moveInfo.Dir.y > -0.3f)
                    {
                        turnInfo.SetTurn(new Vector2(moveInfo.Dir.x, 0));
                    }
                    else
                    {
                        turnInfo.SetTurn(moveInfo.Dir);
                    }


                }


                if (!coolDown)
                {
                    coolCount += 1;
                    if (coolCount >= 15)
                    {
                        coolCount = 0;
                        coolDown = true;
                    }
                }



                if (Vector2.Distance(parentTransform.position, target.position) < 1.5f)
                {
                    barrier.SetActive(false);
                    _controller.OnActionSum(1);
                }




                int currentTargetHp = _targetHPInfo.GetHP();
                if (currentTargetHp < preTargetHp)
                {

                    AddReward((1000 - this.StepCount) / 1000);
                    Debug.Log("target damage");
                    damageCheck = true;
                }
                preTargetHp = currentTargetHp;



                int currentSelfHp = _pHpiSelf.GetHP();
                if (currentSelfHp < preSelfHp)
                {
                    AddReward(-1.0f);
                    Debug.Log("agent damage");
                    damageCheck = true;
                }
                preSelfHp = currentSelfHp;

                

                if (preSelfHp < 30 && level2)
                {
                    level2 = false;
                    parentTransform.GetComponent<TopDownCharacterController>().UseSpecial();
                    Create(false);
                }

                if (preSelfHp < 5 && level3)
                {
                    level3 = false;
                    foreach (RodObjectInfo rol in rodList)
                    {
                        Destroy(rol.gameObject);
                    }
                    rodList.Clear();
                    parentTransform.GetComponent<TopDownCharacterController>().UseSpecial();
                    Create(true);
                }
                if (preSelfHp <= 0)
                {
                    rodList.Clear();
                    if (parentTransform.GetComponent<TopDownCharacterController>().anim_finish && !death)
                    {
                        death = true;
                    }
                    target.Find("HP").GetComponent<PlayerHPInfo>().damage_switch = false;

                }
                

                if (death && !fadeEnd)
                {
                    fadeScreenInfo.SetNum(-2);
                    fadeScreenInfo.Fade(scene_name);
                    fadeEnd = true;
                }
            }

        }
        else
        {
            moveInfo.stopCheckLong = true;

            barrier.SetActive(false);
            moveInfo.SetMove(new Vector2(0,0));
        }
        
    }


    private void Create(bool level_up)
    {
        GameObject ro;
        int num = 0;//Random.Range(0,1);
        Vector2[] ro_des;
        if (num == 0)
        {

            /*
            switch (moveInfo.CurrentPosition)
            {
                case 0:
                    ro_des[0] = des_list[1];
                    ro_des[1] = des_list[2];
                    ro_des[2] = des_list[3];
                    ro_des[3] = des_list[0];
                    break;
                case 1:
                    ro_des[0] = des_list[2];
                    ro_des[1] = des_list[3];
                    ro_des[2] = des_list[0];
                    ro_des[3] = des_list[1];
                    break;
                case 2:
                    ro_des[0] = des_list[3];
                    ro_des[1] = des_list[0];
                    ro_des[2] = des_list[1];
                    ro_des[3] = des_list[2];
                    break;
                case 3:
                    ro_des[0] = des_list[0];
                    ro_des[1] = des_list[1];
                    ro_des[2] = des_list[2];
                    ro_des[3] = des_list[3];
                    break;
            }
            */
            
            ro_des = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                ro_des[i] = des_list[(i + moveInfo.CurrentPosition + 1) % 4];
            }
            ro = Instantiate(rodObject, this.transform.position, Quaternion.identity);
            ro.GetComponent<RodObjectInfo>().Initialize(ro_des, true, target);
            if (level_up)
            {
                ro.GetComponent<RodObjectInfo>().level_up = true;
            }
            rodList.Add(ro.GetComponent<RodObjectInfo>());

            ro_des = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                ro_des[i] = des_list[( 4 - i + moveInfo.CurrentPosition) % 4];
            }
            ro = Instantiate(rodObject, this.transform.position, Quaternion.identity);
            ro.GetComponent<RodObjectInfo>().Initialize(ro_des, true, target);
            if (level_up)
            {
                ro.GetComponent<RodObjectInfo>().level_up = true;
            }
            rodList.Add(ro.GetComponent<RodObjectInfo>());
            

        }
        else
        {
            ro_des = new Vector2[0];
        }
        
    }
}

