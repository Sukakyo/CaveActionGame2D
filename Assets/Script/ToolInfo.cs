using Cainos.PixelArtTopDown_Basic;
using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ToolInfo : MonoBehaviour
{
    [SerializeField]
    private ToolSet[] tools;
    public ToolSet[] Tools { get { return tools; } set { tools = value; } }

    [SerializeField]
    private List<ToolSet> availableTools = new List<ToolSet>();
    public List<ToolSet> AvailableTools { get {  return availableTools; } set { availableTools = value; } }

    [SerializeField]
    ItemHolderInfo itemHolderInfo;

    private Vector2 _direction;


    [SerializeField]
    private GameObject scytheEffectPrehab;

    [SerializeField]
    private GameObject fireBallPrehab;

    [SerializeField]
    private string sortingLayer;


    [SerializeField]
    private float cool_time = 0.5f;

    private float time = 0;
    private bool canUse = true;


    private void Update()
    {
        if(!canUse)
        {
            if (time > cool_time)
            {
                canUse = true;
                time = 0;
            }
            else
            {
                time += Time.deltaTime;
            }
        }
    }


    public void Bake()
    {
        availableTools.Clear();
        foreach (ToolSet toolset in tools)
        {
            if(toolset.tool_on_off == true)
            {
                availableTools.Add(toolset);
            }
        }
    }

    public bool CheckAvailable(int num)
    {
        return tools[num].tool_on_off;
    }

    public void ScytheEffect()
    {
        GameObject scytheEffect = Instantiate(scytheEffectPrehab, this.transform);

        this.gameObject.transform.DetachChildren();

        scytheEffect.GetComponent<FireBallInfo>().startDirection = _direction;
        scytheEffect.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;

        //fireball‚Æ“¯‚¶
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = tools[2].AudioClipGet;
        audioSource.Play();

    }


    public void ChangeDirection(Vector2 dir)
    {
        _direction = dir;
    }

    public void SetLayer(string sortingLayer)
    {
        this.sortingLayer = sortingLayer;
    }

    public void UseTool(int index,Animator player_animator)
    {
        if (canUse)
        {
            if (tools[index].tool_on_off)
            {
                if (itemHolderInfo.SearchItemNum(tools[index].ToolGet.UseItemIndex)
                    >= tools[index].ToolGet.UseItemNum)
                {
                    Debug.Log(true);
                    itemHolderInfo.MinusItem(index, this.transform.parent.parent.GetComponent<TopDownCharacterController>().current_tool, tools[index].ToolGet.UseItemNum);

                    GameObject fireball;
                    switch (index)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            fireball = Instantiate(fireBallPrehab, this.transform);

                            this.gameObject.transform.DetachChildren();

                            fireball.GetComponent<FireBallInfo>().startDirection = _direction;
                            fireball.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
                            player_animator.SetBool("IsMoving", false);
                            player_animator.SetTrigger("UseMagic");
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        case 6:
                            for (int i = -1; i < 2; i++)
                            {
                                fireball = Instantiate(fireBallPrehab, this.transform);
                                this.gameObject.transform.DetachChildren();

                                Vector3 angles = new Vector3(0, 0, 30) * i;
                                fireball.GetComponent<FireBallInfo>().startDirection = (Quaternion.Euler(angles) * _direction);
                                fireball.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;

                            }


                            player_animator.SetBool("IsMoving", false);
                            player_animator.SetTrigger("UseMagic");
                            break;

                    }
                    AudioSource audioSource = GetComponent<AudioSource>();
                    audioSource.clip = tools[index].AudioClipGet;
                    audioSource.Play();
                    canUse = false;
                }
            }
        }
        
        
    }

    public bool GetToolUseOnOff(int index)
    {
        return tools[index].tool_on_off;
    }
}

[System.SerializableAttribute]
public class ToolSet
{
    [SerializeField]
    private Tool tool;
    public Tool ToolGet { get { return tool; } }

    [SerializeField]
    private AudioClip audioClip;

    public AudioClip AudioClipGet { get { return audioClip; } }

    public bool tool_on_off;

}
