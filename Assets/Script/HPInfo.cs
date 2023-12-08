using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class HPInfo : MonoBehaviour
{

    public int max_hp;
    [SerializeField]
    protected int hp;
    protected int preHp;
    public int Hp {get { return hp;} set { hp = value; } }

    [SerializeField]
    protected Animator animator;

    private Vector2 _dir;
    public Vector2 Dir { get { return _dir; } }

    [SerializeField]
    int knock_back_power;

    bool deathOn = false;

    [SerializeField]
    GameObject character;

    [SerializeField]
    AudioSource audioSource;

    string parentTag;

    private bool fireOn = false;

    public bool damage_switch = true;



    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (this.enabled == false) return;


        if (!fireOn && damage_switch)
        {
            Debug.Log("touch");

            if (gameObject.tag == "Player" || gameObject.tag == "Boss" || gameObject.tag == "Enemy")
            {
                animator.SetTrigger("IsDamaged");
                _dir = (this.transform.position - collision.transform.position).normalized;
                _dir = _dir * knock_back_power;
                GetComponent<AudioSource>().Play();
            }

            DamageMovement(collision);
            fireOn = true;
        }

    }

    protected abstract void DamageMovement(Collider2D collision);

    private void Update()
    {
        // false Çè¡Ç∑Ç±Ç∆(äÆóπ)
        if (hp <= 0 && !deathOn /*&& false*/)
        {

            PlayAudio();
            character.GetComponent<TopDownCharacterController>().ToDeath();

            DeathMovement();

            deathOn = true;
        }
    }

    private void LateUpdate()
    {
        fireOn = false;
    }

    protected abstract void DeathMovement();

    public void PlayAudio()
    {
        audioSource.Play();
    }


    public int GetHP()
    {
        return hp;
    }


}
