using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPInfo : HPInfo
{
    [SerializeField]
    Animator enemy_animator;

    [SerializeField]
    int player_damage = 1;

    public enum EnemyDeathType
    {
        normal,
        doorOpen
    }

    [SerializeField]
    EnemyDeathType type;

    [SerializeField]
    DoorInfo door;

    private GameObject _deathDropPrehab;

   
    public List<DropObjectSet> dropItemObject;

    private int sumWeight = 0;
    private List<GameObject> _selectList = new List<GameObject>();


    private void Start()
    {
        sumWeight = 0;
        _selectList.Clear();
        foreach (DropObjectSet dio in dropItemObject)
        {
            sumWeight += dio.ratio_num;

            for (int i = 0; i < dio.ratio_num; i++)
            {
                _selectList.Add(dio.dropObject);
                
            }
        }
    }



    protected override void DamageMovement(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Fire" || collision.tag == "Scythe Effect")
        {
            hp -= player_damage;
        }
        enemy_animator.SetTrigger("damaged");
    }

    protected override void DeathMovement()
    {
        animator.SetBool("IsDefeat", true);
        animator.SetTrigger("DefeatTrigger");

        switch (type)
        {
            case EnemyDeathType.doorOpen:
                door.OpenDoor();
                break;
        }

        DropItem();
    }

    private void DropItem()
    {
        if (dropItemObject.Count > 0)
        {

            int num = Random.Range(0, sumWeight);
            _deathDropPrehab = _selectList[num]; 
        
            GameObject dropObject = Instantiate(_deathDropPrehab, this.gameObject.transform.position,Quaternion.identity);
            /*dropObject.GetComponent<SpriteRenderer>().sortingLayerName =
                this.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;*/
            if(dropObject.tag == "Item")
            {
                dropObject.GetComponent<ItemObjectInfo>().Bake();
            }
            dropObject.transform.parent = null;
        }
        
    }
}
