using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHPInfo : HPInfo
{
    [SerializeField]
    HeartInfo heartInfo;

    [SerializeField]
    int enemy_damage = 1;

    [SerializeField]
    GameManagement gameManagement;

    public void Start()
    {
        //hp = max_hp;
        if (gameManagement.gameMode == GameManagement.GameMode.play_game && this.transform.parent.tag == "Player")
        {
            UpdateHP();
        }
    }

    protected override void DamageMovement(Collider2D collision)
    {
        if (((collision.tag == "Enemy" || collision.tag == "Boss" || collision.tag == "Fire") &&
             (this.transform.parent.tag == "Player" || this.transform.parent.tag == "Decoy")) ||
            ((collision.tag == "Player" || collision.tag == "Fire" || collision.tag == "Scythe Effect") &&
             (this.transform.parent.tag == "Enemy" || this.transform.parent.tag == "Boss")))
        {
            
            hp -= enemy_damage;
        }
        if (gameManagement.gameMode == GameManagement.GameMode.play_game && this.transform.parent.tag == "Player")
        {
            UpdateHP();
        }
    }

    protected override void DeathMovement()
    {
        
        animator.SetBool("IsGameOver",true);
        animator.SetTrigger("GameOverTrigger");
        
    }

    public void UpdateHP()
    {
        heartInfo.CreateHeart(hp);
    }

    

    

}
