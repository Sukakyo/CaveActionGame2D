using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockInfo : MonoBehaviour
{
    [SerializeField]
    DropObjectSet[] dropItemObject;

    [SerializeField]
    Sprite[] sprites; 
    
    /*
    [SerializeField]
    int current_sprites_num = 0;*/

    [SerializeField]
    string LayerName;

    private int sumWeight = 0;
    private List<GameObject> _selectList = new List<GameObject>();


    private void Awake()
    {
        sumWeight = 0;
        _selectList.Clear();
        foreach(DropObjectSet dio in dropItemObject)
        {
            sumWeight += dio.ratio_num;

            for (int i = 0; i < dio.ratio_num; i++)
            {
                _selectList.Add(dio.dropObject);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scythe Effect")
        {
            Animator animator = GetComponent<Animator>();
            AudioSource audio = GetComponent<AudioSource>();
            animator.SetBool("Broken", true);
            audio.Play();
            if (dropItemObject.Length > 0)
            {
                
                int num = Random.Range(0,sumWeight);
                GameObject itemObject = Instantiate(_selectList[num], transform.position, Quaternion.identity);
                itemObject.GetComponent<ItemObjectInfo>().Bake();
            }
        }
    }

    public void ChangeSprite(int num)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[num];
    }

    public void DestroySelf()
    {
        
        Destroy(gameObject);
    }
}

[System.Serializable]

public class DropObjectSet 
{
    [SerializeField]
    public GameObject dropObject;

    [SerializeField]
    public int ratio_num = 1;
}
