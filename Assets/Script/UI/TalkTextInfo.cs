using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.IO;
using Cainos.PixelArtTopDown_Basic;

public class TalkTextInfo : MonoBehaviour
{
    [SerializeField]
    private GameManagement gameManagement;
    [SerializeField]
    private Animator player_animator;

    [SerializeField]
    private Image[] characterImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private RubyTextMeshProUGUI talkText;

    [SerializeField]
    private GameObject imageObject;


    private List<string[]> text_data = new List<string[]>();
    private int text_data_num = 1;

    private bool text_set = false;
    private char[] text_char;

    private float now_time = 0;
    [SerializeField]
    private float talkSpeed = 0.2f;

    private int char_num = 0;
    [SerializeField]
    private int max_stay_num = 10;
    private int stay_num = 0;

    private float after_time = 0;
    [SerializeField]
    private float max_after_time = 2f;

    private bool text_finish = false;


    private AudioSource text_audio;

    [HideInInspector]
    public bool talk_finish = false;


    private string text = "";


    [SerializeField]
    private Sprite[] images;
    [SerializeField]
    private Image ui_image;


    private bool textLoading = false;

    private void Start()
    {
        text_audio = GetComponent<AudioSource>();
    }


    public void OnFire(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            OnFire();
               
        }
    }

    public void OnFire()
    {
        if (textLoading)
        {
            
                if (text_finish)
                {
                    Debug.Log(text_data_num);
                    Debug.Log(text_data.Count);
                    text_data_num++;
                    if (text_data_num < text_data.Count)
                    {
                        ChangeText();
                    }
                    else
                    {
                        FinishTalk();
                    }
                }
                else
                {
                    FinishText();
                }

            

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagement.state == GameManagement.GameState.talk)
        {
            if (!text_set)
            {
                StartText();
                nameText.enabled = true;
                talkText.enabled = true;

                textLoading = true;
                text_set = true;
            }
            else
            {
                if (char_num < text_char.Length)
                {
                    if (now_time > talkSpeed)
                    {

                        if (stay_num == 0)
                        {
                            if (text_char[char_num] == 'ÅB')
                            {
                                stay_num = max_stay_num;
                            }

                            if (text_char[char_num] == '<' && text_char[char_num+1] == 'r' )
                            {
                                while (text_char[char_num] != 'r' || text_char[char_num+1] != '>' )
                                {
                                    text += text_char[char_num];
                                    talkText.uneditedText = text;
                                    char_num++;
                                }

                                text += text_char[char_num];
                                talkText.uneditedText = text;
                                char_num++;
                            }

                            //talkTextRuby.Text += text_char[char_num];
                            text += text_char[char_num];
                            talkText.uneditedText = text;
                            
                            char_num++;
                            now_time = 0;
                        }
                        else
                        {
                            stay_num--;
                            now_time = 0;
                        }
                    }
                    else
                    {
                        now_time += Time.deltaTime;
                    }
                }
                else
                {
                    if (!text_finish)
                    {
                        if (after_time > max_after_time)
                        {
                            FinishText();
                            after_time = 0;
                        }
                        else
                        {
                            after_time += Time.deltaTime;
                        }
                    }
                }
            }
        }
    }

    private void ResetText()
    {
        Debug.Log("deleat");
        text = "";
        talkText.uneditedText = "";

    }
    private void StartText()
    {
        //2óÒñ⁄ÇÃï∂éöÇéÊÇËàµÇ§
        ResetText();
        text_audio.Play();

        Debug.Log(text_data[text_data_num][1]);
        if (text_data[text_data_num][1].Equals("NULL"))
        {
            nameText.text = "";
        }
        else if (!(text_data[text_data_num][1].Equals("_"))) 
        {
            Debug.Log(text_data[text_data_num][1]);
            nameText.text = text_data[text_data_num][1];
        }

        if (text_data[text_data_num].Length > 3)
        {
            if (!text_data[text_data_num][3].Equals(""))
            {
                ChangeImage(int.Parse(text_data[text_data_num][3]));
            }
        }

        Debug.Log(text_data[text_data_num][2]);
        text_char = text_data[text_data_num][2].ToCharArray();
        
    }
    
    private void ChangeText()
    {
        char_num = 0;
        now_time = 0;
        text_finish = false;
        imageObject.GetComponent<Animator>().SetBool("PlayImage", false);
        imageObject.GetComponent<Image>().enabled = false;
        
        StartText();
    }
    private void FinishText()
    {
        imageObject.GetComponent<Image>().enabled = true;
        imageObject.GetComponent<Animator>().SetBool("PlayImage", true);
        text_finish = true;
        text = text_data[text_data_num][2];
        talkText.uneditedText = text_data[text_data_num][2];
        char_num = text_char.Length;
    }

    public void StartTalk()
    {
        talk_finish = false;

        text_set = false;

        TextAsset textAsset;


        textAsset = gameManagement.csv_text;

        StringReader sr = new StringReader(textAsset.text);
        while (sr.Peek() > -1)
        {
            string line = sr.ReadLine();
            line = line.Replace("\\n", "\n");
            line = line.Replace("\\t", "\t");
            Debug.Log(line);
            string[] line_splits = line.Split(new char[] { '\t' });

            text_data.Add(line_splits);
        }
        

        

    }

    private void FinishTalk()
    {
        char_num = 0;
        now_time = 0;
        text_finish = false;
        text_data = new List<string[]>();
        text_data_num = 1;
        nameText.enabled = false;
        talkText.enabled = false;
        gameManagement.gameObject.GetComponent<Animator>().SetBool("IsTalkTime", false);
        if (player_animator != null)
        {
            player_animator.SetBool("IsGetTool", false);
        }

        talk_finish = true;

        textLoading = false;
    }



    public void ChangeImage(int num)
    {
        ui_image.sprite = images[num];
    }

    public void SetImages(Sprite[] sprites)
    {
        this.images = sprites;
    }
}
