using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWall : MonoBehaviour
{
    [SerializeField]
    bool awake_animation = true;
    Animator _animator;
    AudioSource _audioSource;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (awake_animation)
        {
            _animator.SetTrigger("animation");
        }
        else
        {
            _animator.SetTrigger("not_animation");
        }
    }

    public void PlayAudio()
    {
        _audioSource.Play();
    }
}
