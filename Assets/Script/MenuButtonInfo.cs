using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonInfo : MonoBehaviour
{
    [SerializeField]
    GameObject sliderButton;
    [SerializeField]
    GameObject weaponChangeButton;
    [SerializeField]
    GameObject toolChangeButton;

    [SerializeField]
    Canvas canvas;

    bool menuOn = false;

    

    private void Awake()
    {
        float height = weaponChangeButton.GetComponent<RectTransform>().sizeDelta.y;
        //float ratio = Screen.currentResolution.height / 576;
        float ratio = canvas.GetComponent<RectTransform>().localScale.y;
        height = 50;
        Debug.Log(height);

        sliderButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, height*2, 0);
        weaponChangeButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-200, height, 0);
        toolChangeButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100,height, 0);

        
        
    }

    public void OnClick()
    {
        float height = weaponChangeButton.GetComponent<RectTransform>().sizeDelta.y;
        //float ratio = Screen.currentResolution.height / 576;
        float ratio = canvas.GetComponent<RectTransform>().localScale.y;
        height = height * ratio;
        Debug.Log(height);
        if (menuOn)
        {
           

            sliderButton.transform.position += new Vector3(0, -height, 0);
            weaponChangeButton.transform.position += new Vector3(0, -height, 0);
            toolChangeButton.transform.position += new Vector3(0, -height, 0);

            toolChangeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            weaponChangeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);

            menuOn = false;
        }
        else
        {
            sliderButton.transform.position += new Vector3(0, height, 0);
            weaponChangeButton.transform.position += new Vector3(0, height, 0);
            toolChangeButton.transform.position += new Vector3(0, height, 0);

            toolChangeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 200);
            weaponChangeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 200);

            menuOn = true;
        }
    }
}
