using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScreenInfo : MonoBehaviour
{
    [SerializeField]
    bool fade;

    private Animator _anim;

    [SerializeField]
    float loadSceneTime = 5f;

    float time = 0f;
    bool loadSceneSwitch = false;

    public string scene_name;

    [SerializeField]
    GameManagement gameManagement;

    public bool load;

    public int bgm_num = -1;

    

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.SetBool("StartFade" , fade);
        Fade();
    }

    public void Fade()
    {
        _anim.SetTrigger("FadeSwitch");
    }

    public void Fade(string scene_name)
    {
        
        this.scene_name = scene_name;
        loadSceneSwitch = true;
        Fade();
    }

    public void SetNum(int num)
    {
        bgm_num = num;
    }

    private void Update()
    {
        if (loadSceneSwitch)
        {
            if(time > loadSceneTime)
            {
                time = 0f;
                loadSceneSwitch = false;
                if (bgm_num != -1)
                {
                    gameManagement.OnSceneLoadInstance(scene_name, LoadSceneMode.Single, load, bgm_num);
                }
                else
                {
                    gameManagement.OnSceneLoadInstance(scene_name, LoadSceneMode.Single, load);
                }
            }
            else
            {
                time += Time.deltaTime;
            }
        }
    }
}
