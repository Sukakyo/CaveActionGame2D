using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyHPInfo : HPInfo
{
    int enemy_damage;


    protected override void DamageMovement(Collider2D collision)
    {
        if (((collision.tag == "Enemy" || collision.tag == "Boss" || collision.tag == "Fire") &&
             (this.transform.parent.tag == "Player" || this.transform.parent.tag == "Decoy")) ||
            ((collision.tag == "Player" || collision.tag == "Fire") &&
             (this.transform.parent.tag == "Enemy" || this.transform.parent.tag == "Boss")))
        {
            enemy_damage = 1;
            hp -= enemy_damage;
        }

        int x = Random.Range(-67, -44);
        int y = Random.Range(16, 31);

        this.transform.parent.position = new Vector3(x, y, 0);
    }

    protected override void DeathMovement()
    {
        
    }
}
