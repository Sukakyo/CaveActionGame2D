using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChangeTriggerInfo : MonoBehaviour
{
    [SerializeField]
    string sceneName;

    [SerializeField]
    FadeScreenInfo fadeScreenInfo;

    [SerializeField]
    bool clear = false;

    [SerializeField]
    GameManagement gameManagement;

    [SerializeField]
    Animator gameManagerAnim;


    bool start = false;
    bool destroy = false;
    float time = 0;
    float maxTime = 0.3f;


    GameObject _tmp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (clear)
        {
            start = true;
            gameManagerAnim.SetTrigger("IsClear");
            
            _tmp = collision.gameObject;
        }
        else
        {
            if (sceneName != null)
            {
                fadeScreenInfo.Fade(sceneName);
            }
        }

        
    }

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnEnd();
        }
    }

    public void OnEnd()
    {
        if (gameManagement.state == GameManagement.GameState.clear)
        {
            gameManagement.OnSceneLoadInstance(sceneName, LoadSceneMode.Single, true, 1);
        }
    }


    private void Update()
    {
        if (start)
        {
            if (time > maxTime)
            {
                destroy = true;
                start = false;
            }
            else
            {
                time += Time.deltaTime;
            }
        }



        if (destroy)
        {
            gameManagement.GameClear();
            Destroy(_tmp);
            destroy = false;
        }
    }
}
