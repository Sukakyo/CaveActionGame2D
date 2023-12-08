using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderGeneratorInfo : MonoBehaviour
{
    [SerializeField]
    GameObject thunderPrehab;

    [SerializeField]
    Vector2[] pos_list;
    int pos_list_num = 0;

    public void GenerateFromList()
    {
        Generate(pos_list[pos_list_num]);
        pos_list_num++;
    }

    public void Generate(Vector2 pos)
    {
        Vector2 tmp_pos = this.transform.position;
        Vector2 angle_vec = (pos - tmp_pos).normalized;
        float angle = Vector2.SignedAngle(angle_vec, new Vector2(0, -1));

        GameObject gb = Instantiate(thunderPrehab,this.transform.position,Quaternion.Euler(0,0,angle));
        gb.GetComponent<ThunderInfo>().desternation = pos;
    }
}
