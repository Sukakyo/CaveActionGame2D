using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cainos.PixelArtTopDown_Basic;

public class MovieManagement : MonoBehaviour
{
    [SerializeField]
    MovieSet[] movieSets;

    [SerializeField]
    Image ui_image;

    [SerializeField]
    TalkTextInfo talkTextInfo;

    [SerializeField]
    CameraFollow main_camera;

    private Sprite[] _useSprites;

    private int useNum;
    [SerializeField]
    private int debug_num;

    

    private void Awake()
    {
        string pre_scene = GameManagement.gameData.pre_scene_name;
        int i = 0;
        for (i = 0; i < movieSets.Length; i++)
        {
            if (pre_scene == movieSets[i].PreScene)
            {
                break;
            }
        }

        foreach (MovieSet ms in movieSets)
        {
            ms.TimeLineManagementGet.gameObject.SetActive(false);
        }

        if (i != movieSets.Length)
        {
            movieSets[i].TimeLineManagementGet.gameObject.SetActive(true);
            _useSprites = movieSets[i].ImagesGet;
            talkTextInfo.SetImages(_useSprites);
            ui_image.sprite = _useSprites[0];
            useNum = i;
            main_camera.target = movieSets[i].Target;
        }
        else
        {
            movieSets[debug_num].TimeLineManagementGet.gameObject.SetActive(true);
            _useSprites = movieSets[debug_num].ImagesGet;
            talkTextInfo.SetImages(_useSprites);
            ui_image.sprite = _useSprites[0];
            useNum = debug_num;
            main_camera.target = movieSets[debug_num].Target;
        }
    }


    public void ChangeImage(int num)
    {
        ui_image.sprite = _useSprites[num];
    }

    public void ChangeTarget(Transform _target)
    {
        main_camera.target = _target;
    }
    
}

[System.Serializable]
public class MovieSet
{
    [SerializeField]
    TimeLineManagement timeLineManagement;

    public TimeLineManagement TimeLineManagementGet { get { return timeLineManagement; } }

    [SerializeField]
    string pre_scene = "";

    public string PreScene {get{return pre_scene;}}

    [SerializeField]
    Sprite[] images;

    public Sprite[] ImagesGet { get { return images; } }

    [SerializeField]
    Transform target;

    public Transform Target { get { return target; } }
}
