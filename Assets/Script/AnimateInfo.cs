using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateInfo : MonoBehaviour
{
    [SerializeField]
    int spriteNum = 0;


    // キャラクターの歩行などの画像を指定
    public int SpriteNum
    {
        get { return spriteNum; }
    }

    

}
