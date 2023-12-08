using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using System;
//using UnityEngine.UIElements;

public class GameManagement : MonoBehaviour
{

    [SerializeField]
    private CameraFollow cameraInfo;

    public enum GameState
    {
        start,
        play,
        pause,
        talk,
        clear,
        over
    }

    [SerializeField]
    GameObject startPanel;

    [SerializeField]
    GameObject playPanel;
    [SerializeField]
    List<OnScreenButton> playPanelButton = new List<OnScreenButton>();

    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    List<OnScreenButton> pausePanelButton = new List<OnScreenButton>();

    [SerializeField] 
    GameObject talkPanel;
    [SerializeField]
    List<OnScreenButton> talkPanelButton = new List<OnScreenButton>();

    [SerializeField]
    GameObject clearPanel;
    [SerializeField]
    List<OnScreenButton> clearPanelButton = new List<OnScreenButton>();

    [SerializeField]
    GameObject overPanel;
    [SerializeField]
    List<OnScreenButton> overPanelButton = new List<OnScreenButton>();

    [SerializeField]
    GameObject loadPanel;

    [SerializeField]
    GameObject[] timeLines;
    

    public GameState state = GameState.start;

    public enum PlayerMode
    {
        none,
        touch
    }

    public PlayerMode playerMode = PlayerMode.none;

    public GameObject touch_object;
    public TextAsset csv_text;

    public enum WeaponState
    {
        normal,
        special
    }

    public WeaponState weaponState;

    public enum GameMode
    {
        title,
        play_game
    }
    [SerializeField]
    public GameMode gameMode = GameMode.play_game;

    [SerializeField]
    GameObject[] dontDestroyObjects;

    [SerializeField]
    private GameObject BGMSwitch;

    [SerializeField]
    private AudioSet[] audioSets;


    [SerializeField]
    private GameData gd;

    public static GameData gameData = new GameData();

    
    private bool canToTitle = false;

    private void Awake()
    {
        gd = gameData;

        if (gd.change_scene)
        {
            GameObject player;
            if (gd.pre_scene_name != "Title" && gd.pre_scene_name != "stage0_start")
            {
                
                try
                {
                    player = GameObject.Find("Player");
                    Debug.Log(player);
                }
                catch
                {
                    player = null;
                }

                if (player != null)
                {
                    HPInfo hpinfo;
                    ItemHolderInfo itemholderinfo;
                    ToolInfo toolsinfo;
                    try
                    {
                        hpinfo = player.transform.Find("HP").GetComponent<HPInfo>();
                        itemholderinfo = player.transform.Find("ItemHolder").GetComponent<ItemHolderInfo>();
                        toolsinfo = player.transform.Find("ToolHolder").Find("Tool").GetComponent<ToolInfo>();
                        Debug.Log(hpinfo);
                        Debug.Log(itemholderinfo);
                        Debug.Log(toolsinfo);
                    }
                    catch
                    {
                        hpinfo = null;
                        itemholderinfo = null;
                        toolsinfo = null;
                    }

                    if (hpinfo != null && itemholderinfo != null && toolsinfo != null)
                    {
                        TopDownCharacterController playerController = player.GetComponent<TopDownCharacterController>();
                        toolsinfo.Tools = gameData.tools;
                        toolsinfo.AvailableTools = gameData.availableTools;

                        hpinfo.Hp = gameData.hp;
                        //Debug.Log(gameData.hp);
                        itemholderinfo.Items = gameData.items;
                        //Debug.Log(itemholderinfo.Items[3].ItemNum);
                        //Debug.Log(gameData.items[3].ItemNum);

                        //Debug.Log(hpinfo.Hp);
                        playerController.Bake();

                        playerController.ChangeTool(gameData.current_tool);
                        //Debug.Log(playerController.current_tool);
                        //Debug.Log(gameData.current_tool);

                    }
                }

                
            }

            switch (gd.pre_scene_name)
            {
                case "stage0_boss_1":
                    foreach (GameObject tl in timeLines)
                    {
                        tl.SetActive(false);
                    }

                    timeLines[0].SetActive(true);
                    break;
                case "stage0_start":
                    try
                    {
                        player = GameObject.Find("Player");
                        Debug.Log(player);
                    }
                    catch
                    {
                        player = null;
                    }

                    if (player != null)
                    {
                        Animator animator = player.GetComponent<Animator>();
                        animator.SetBool("start", true);
                    }
                    break;
                case "Title":
                    gameData.start_time = Time.time;
                    break;
            }
        }
        else
        {
            foreach(GameObject gb in dontDestroyObjects) {
                GameObject gbIns = Instantiate(gb);
                Transform loadPanelTrans = gbIns.transform.Find("LoadPanel");
                if(loadPanelTrans != null)
                {
                    gd.sloadPanel = loadPanelTrans.gameObject;
                }
                if (gbIns.tag == "BGMSwitch")
                {
                    gameData.BGMSwitch = gbIns;
                    BGMSwitch = gbIns;
                }
            }
            BGMInfo bgmInfo = BGMSwitch.GetComponent<BGMInfo>();
            bgmInfo.OnClickStop();
            bgmInfo.SetMusic(audioSets[0].introClip, audioSets[0].loopClip);
            bgmInfo.OnClickPlay();
        }

        loadPanel = gd.sloadPanel;

        loadPanel.SetActive(false);

        BGMSwitch = gameData.BGMSwitch;
    }


    private void Start()
    {
        /*
        if (gd.change_scene)
        {

            Debug.Log(gameData.pre_scene_name);
            SceneManager.UnloadSceneAsync(gameData.pre_scene_name);

        }
        */

        
    }


    public void SetActivePanel()
    {
        ChangePanel();
        
    }

    void ChangePanel()
    {
        
        switch (state)
        {
            case GameState.start:


                startPanel.SetActive(true);
                playPanel.SetActive(false);
                pausePanel.SetActive(false);
                talkPanel.SetActive(false);
                clearPanel.SetActive(false);
                overPanel.SetActive(false);
                break;
            case GameState.play:


                startPanel.SetActive(false);
                playPanel.SetActive(true);
                pausePanel.SetActive(false);
                talkPanel.SetActive(false);
                clearPanel.SetActive(false);
                overPanel.SetActive(false);
                break;
            case GameState.pause:


                startPanel.SetActive(false);
                playPanel.SetActive(false);
                pausePanel.SetActive(true);
                talkPanel.SetActive(false);
                clearPanel.SetActive(false);
                overPanel.SetActive(false);

                break;
            case GameState.talk:


                startPanel.SetActive(false);
                playPanel.SetActive(false);
                pausePanel.SetActive(false);
                talkPanel.SetActive(true);
                clearPanel.SetActive(false);
                overPanel.SetActive(false);
                break;
            case GameState.clear:


                startPanel.SetActive(false);
                playPanel.SetActive(false);
                pausePanel.SetActive(false);
                talkPanel.SetActive(false);
                clearPanel.SetActive(true);
                overPanel.SetActive(false);
                break;
            case GameState.over:


                startPanel.SetActive(false);
                playPanel.SetActive(false);
                pausePanel.SetActive(false);
                talkPanel.SetActive(false);
                clearPanel.SetActive(false);
                overPanel.SetActive(true);
                break;
        }

        
    }

    public void TalkStart(TextAsset textAsset)
    {
        csv_text = textAsset;
        talkPanel.transform.Find("TalkObjects").GetComponent<TalkTextInfo>().StartTalk();
        GetComponent<Animator>().SetBool("IsTalkTime", true);
    }

    public bool TalkStopCheck()
    {
        bool tmp = talkPanel.transform.Find("TalkObjects").GetComponent<TalkTextInfo>().talk_finish;
        return tmp;
    }

    public void TalkFinish()
    {
        talkPanel.transform.Find("TalkObjects").GetComponent<TalkTextInfo>().talk_finish = false;
    }


    public void ChangePositionCamera()
    {
        cameraInfo.ChangePosition();
    }



    public void OnSceneLoadInstance(string scene_name, LoadSceneMode mode, bool load)
    {
        GameManagement.OnActiveSceneLoaded();
        if (load)
        {
            loadPanel.SetActive(true);
        }
        //playerImage = loadPanel.transform.Find("Image").GetComponent<Image>();
        SceneManager.LoadScene(scene_name, mode);
        //StartCoroutine(LoadScene(scene_name, mode));
    }

    public void OnSceneLoadInstance(string scene_name, LoadSceneMode mode, bool load,int bgmNum)
    {
        ChangeMusic(bgmNum);
        

        GameManagement.OnActiveSceneLoaded();
        if (load)
        {
            loadPanel.SetActive(true);
        }

       
        //playerImage = loadPanel.transform.Find("Image").GetComponent<Image>();
        SceneManager.LoadScene(scene_name, mode);
        //StartCoroutine(LoadScene(scene_name, mode));
    }

    public void ChangeMusic(int bgmNum)
    {
        BGMInfo bgmInfo = BGMSwitch.GetComponent<BGMInfo>();
        bgmInfo.OnClickStop();
        if (bgmNum != -2)
        {
            bgmInfo.SetMusic(audioSets[bgmNum].introClip, audioSets[bgmNum].loopClip);
            bgmInfo.OnClickPlay();
        }
    }



    /*public static void OnSceneLoadLinq(string scene_name,LoadSceneMode mode)
    {


        OnActiveSceneLoaded(SceneManager.GetSceneByName(scene_name));
        
    }*/



    private static void OnActiveSceneLoaded()
    {
        GameObject player;
        gameData.change_scene = true;
        try
        {
            player = GameObject.Find("Player");
        }
        catch
        {
            player = null;
        }


        if (player != null)
        {
            HPInfo hpinfo;
            ItemHolderInfo itemholderinfo;
            ToolInfo toolsinfo;
            try
            {
                hpinfo = player.transform.Find("HP").GetComponent<HPInfo>();
                itemholderinfo = player.transform.Find("ItemHolder").GetComponent<ItemHolderInfo>();
                toolsinfo = player.transform.Find("ToolHolder").Find("Tool").GetComponent<ToolInfo>();
            }
            catch
            {
                hpinfo = null;
                itemholderinfo = null;
                toolsinfo = null;
            }

            if (hpinfo != null && itemholderinfo != null && toolsinfo != null)
            {
                TopDownCharacterController playerController = player.GetComponent<TopDownCharacterController>();
                gameData.current_tool = playerController.current_tool;
                
                gameData.hp = hpinfo.Hp;
                
                gameData.items = itemholderinfo.Items;
                gameData.tools = toolsinfo.Tools;
                gameData.availableTools = toolsinfo.AvailableTools;
                
            }
        }
        gameData.pre_scene_name = SceneManager.GetActiveScene().name;

    }

    IEnumerator LoadScene(string scene_name,LoadSceneMode mode)
    {
        
        
        AsyncOperation async = SceneManager.LoadSceneAsync(scene_name,mode);
        while (!async.isDone)
        {

            yield return null;

        }
        
    }


    public void GameClear()
    {
        int time = (int)(Time.time - gameData.start_time);
        Debug.Log(time);
        int second = time % 60;

        int minute = (time / 60) % 60;
        int hour = (time / 60) / 60;

        clearPanel.transform.Find("TimeText").GetComponent<TextMeshProUGUI>().text += hour.ToString("D3") + ":" + minute.ToString("D2") + ":" + second.ToString("D2");
    }


    public void ChangeCanTitle()
    {
        canToTitle = true;
        GetComponent<Animator>().SetBool("IsClear", true);   
    }

    public void GoToTitle()
    {
        if (canToTitle)
        {
            OnSceneLoadInstance("Title", LoadSceneMode.Single, true, 0);
        }
    }

    public void GoToTitle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GoToTitle();
        }
    }

    public void GameOver()
    {
        GetComponent<Animator>().SetBool("IsOver", true);
    }


}

[System.SerializableAttribute]
public class GameData
{
    public bool change_scene = false;

    public int hp;
    public ItemStorage[] items;
    public ToolSet[] tools;
    public List<ToolSet> availableTools;
    public int current_tool;

    public string pre_scene_name;

    public GameObject sloadPanel;

    public GameObject BGMSwitch;

    public float start_time = 0;


}

[System.Serializable]
public class AudioSet
{
    public AudioClip introClip;
    public AudioClip loopClip;
}
