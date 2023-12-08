using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Cainos.PixelArtTopDown_Basic;
using Unity.Barracuda;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Serialization;

public class BossOutputAgent : MonoBehaviour
{
    TopDownCharacterController.State _state;
    Vector2 _moveDir = Vector2.zero;
    Vector2 _movePos = Vector2.zero;
    Vector2 _turnDir = Vector2.zero;

    [FormerlySerializedAs("parentTransform")]
    [SerializeField]
    Transform _parentTransform;

    [FormerlySerializedAs("target")]
    [SerializeField]
    Transform _target; //Playerであること

    [FormerlySerializedAs("moveInfo")]
    [SerializeField]
    BossMoveInfo _moveInfo;

    [FormerlySerializedAs("turnInfo")]
    [SerializeField]
    TurnInfo _turnInfo;

    [FormerlySerializedAs("toolInfo")]
    [SerializeField]
    ToolInfo _toolInfo;

    [FormerlySerializedAs("barrier")]
    [SerializeField]
    GameObject _barrier;



    private TopDownCharacterController _controller;
    private Rigidbody2D _rigid;

    HPInfo _targetHPInfo;
    private int preTargetHp;

    HPInfo _pHpiSelf;
    private int preSelfHp;

    [FormerlySerializedAs("firstPos")]
    [SerializeField]
    Vector3 _firstPos;

    private bool damageCheck = false;
    private bool endEpisode = false;

    private Vector2 _preVel = Vector2.zero;

    [FormerlySerializedAs("_target")]
    [SerializeField]
    private RenderTexture _fieldTexture;
    private Vector4[] _colorVectors;


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
    [FormerlySerializedAs("des_list")]
    [SerializeField]
    Vector2[] _desList = new Vector2[4];

    List<RodObjectInfo> rodList = new List<RodObjectInfo>();

    [SerializeField]
    FadeScreenInfo fadeScreenInfo;

    [FormerlySerializedAs("scene_name")]
    [SerializeField]
    string sceneName;

    [FormerlySerializedAs("modelAsset")]
    [SerializeField]
    NNModel _modelAsset;
    private Model m_RuntimeModel;
    private IWorker m_worker;


    private float time = 0f;

    [FormerlySerializedAs("max_time")]
    [SerializeField]
    private float maxTime = 0.25f;
    private int frameCount = 0;


    private int discreteAction = 0;

    float[] list = new float[2254];
    Color[] _colors;


    private float _barrierTime = 0f;
    [SerializeField]
    private float maxBarrierTime = 0.5f;


    private Color[] GetPixels()
    {
        // アクティブなレンダーテクスチャをキャッシュしておく
        RenderTexture currentRT = RenderTexture.active;

        // アクティブなレンダーテクスチャを一時的にTargetに変更する
        RenderTexture.active = _fieldTexture;

        // Texture2D.ReadPixels()によりアクティブなレンダーテクスチャのピクセル情報をテクスチャに格納する
        Texture2D texture = new Texture2D(_fieldTexture.width, _fieldTexture.height);
        texture.ReadPixels(new Rect(0, 0, _fieldTexture.width, _fieldTexture.height), 0, 0);
        texture.Apply();

        // ピクセル情報を取得する
        Color[] colors = texture.GetPixels();

        // アクティブなレンダーテクスチャを元に戻す
        RenderTexture.active = currentRT;

        return colors;
    }

    private void Start()
    {
        _targetHPInfo = _target.Find("HP").GetComponent<HPInfo>();

        preTargetHp = _targetHPInfo.max_hp;

        _pHpiSelf = _parentTransform.Find("HP").GetComponent<HPInfo>();
        preSelfHp = _pHpiSelf.max_hp;

        _controller = _parentTransform.GetComponent<TopDownCharacterController>();
        _rigid = _parentTransform.GetComponent<Rigidbody2D>();

        //controller.OnSelect();


        m_RuntimeModel = ModelLoader.Load(_modelAsset);

        WorkerFactory.Type worker_type = WorkerFactory.Type.Compute;

        m_worker = WorkerFactory.CreateWorker(worker_type, m_RuntimeModel);
        
    }


    public void GetParam(TopDownCharacterController.State st, Vector2 mv_dir, Vector2 mv_pos, Vector2 tr_dir)
    {
        _state = st;
        _moveDir = mv_dir;
        _movePos = mv_pos;
        _turnDir = tr_dir;
    }

    
    private async void Action()
    {
        await Task.Run(() => UpdateAction());
    }

    private void UpdateAction()
    {



        float targetPosX;
        float targetPosY;
        if (_target != null)
        {
            targetPosX = _target.position.x;
            targetPosY = _target.position.y;
        }
        else
        {
            targetPosX = 0;
            targetPosY = 0;
        }

        GetParam(_controller.state, _moveInfo.move_dir, _parentTransform.position, _turnInfo.move_dir);

        //color_vectors = new Vector4[colors.Length];



        list[0] = ((targetPosX - (-68f)) / 26f);
        list[1] = ((targetPosY - 15f) / 16f);
        list[2] = ((int)_state / 7f);
        list[3] = (_moveDir.normalized.x);
        list[4] = (_moveDir.normalized.y);
        list[5] = ((_movePos.x - (-68f)) / 26f);
        list[6] = ((_movePos.y - 15f) / 16f);
        list[7] = (_turnDir.normalized.x);
        list[8] = (_turnDir.normalized.y);
        list[9] = (Convert.ToSingle(_moveInfo.goToCheck));

        _colors = GetPixels();

        // お試し
        for (int i = 0; i < _colors.Length; i++)
        {
            list[10 + i * 3] = (_colors[i].r);
            list[10 + 1 + i * 3] = (_colors[i].g);
            list[10 + 2 + i * 3] = (_colors[i].b);
            //list.Add(colors[i].a);
        }
        frameCount++;
        

        Tensor input = new Tensor(new int[1] { list.Length }, list);

            int[] arr2 = new int[10];
            Array.Fill<int>(arr2, 0);
            float[] new_arr2 = arr2.Select(s => (float)s).ToArray();
            Tensor mask = new Tensor(new int[1] { arr2.Length }, new_arr2);

            Dictionary<string, Tensor> inputs = new Dictionary<string, Tensor>() { { "obs_0", input }, { "action_masks", mask } };

            Debug.Log(list.Length);


            m_worker.Execute(inputs);
            Tensor output = m_worker.PeekOutput("discrete_actions");
            Debug.Log(output[0]);
            discreteAction = (int)output[0];
            Debug.Log(discreteAction);



            input.Dispose();
            mask.Dispose();
            output.Dispose();

            time = 0f;
            frameCount = 0;
        
    }

    private void Update()
    {
        

        if (time > maxTime)
        {
            UpdateAction();
        }
        else
        {
            time += Time.deltaTime;
            Debug.Log(time);
        }


        if (!first)
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



        if (first && preTargetHp > 0)
        {
            _moveInfo.stopCheckLong = false;


            rodList.RemoveAll(x => x == null);

            

            GetParam(_controller.state, _moveInfo.move_dir, _parentTransform.position, _turnInfo.move_dir);

            if (firstSpecial)
            {
                _turnInfo.SetTurn(new Vector2(0, -1));
                _parentTransform.GetComponent<TopDownCharacterController>().UseSpecial();
                firstSpecial = false;
            }
            else
            {




                if (endEpisode)
                {
                    //EndEpisode();
                    endEpisode = false;
                }


                //int num = Random.Range(0, 10);

                
                int num = discreteAction;
                Debug.Log("num:" + num);



                if (!_moveInfo.goToCheck)
                {


                    
                    if (num == 0)
                    {

                        
                    }
                    else
                    {
                        _barrier.SetActive(false);
                        _barrierTime = 0;

                        int current_pos = _moveInfo.CurrentPosition;
                        Vector2 tool_dir = Vector2.zero;
                        switch (current_pos)
                        {
                            case 0:
                                _turnInfo.SetTurn(new Vector2(0, -1));
                                _parentTransform.GetComponent<TopDownCharacterController>().turn_dir_num = 0;
                                tool_dir = new Vector2(0, -1);
                                break;
                            case 1:
                                _turnInfo.SetTurn(new Vector2(-1, 0));
                                _parentTransform.GetComponent<TopDownCharacterController>().turn_dir_num = 1;
                                tool_dir = new Vector2(-1, 0);
                                break;
                            case 2:
                                _turnInfo.SetTurn(new Vector2(0, 1));
                                _parentTransform.GetComponent<TopDownCharacterController>().turn_dir_num = 3;
                                tool_dir = new Vector2(0, 1);
                                break;
                            case 3:
                                _turnInfo.SetTurn(new Vector2(1, 0));
                                _parentTransform.GetComponent<TopDownCharacterController>().turn_dir_num = 2;
                                tool_dir = new Vector2(1, 0);
                                break;
                        }
                        _toolInfo.ChangeDirection(tool_dir);


                        if (coolDown)
                        {
                            _controller.OnActionSum(2);
                            foreach (RodObjectInfo roi in rodList)
                            {
                                roi.Fire();
                            }
                            //AddReward(-0.2f);
                            coolDown = false;
                        }

                        
                    }
                    _moveInfo.ChangeGoTo(UnityEngine.Random.Range(0, 2));
                }
                else
                {
                    

                    Vector2 dir = _target.transform.position - this.transform.position;

                    float angle = Vector2.SignedAngle(new Vector2(0, -1), dir);
                    BarrierInfo barrierInfo = _barrier.GetComponent<BarrierInfo>();
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



                    if (_moveInfo.Dir.y < 0.3f && _moveInfo.Dir.y > -0.3f)
                    {
                        _turnInfo.SetTurn(new Vector2(_moveInfo.Dir.x, 0));
                    }
                    else
                    {
                        _turnInfo.SetTurn(_moveInfo.Dir);
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
                if (!_barrier.activeSelf)
                {
                    if(_barrierTime > maxBarrierTime)
                    {
                        _barrierTime = 0;
                        _barrier.SetActive(true);
                    }
                    else
                    {
                        _barrierTime += Time.deltaTime;
                    }
                }



                if (Vector2.Distance(_parentTransform.position, _target.position) < 1.5f)
                {
                    _barrier.SetActive(false);
                    _controller.OnActionSum(1);
                }




                int currentTargetHp = _targetHPInfo.GetHP();
                if (currentTargetHp < preTargetHp)
                {

                    //AddReward((1000 - this.StepCount) / 1000);
                    Debug.Log("target damage");
                    damageCheck = true;
                }
                preTargetHp = currentTargetHp;



                int currentSelfHp = _pHpiSelf.GetHP();
                if (currentSelfHp < preSelfHp)
                {
                    //AddReward(-1.0f);
                    Debug.Log("agent damage");
                    damageCheck = true;
                }
                preSelfHp = currentSelfHp;


                if (preSelfHp < 30 && level2)
                {
                    level2 = false;
                    _parentTransform.GetComponent<TopDownCharacterController>().UseSpecial();
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
                    _parentTransform.GetComponent<TopDownCharacterController>().UseSpecial();
                    Create(true);
                }
                if (preSelfHp <= 0)
                {
                    rodList.Clear();
                    if (_parentTransform.GetComponent<TopDownCharacterController>().anim_finish && !death)
                    {
                        death = true;
                    }
                    _target.Find("HP").GetComponent<PlayerHPInfo>().damage_switch = false;

                }
                if (preTargetHp <= 0)
                {
                    _parentTransform.Find("HP").GetComponent<PlayerHPInfo>().damage_switch = false;
                }

                if (death && !fadeEnd)
                {
                    fadeScreenInfo.SetNum(-2);
                    fadeScreenInfo.Fade(sceneName);
                    fadeEnd = true;
                }
            }

        }
        else
        {
            _moveInfo.stopCheckLong = true;

            _barrier.SetActive(false);
            _moveInfo.SetMove(new Vector2(0, 0));
        }


    }

    private void OnDestroy()
    {
        
        m_worker.Dispose();
    }


    private void Create(bool level_up)
    {
        GameObject ro;
        int num = 0;//Random.Range(0,1);
        Vector2[] ro_des;
        if (num == 0)
        {

            
            
            ro_des = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                ro_des[i] = _desList[(i + _moveInfo.CurrentPosition + 1) % 4];
            }
            ro = Instantiate(rodObject, this.transform.position, Quaternion.identity);
            ro.GetComponent<RodObjectInfo>().Initialize(ro_des, true, _target);
            if (level_up)
            {
                ro.GetComponent<RodObjectInfo>().level_up = true;
            }
            rodList.Add(ro.GetComponent<RodObjectInfo>());

            ro_des = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                ro_des[i] = _desList[( 4 - i + _moveInfo.CurrentPosition) % 4];
            }
            ro = Instantiate(rodObject, this.transform.position, Quaternion.identity);
            ro.GetComponent<RodObjectInfo>().Initialize(ro_des, true, _target);
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

