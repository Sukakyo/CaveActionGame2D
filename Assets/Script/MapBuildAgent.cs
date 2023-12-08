using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuildAgent : MonoBehaviour
{
    [SerializeField]
    RoomManagement roomManagement;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("RoomChangeTrigger"))
        {
            Debug.Log(collision.gameObject);
            roomManagement.ChangeRoom(collision.gameObject.GetComponent<MapBuildTrigger>().changeRoomName);
        }
    }
}
