using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class RoomManagement : MonoBehaviour
{
    [SerializeField]
    List<RoomInfo> roomInfos = new List<RoomInfo>();


    [SerializeField]
    TextAsset trans_csv;

    [SerializeField]
    TextAsset invisible_csv;

    [SerializeField]
    TextAsset generate_csv;

    List<string[]> _transData = new List<string[]>();

    [SerializeField]
    RoomInfo current_roomInfo;

    RoomInfo _preRoomInfo;


    private void Start()
    {
        SetRoomOnOff();
    }

    public void Bake()
    {
        foreach (RoomInfo roomInfo in roomInfos)
        {
            roomInfo.nextRooms.Clear();
        }
        foreach (RoomInfo roomInfo in roomInfos)
        {
            roomInfo.upRooms.Clear();
        }
        foreach (RoomInfo roomInfo in roomInfos)
        {
            roomInfo.generateRooms.Clear();
        }


        CreateRoomGraph(trans_csv,_transData);

        for (int i = 1; i < _transData.Count; i++)
        {
            string first = _transData[i][0];
            string second = _transData[i][1];

            RoomInfo firstRoom = roomInfos.Find(x => x.gameObject.name.Equals(first));
            RoomInfo secondRoom = roomInfos.Find(x => x.gameObject.name.Equals(second));
            Debug.Log(firstRoom);
            Debug.Log(secondRoom);
            Debug.Log(_transData.Count);
            firstRoom.nextRooms.Add(secondRoom);
        }


        CreateRoomGraph(invisible_csv, _transData);

        for (int i = 1; i < _transData.Count; i++)
        {
            string first = _transData[i][0];
            string second = _transData[i][1];

            RoomInfo firstRoom = roomInfos.Find(x => x.gameObject.name.Equals(first));
            RoomInfo secondRoom = roomInfos.Find(x => x.gameObject.name.Equals(second));
            Debug.Log(firstRoom);
            Debug.Log(secondRoom);
            Debug.Log(_transData.Count);
            firstRoom.upRooms.Add(secondRoom);
        }

        CreateRoomGraph(generate_csv, _transData);

        for (int i = 1; i < _transData.Count; i++)
        {
            string first = _transData[i][0];
            string second = _transData[i][1];

            RoomInfo firstRoom = roomInfos.Find(x => x.gameObject.name.Equals(first));
            RoomInfo secondRoom = roomInfos.Find(x => x.gameObject.name.Equals(second));
            Debug.Log(firstRoom);
            Debug.Log(secondRoom);
            Debug.Log(_transData.Count);
            firstRoom.generateRooms.Add(secondRoom);
        }


        Debug.Log(roomInfos[0].gameObject.name);
        current_roomInfo = roomInfos[0];
    }

    private void CreateRoomGraph(TextAsset text_csv, List<string[]> data)
    {
        data.Clear();

        StringReader sr = new StringReader(text_csv.text);
        while (sr.Peek() > -1)
        {
            string line = sr.ReadLine();
            line = line.Replace("\\n", "\n");
            //Debug.Log(line);
            string[] line_splits = line.Split(new char[] { ',' });

            data.Add(line_splits);
        }
    }


    // RoomChangeColliderÇ≈égóp
    public void ChangeRoom(string room_name)
    {
        _preRoomInfo = current_roomInfo;
        
        if (current_roomInfo.name.Equals(room_name))
        {
            return;
        }
        else
        {
            foreach (RoomInfo roomInfo in current_roomInfo.nextRooms)
            {
                Debug.Log(roomInfo.gameObject.name);
                Debug.Log(room_name);
            }
            
            current_roomInfo = current_roomInfo.nextRooms.Find(x => x.name.Equals(room_name));
            Debug.Log(current_roomInfo.name);
            SetRoomOnOff();
        }

        
    }

    private void SetRoomOnOff()
    {

        IEnumerable<RoomInfo> deleteRooms = roomInfos.Except<RoomInfo>(current_roomInfo.generateRooms.Concat(new List<RoomInfo>() { current_roomInfo }));
        
        // ç≈èâÇ…generate
        foreach (RoomInfo roomInfo in current_roomInfo.generateRooms)
        {
            if(!roomInfo.Equals(_preRoomInfo))
            roomInfo.GenerateObject();
        }
        foreach (RoomInfo roomInfo in deleteRooms)
        {
            roomInfo.DeleteObject();
        }

        foreach (RoomInfo roomInfo in roomInfos)
        {
            roomInfo.RoomOff();
        }

        current_roomInfo.RoomOn();
        foreach (RoomInfo roomInfo in current_roomInfo.nextRooms)
        {
            roomInfo.RoomOn();
        }
        foreach (RoomInfo roomInfo in current_roomInfo.upRooms)
        {
            roomInfo.RoomInvisible();
        }

        
    }
}

