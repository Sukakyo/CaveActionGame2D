using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartInfo : MonoBehaviour
{
    private List<GameObject> _gameObjects = new List<GameObject>();
    [SerializeField]
    private GameObject gbPrehab;
    [SerializeField]
    private GameObject brokenPrehab;

    [SerializeField]
    private HPInfo hpInfo;

    public void CreateHeart(int num)
    {
        foreach (GameObject gb in _gameObjects)
        {
            Destroy(gb);
        }
        _gameObjects.Clear();
        for(int i = 0; i < num; i++)
        {
            _gameObjects.Add(Instantiate(gbPrehab,this.transform));
        }
        for(int i = 0; i < hpInfo.max_hp - num; i++)
        {
            _gameObjects.Add(Instantiate(brokenPrehab, this.transform));
        }
        Debug.Log(true);
    }
}
