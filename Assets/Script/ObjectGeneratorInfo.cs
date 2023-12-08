using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneratorInfo : MonoBehaviour
{
    public enum GenerateType
    {
        enemy,
        rock
    }

    public GenerateType generateType;

    [SerializeField]
    GameObject prehab;

    public GameObject gb;

    [SerializeField]
    private string layer;
    [SerializeField]
    private string sortingLayer;
    [SerializeField]
    private int sortingOrder;

    

    
    public List<DropObjectSet> dropObjectSet;

    

    private void Start()
    {
        //Generate();
    }

    public void Generate()
    {
        if(gb == null)
        {
            gb = Instantiate(prehab,transform.position,Quaternion.identity);
            gb.layer = LayerMask.NameToLayer(layer);
            
            SpriteRenderer spriteRenderer = gb.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = sortingLayer;
            spriteRenderer.sortingOrder = sortingOrder;
            gb.GetComponent<TopDownCharacterController>().Bake();
            gb.transform.Find("EnemyAI").GetComponent<EnemyAIInfo>().Bake();
            EnemyHPInfo hp = gb.transform.Find("HP").GetComponent<EnemyHPInfo>();

            foreach (DropObjectSet gb in dropObjectSet)
            {
                hp.dropItemObject.Add(gb);
            }
        }
    }
}
