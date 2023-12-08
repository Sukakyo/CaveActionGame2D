using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAudioInfo : MonoBehaviour
{
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
