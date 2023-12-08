using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyInfo : MonoBehaviour
{
    [SerializeField]
    GameObject prehab;

    float time = 0;


    

    private void Update()
    {
        if (time > 3f)
        {
            time = 0;
            
            GameObject gb = Instantiate(prehab, transform.position + new Vector3(0, -1, 0),Quaternion.identity);
            gb.GetComponent<FireBallInfo>().startDirection = new Vector3(0, -1, 0);
            gb.transform.localScale = gb.transform.localScale * 2;
            

            gb = Instantiate(prehab, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
            gb.GetComponent<FireBallInfo>().startDirection = new Vector3(-1, 0, 0);
            gb.transform.localScale = gb.transform.localScale * 2;
            

            gb = Instantiate(prehab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            gb.GetComponent<FireBallInfo>().startDirection = new Vector3(0, 1, 0);
            gb.transform.localScale = gb.transform.localScale * 2;
            

            gb = Instantiate(prehab, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
            gb.GetComponent<FireBallInfo>().startDirection = new Vector3(1, 0, 0);
            gb.transform.localScale = gb.transform.localScale * 2;
            


        }
        else
        {
            time += Time.deltaTime;
        }
    }
}
