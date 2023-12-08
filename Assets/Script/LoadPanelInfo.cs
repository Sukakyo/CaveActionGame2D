using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelInfo : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    float time = 0;
    const float ScaleTime = 1;
    Image _image;

    private void Awake()
    {
        _image = this.transform.Find("Image").GetComponent<Image>();
    }


    private void Update()
    {
        time += Time.unscaledDeltaTime;

        if (time > ScaleTime)
        {
            time = 0;
        }
        else
        {
            if(time >= 0 && time < 0.25f)
            {
                _image.sprite = sprites[0];
            }
            else if (time >= 0.25f && time  < 0.5f)
            {
                _image.sprite = sprites[1];
            }
            else if (time >= 0.5f && time < 0.75f)
            {
                _image.sprite = sprites[2];
            }
            else
            {
                _image.sprite = sprites[3];
            }

        }
    }
}
