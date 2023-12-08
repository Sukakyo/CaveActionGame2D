using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManagement : MonoBehaviour
{
    [SerializeField]
    int frameRate = 60;

    private void Start()
    {
        Application.targetFrameRate = frameRate;
    }
}
