using UnityEngine;
using UnityEngine.Serialization;

public class BGMInfo : MonoBehaviour
{
    [FormerlySerializedAs("introLoopAudio")]
    [SerializeField] private IntroLoopAudio _introLoopAudio;


   
    private void Awake()
    {
        DontDestroy();
        //introLoopAudio = this.transform.Find("BGMPlayer").GetComponent<IntroLoopAudio>();
    }


    private void Start()
    {
        _introLoopAudio.Play();
    }

    public void OnClickPlay()
    {
        _introLoopAudio.Play();
    }
    public void OnClickPause()
    {
        _introLoopAudio.Pause();
    }
    public void OnClickStop()
    {
        _introLoopAudio.Stop();
    }


    public void DontDestroy()
    {
        DontDestroyOnLoad(this);
    }

    public void SetMusic(AudioClip intro, AudioClip loop)
    {
        _introLoopAudio.SetMusic(intro, loop);
    }
}