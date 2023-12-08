using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

namespace Cainos.PixelArtTopDown_Basic
{
    [System.SerializableAttribute]
    public class ValueList<Type>
    {
        public List<Type> List = new List<Type>();

        public ValueList(List<Type> list)
        {
            List = list;
        }
    }

    public class TopDownCharacterController : MonoBehaviour
    {
        [SerializeField]
        private GameManagement gameManagement;

        public enum State
        {
            idle,
            attack,
            damage,
            death,
            magic,
            getItem,
            special,
            auto,
            start
        }

        public bool weapon_mode = false;
        public int current_tool;
        [SerializeField]
        private ToolSetInfo toolSetInfo;
        





        public float normal_speed;
        public float high_speed;

        private float speed;

        enum SpeedType
        {
            slow,
            normal,
            high
        }
        [SerializeField]
        private SpeedType speed_type = SpeedType.normal;
        public const int HIGH_SPEED_SPRITE = 8;



        [SerializeField]
        private string character_type;
        //player, enemy, boss

        
        private Animator animator;

        [SerializeField]
        private GameObject move;
        [SerializeField]
        private GameObject turn;
        [SerializeField]
        private GameObject walk;
        [SerializeField]
        private GameObject weapon;
        [SerializeField]
        private GameObject itemHolder;
        [SerializeField]
        private GameObject hp;
        [SerializeField]
        private CapsuleCollider2D hpCollider;
        [SerializeField]
        private GameObject effect_object;
        [SerializeField]
        private Transform toolHolder;
        [SerializeField]
        private GameObject toolObject;
        [SerializeField]
        private ToolInfo toolInfo;
        [SerializeField]
        private SpecialObjectInfo specialInfo;
        [SerializeField]
        private GameObject damageTouch;
        [SerializeField]
        private CapsuleCollider2D damageCollider;

        private Vector2 move_dir;
        public int turn_dir_num = 0;

        public State state = State.idle;


        public bool anim_finish = false;

        public enum MoveState
        {
            normal,
            auto
        }

        
        public MoveState moveState = MoveState.normal;




        [SerializeField]
        private ValueList<Sprite>[] walk_sprites = new ValueList<Sprite>[4];
        [SerializeField]
        private ValueList<Sprite>[] special_sprites = new ValueList<Sprite>[4];
        
        private SpriteRenderer spriteRenderer;


        private bool gameOverReturn = false;

        private bool fire_on = false;

        

        
        public void Bake()
        {
            
            if (character_type == "player" || character_type == "boss")
            {
                move = this.gameObject.transform.Find("PlayerMove").gameObject;
                turn = this.gameObject.transform.Find("PlayerTurn").gameObject;
                walk = this.gameObject.transform.Find("Animate").gameObject;
                weapon = this.gameObject.transform.Find("Weapon").gameObject;
               
                
                
                specialInfo =  this.gameObject.transform.Find("SpecialObject").GetComponent<SpecialObjectInfo>();
                toolInfo = toolObject.GetComponent<ToolInfo>();

            }
            else if (character_type == "enemy")
            {
                move = this.gameObject.transform.Find("EnemyMove").gameObject;
                turn = this.gameObject.transform.Find("EnemyTurn").gameObject;
                walk = this.gameObject.transform.Find("Animate").gameObject;
                
                damageTouch = this.gameObject.transform.Find("DamageTouch").gameObject;
                damageCollider = damageTouch.GetComponent<CapsuleCollider2D>();
            }

            gameManagement = GameObject.Find("GameManager").GetComponent<GameManagement>();

        }

        

        private void Awake()
        {


            speed = normal_speed;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

           
            if (this.gameObject.tag.Equals("Player"))
            {
                itemHolder = this.gameObject.transform.Find("ItemHolder").gameObject;
                toolHolder = this.gameObject.transform.Find("ToolHolder");
                toolObject = toolHolder.Find("Tool").gameObject;

                if (gameManagement.gameMode == GameManagement.GameMode.play_game)
                {
                    ChangeTool(current_tool);
                }
                else if (gameManagement.gameMode == GameManagement.GameMode.title)
                {
                    ChangeTool(current_tool);
                    speed_type = SpeedType.high;
                    speed = high_speed;
                }

                
            }

            hp = this.gameObject.transform.Find("HP").gameObject;
            hpCollider = hp.GetComponent<CapsuleCollider2D>();

        }

        
        public void OnFireSumButton()
        {
            if (Input.GetMouseButton(0))
            {
                OnFireSum();
                
                
            }else if (Input.GetMouseButton(1))
            {
                OnFire2();
            }

        }

        public void OnFireSum(InputAction.CallbackContext context)
        {
            Debug.Log("]");
            if (context.performed)
            {
                OnFireSum();
            }

        }

        public void OnFireSum()
        {
            if (gameManagement.state == GameManagement.GameState.play)
            {
                if (gameManagement.playerMode == GameManagement.PlayerMode.none)
                {
                    if (state == State.idle)
                    {
                        OnFire1();
                    }
                }
                else if (gameManagement.playerMode == GameManagement.PlayerMode.touch)
                {
                    gameManagement.touch_object.GetComponent<TouchObjectInfo>().FireEvent();
                }
            }
            else if (gameManagement.state == GameManagement.GameState.over)
            {
                if (gameOverReturn)
                {
                    gameManagement.OnSceneLoadInstance("Title", LoadSceneMode.Single, true,0);
                }
            }
           



        }



        public void OnFire1(InputAction.CallbackContext context)
        { 
            Debug.Log("[");
            if (context.performed)
            {

                OnFire1();
            }

        }

        public void OnFire1()
        {
            if (!fire_on)
            {

                if (gameManagement.state == GameManagement.GameState.play
                  /*|| gameManagement.state == GameManagement.GameState.start*/)
                {
                    if (state == State.idle && moveState == MoveState.normal)
                    {
                        animator.SetBool("IsMoving", false);
                        animator.SetBool("IsHighMoving", false);
                        animator.SetTrigger("IsAttacking");

                        if (character_type == "player" || character_type == "boss")
                        {
                            weapon.GetComponent<WeaponInfo>().PlayAudio();
                            if (gameManagement.weaponState == GameManagement.WeaponState.special && character_type == "player")
                            {
                                toolObject.GetComponent<ToolInfo>().ScytheEffect();
                            }
                        }

                        fire_on = true;
                    }
                }

            }
        }

        public void OnFire2(InputAction.CallbackContext context)
        {
            Debug.Log("ENTER");

            if (context.performed)
            {
                OnFire2();
            }
        }

        public void OnFire2()
        {
            if (!fire_on)
            {

                if (gameManagement.state == GameManagement.GameState.play
                  /*|| gameManagement.state == GameManagement.GameState.start*/)
                {
                    //if (gameManagement.playerMode == GameManagement.PlayerMode.none)
                    //{
                    if (state == State.idle && moveState == MoveState.normal)
                    {
                        if (character_type == "player" || character_type == "boss")
                        {
                            toolObject.GetComponent<ToolInfo>().UseTool(current_tool, animator);
                        }

                        fire_on = true;
                    }
                    //}
                    /*else if (gameManagement.playerMode == GameManagement.PlayerMode.touch)
                    {
                        gameManagement.touch_object.GetComponent<TouchObjectInfo>().FireEvent(context);

                    }*/
                }
            }
            
        }




        public void OnSelect()
        {
            if (gameManagement.state == GameManagement.GameState.play
              /*|| gameManagement.state == GameManagement.GameState.start*/)
            {
                if (speed_type == SpeedType.normal)
                {
                    speed_type = SpeedType.high;
                    speed = high_speed;
                }
                else if (speed_type == SpeedType.high)
                {
                    speed_type = SpeedType.normal;
                    speed = normal_speed;
                }
            }
        }

        public void OnSpecialFire(InputAction.CallbackContext context)
        {
            
            if (context.performed)
            {
                    OnSpecialFire();       
            }
        }

        public void OnSpecialFire()
        {
            if (gameManagement.state == GameManagement.GameState.play
              /*|| gameManagement.state == GameManagement.GameState.start*/)
            {

                if (state == State.idle && moveState == MoveState.normal)
                {
                    Debug.Log("X");
                    if (specialInfo.CanUseSpecial)
                    {
                        if (character_type == "player")
                        {
                            if (gameManagement.weaponState == GameManagement.WeaponState.normal)
                            {
                                animator.SetBool("IsMoving", false);
                                UseSpecial();
                                specialInfo.UseSpecialWeapon();
                            }
                        }

                    }

                }
            }
        }


        public void OnChangeTool(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnChangeTool();
            }
        }
        

        public void OnChangeTool()
        {
            if (gameManagement.state == GameManagement.GameState.play
              /*|| gameManagement.state == GameManagement.GameState.start*/)
            {
                ToolInfo toolInfo = toolObject.GetComponent<ToolInfo>();
                List<ToolSet> availableSets = toolInfo.AvailableTools;
                int num = availableSets.FindIndex(x => x.ToolGet == toolInfo.Tools[current_tool].ToolGet);
                Debug.Log(num);
                num += 1;
                //Debug.Log(availableSets.Count);
                Debug.Log(num);
                if (num >= availableSets.Count)
                {
                    num = 0;
                }
                //Debug.Log(num);

                num = availableSets[num].ToolGet.ToolIndex;

                ChangeTool(num);
            }
        }


        public void OnActionSum(int num)
        {
            switch(num)
            {
                case 0:
                    break;
                case 1:
                    OnFire1();
                    break;
                case 2: 
                    OnFire2();
                    break;
            }
        }

        private void MovementAction()
        {
            if (state == State.getItem)
            {
                ChangeTool(current_tool);
                state = State.idle;

            }

            move_dir = Vector2.zero;
            int anim_sprites_num = 0;

            if (state == State.idle)
            {

                move_dir = move.GetComponent<MoveInfo>().Dir;
                turn_dir_num = turn.GetComponent<TurnInfo>().DirNum;

                //Debug.Log(toolHolder);
                if (this.gameObject.tag == "Player" || this.gameObject.tag == "Boss")
                {
                    ToolInfo tool_info = toolObject.GetComponent<ToolInfo>();
                    float angle;
                    Vector2 tool_dir;
                    switch (turn_dir_num)
                    {
                        case 0:
                            angle = 0;
                            tool_dir = new Vector2(0, -1);
                            break;
                        case 1:
                            angle = -90;
                            tool_dir = new Vector2(-1, 0);
                            break;
                        case 2:
                            angle = 90;
                            tool_dir = new Vector2(1, 0);
                            break;
                        case 3:
                            angle = -180;
                            tool_dir = new Vector2(0, 1);
                            break;
                        default: angle = 0; tool_dir = new Vector2(0, -1); break;
                    }
                    toolHolder.eulerAngles
                        = new Vector3(0, 0, angle);
                    tool_info.ChangeDirection(tool_dir);

                }
                anim_sprites_num = walk.GetComponent<AnimateInfo>().SpriteNum;



                //animator.SetInteger("Direction", looks_dir_num);


                if (speed_type == SpeedType.normal)
                {
                    animator.SetBool("IsHighMoving", false);
                    if (move_dir.magnitude > 0)
                    {
                        animator.SetBool("IsMoving", true);
                    }
                    else
                    {
                        animator.SetBool("IsMoving", false);
                    }
                }
                else if (speed_type == SpeedType.high)
                {
                    animator.SetBool("IsMoving", false);
                    if (move_dir.magnitude > 0)
                    {
                        animator.SetBool("IsHighMoving", true);
                    }
                    else
                    {
                        animator.SetBool("IsHighMoving", false);
                    }
                }
                if (gameManagement.weaponState == GameManagement.WeaponState.normal)
                {
                    spriteRenderer.sprite = walk_sprites[turn_dir_num].List[anim_sprites_num];
                }
                else if (gameManagement.weaponState == GameManagement.WeaponState.special)
                {
                    if (character_type == "player")
                    {
                        spriteRenderer.sprite = special_sprites[turn_dir_num].List[anim_sprites_num];
                    }
                    else
                    {
                        spriteRenderer.sprite = walk_sprites[turn_dir_num].List[anim_sprites_num];
                    }
                }



                if (character_type == "player")
                {
                    weapon.GetComponent<WeaponInfo>().ChangeDir(turn_dir_num);
                }
                GetComponent<Rigidbody2D>().velocity = move_dir * speed;
            }
            else if (state == State.attack
                  || state == State.magic
                  || state == State.getItem
                  || state == State.special)
            {

                anim_sprites_num = walk.GetComponent<AnimateInfo>().SpriteNum;
                if (gameManagement.weaponState == GameManagement.WeaponState.normal)
                {
                    spriteRenderer.sprite = walk_sprites[turn_dir_num].List[anim_sprites_num];
                }
                else if (gameManagement.weaponState == GameManagement.WeaponState.special)
                {
                    if (character_type == "player")
                    {
                        spriteRenderer.sprite = special_sprites[turn_dir_num].List[anim_sprites_num];
                    }
                    else
                    {
                        spriteRenderer.sprite = walk_sprites[turn_dir_num].List[anim_sprites_num];
                    }
                }
                if (gameManagement.gameMode == GameManagement.GameMode.play_game)
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }

            }
            else if (state == State.damage)
            {
                move_dir = hp.GetComponent<HPInfo>().Dir;
                GetComponent<Rigidbody2D>().velocity = move_dir;

            }
            else if (state == State.death)
            {
                turn_dir_num = turn.GetComponent<TurnInfo>().DirNum;
                anim_sprites_num = walk.GetComponent<AnimateInfo>().SpriteNum;

                if (gameManagement.weaponState == GameManagement.WeaponState.normal)
                {
                    spriteRenderer.sprite = walk_sprites[turn_dir_num].List[anim_sprites_num];
                }
                else if (gameManagement.weaponState == GameManagement.WeaponState.special)
                {
                    if (character_type == "player")
                    {
                        spriteRenderer.sprite = special_sprites[turn_dir_num].List[anim_sprites_num];
                    }
                    else
                    {
                        spriteRenderer.sprite = walk_sprites[turn_dir_num].List[anim_sprites_num];
                    }
                }
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else if (state == State.start)
            {
                anim_sprites_num = walk.GetComponent<AnimateInfo>().SpriteNum;

                spriteRenderer.sprite = walk_sprites[0].List[anim_sprites_num];
            }

        }

        private void Update()
        {
            
            if (gameManagement.gameMode == GameManagement.GameMode.title)
            {
                if (this.transform.position.x < -64)
                {
                    this.transform.position = new Vector2(this.transform.position.x + 128, this.transform.position.y);
                    gameManagement.ChangePositionCamera();

                }
            }

            if (gameManagement.state == GameManagement.GameState.play
              ||gameManagement.state == GameManagement.GameState.start)
            {

                MovementAction();

            }
            else if (gameManagement.state == GameManagement.GameState.talk)
            {
                animator.SetBool("IsMoving",false);
                animator.SetBool("IsHighMoving", false);
                spriteRenderer.sprite = walk_sprites[turn_dir_num].List[walk.GetComponent<AnimateInfo>().SpriteNum];
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                
                
            }
            else if (gameManagement.state == GameManagement.GameState.over)
            {
                if(character_type == "enemy")
                {
                    MovementAction();
                }
            }


        }

        private void LateUpdate()
        {
            fire_on = false;
        }


        public void ToDeath()
        {
            if (character_type == "player")
            {
                turn = this.gameObject.transform.Find("GameOverTurn").gameObject;
            }
            else if(character_type == "enemy")
            {
                
            }
            else if(character_type == "boss")
            {

            }
        }

        public void PlayAudio()
        {
            hp.GetComponent<PlayerHPInfo>().PlayAudio();
        }

        

        public void GetItem()
        {
            state = State.getItem;
        }

        public void ChangeTool(int num)
        {
            if (gameManagement.gameMode == GameManagement.GameMode.play_game)
            {


                Debug.Log(toolSetInfo);
                //Debug.Log(toolInfo.GetToolUseOnOff(num));
                
                    toolSetInfo.SetToolImage(toolInfo.Tools[num].ToolGet.ToolTexture,
                                             toolInfo.GetToolUseOnOff(num));
                    toolSetInfo.SetToolNumText((itemHolder.GetComponent<ItemHolderInfo>().SearchItemNum(toolInfo.Tools[num].ToolGet.UseItemIndex)).ToString("D3"),
                                                toolInfo.GetToolUseOnOff(num));
                    for (int i = 0; i < 3; i++)
                    {
                        walk_sprites[0].List[14 + i] = toolInfo.Tools[num].ToolGet.useSprites.frontSprites[i];
                        walk_sprites[1].List[14 + i] = toolInfo.Tools[num].ToolGet.useSprites.leftSprites[i];
                        walk_sprites[2].List[14 + i] = toolInfo.Tools[num].ToolGet.useSprites.rightSprites[i];
                        walk_sprites[3].List[14 + i] = toolInfo.Tools[num].ToolGet.useSprites.backSprites[i];

                        special_sprites[0].List[14 + i] = toolInfo.Tools[num].ToolGet.useSprites.frontSprites[i];
                        special_sprites[1].List[14 + i] = toolInfo.Tools[num].ToolGet.useSprites.leftSprites[i];
                        special_sprites[2].List[14 + i] = toolInfo.Tools[num].ToolGet.useSprites.rightSprites[i];
                        special_sprites[3].List[14 + i] = toolInfo.Tools[num].ToolGet.useSprites.backSprites[i];
                    }
                    current_tool = num;
                
            }

        }

        public void DestroySelf()
        {
            Destroy(this.gameObject);
        }

        public void UseSpecial()
        {
            animator.SetTrigger("UseSpecial");
            transform.Find("SpecialObject").GetComponent<AudioSource>().Play();
        }

        public void TurnOnObjects()
        {
            if (this.gameObject.tag == "Enemy")
            {
                GetComponent<SpriteRenderer>().enabled = true;
                Debug.Log("On");
                hp.SetActive(true);
                damageTouch.SetActive(true);
                //hp.GetComponent<CapsuleCollider2D>().enabled = true;
                //damageTouch.GetComponent<CapsuleCollider2D>().enabled = true;
            }
            
        }

        public void TurnOffObjects()
        {
            
            if (this.gameObject.tag == "Enemy")
            {
                GetComponent<SpriteRenderer>().enabled = true;
                Debug.Log("Off");
                hp.SetActive(false);
                damageTouch.SetActive(false);
                //hp.GetComponent<CapsuleCollider2D>().enabled = false;
                //damageTouch.GetComponent<CapsuleCollider2D>().enabled = false;
            }
        }

        public void InvisibleObjects()
        {

            if (this.gameObject.tag == "Enemy")
            {
                Debug.Log("Invisible");
                GetComponent<SpriteRenderer>().enabled = false;
                hp.SetActive(false);
                damageTouch.SetActive(false);
                //hp.GetComponent<CapsuleCollider2D>().enabled = false;
                //damageTouch.GetComponent<CapsuleCollider2D>().enabled = false;
            }
        }

        public void FinishAnim()
        {
            anim_finish = true;
        }

        public void GameOverPlayer()
        {
            if (this.gameObject.tag == "Player")
            {
                gameManagement.GameOver();
                gameOverReturn = true;
            }
        }
    }

    
}



