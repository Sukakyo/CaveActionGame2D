using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TitleSelectManagement : MonoBehaviour
{
    [SerializeField]
    GameObject selectPanel;
    [SerializeField]
    GameObject audioPanel;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    GameManagement gameManagement;

    public void OnFire(InputAction.CallbackContext context)
    {
        if (audioPanel.activeSelf)
        {
            if (gameManagement.state == GameManagement.GameState.play)
            {
                if (context.performed)
                {
                    Debug.Log("debug now");
                    GameObject selectedObj = eventSystem.currentSelectedGameObject;

                    if (selectedObj != null)
                    {
                        if (selectedObj.CompareTag("Exit"))
                        {
                            Select(0);
                        }
                    }
                }
            }
        }

    }

    public void Select(int num)
    {
        switch (num)
        {

            case 0:
                selectPanel.SetActive(true);
                audioPanel.SetActive(false);
                selectPanel.GetComponent<SelectTextInfo>().Initialize();
                break;
            case 1:
                selectPanel.SetActive(false);
                audioPanel.SetActive(true);
                audioPanel.GetComponent<AudioPanelInfo>().Initialize();
                break;
        }
    }
}
