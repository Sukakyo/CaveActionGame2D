using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetChangeInfo : MonoBehaviour
{
    [SerializeField]
    CameraFollow cameraFollow;

    bool touch = false;

    [SerializeField]
    Transform newTarget;

    [SerializeField]
    FadeScreenInfo fadeScreenInfo;

    [SerializeField]
    string scene_name;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!touch)
        {
            touch = true;
            cameraFollow.ChangeTarget(newTarget);
            fadeScreenInfo.Fade(scene_name);
        }
    }
}
