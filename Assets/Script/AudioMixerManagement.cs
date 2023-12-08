using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManagement : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;

    [SerializeField]
    Slider bgmSlider;

    [SerializeField]
    Slider seSlider;

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(SetAudioMixerBGM);
        seSlider.onValueChanged.AddListener(SetAudioMixerSE);

        
    }

    public void SetAudioMixerBGM(float value)
    {
        value /= 5;

        float volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 80f);

        

        audioMixer.SetFloat("BGM", volume);
        Debug.Log($"BGM:{volume}");
    }

    public void SetAudioMixerSE(float value)
    {
        value /= 5;

        float volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 80f);

        

        audioMixer.SetFloat("SE", volume);
        Debug.Log($"SE:{volume}");
        GetComponent<AudioSource>().Play();
    }

}
