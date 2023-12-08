using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class TimeLineManagement : MonoBehaviour
{
    [SerializeField]
    PlayableDirector playableDirector;

    [SerializeField]
    GameManagement gameManagement;

    [SerializeField]
    TextAsset[] textAssets;


    public void PlayText(int index)
    {
        playableDirector.Pause();
        gameManagement.TalkStart(textAssets[index]);
    }

    private void Start()
    {
        playableDirector.Play();
    }

    private void Update()
    {
        if (gameManagement.TalkStopCheck())
        {
            playableDirector.Resume();
        }
    }

    
}