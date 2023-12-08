using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AudioPanelInfo : MonoBehaviour
{
    [SerializeField]
    Slider bgmSlider;

    public void ReSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ReSelect();
        }

    }

    public void ReSelect()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            bgmSlider.Select();
        }
    }

    public void Initialize()
    {
        bgmSlider.Select();
    }

}
