using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}
public enum RoomType
{
    START,
    BATTLE,
    EVENT,
    BOSS,
    SHOP,
    TEST
}

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;
    private void Awake()
    {
        instance = this;
        currentMap = GetComponent<Map>();
    }
    #region Interface
    public Map GetMap(int floor, int baNum, int evNum, int shNum)
    {
        currentMap.Floor = floor;
        currentMap.BattleRoomNum = baNum;
        currentMap.EventRoomNum = evNum;
        currentMap.ShopRoomNum = shNum;

        currentRooms = new List<Room>();
        BuildRooms();
        SetRooms();

        for (int i = 0; i < currentRooms.Count; i++)
        {
            currentRooms[i].DestroyDoors();
        }

        currentMap.Rooms = currentRooms;
        currentMap.SetStartRoom(currentRooms[0]);

        currentMap.MaxBorder = GetMaxBorders();
        currentMap.MinBorder = GetMinBorders();

        for(int i=0;i< currentRooms.Count;i++)
        {
            currentMap.SetRoomOff(currentRooms[i]);
        }

        return currentMap;
    }
    public Map GetTestMap(int floor, RoomType rt, string roomName)
    {

        currentMap = GameObject.Find("Map").GetComponent<Map>();
        currentMap.Floor = floor;

        currentRooms = new List<Room>();
        BuildTestRoom();
        SetRooms();

        for (int i = 0; i < currentRooms.Count; i++)
        {
            currentRooms[i].DestroyDoors();
        }

        currentMap.Rooms = currentRooms;
        currentMap.SetStartRoom(currentRooms[0]);

        currentMap.MaxBorder = GetMaxBorders();
        currentMap.MinBorder = GetMinBorders();

        return currentMap;
    }
    #endregion


    Map currentMap;
    int roomNum;
    List<Room> currentRooms;//현재 놓여진 방들
    Queue<Room> roomQueue;//놓아야 하는 방들
    bool testMode;
    public int space = 3;

    private Vector2Int GetMaxBorders()
    {
        int maxX = 0; int maxY = 0;
        foreach (Room r in currentRooms)
        {
            Vector2Int temp = new Vector2Int((int)r.transform.position.x,
                (int)r.transform.position.y) + r.size;
            if (temp.x > maxX)
            {
                maxX = temp.x;
            }
            if (temp.y > maxY)
            {
                maxY = temp.y;
            }
        }
        return new Vector2Int(maxX, maxY);
    }
    private Vector2Int GetMinBorders()
    {
        int minX = 0; int minY = 0;
        foreach (Room r in currentRooms)
        {

            if (r.transform.position.x < minX)
            {
                minX = (int)r.transform.position.x;
            }
            if (r.transform.position.y < minY)
            {
                minY = (int)r.transform.position.y;
            }
        }
        return new Vector2Int(minX, minY);
    }

    private void GetRoomData(int floor)
    {
        TextAsset[] res = Resources.LoadAll("RoomData/Floor" + floor) as TextAsset[];
        for (int i = 0; i < res.Length; i++)
        {
            Debug.Log(res[i].text);
        }
    }

    private void BuildRooms()
    {
        BuildRoom.Init(currentMap);
        roomQueue = new Queue<Room>();

        Room startRoom = BuildRoom.Build(RoomType.START, "start");
        roomQueue.Enqueue(startRoom);

        for (int i = 0; i < currentMap.BattleRoomNum; i++)
        {
            Room battleRoom = BuildRoom.Build(RoomType.BATTLE);
            roomQueue.Enqueue(battleRoom);
        }
        for (int i = 0; i < currentMap.ShopRoomNum; i++)
        {
            Room shopRoom = BuildRoom.Build(RoomType.SHOP);
            roomQueue.Enqueue(shopRoom);
        }
        for (int i = 0; i < currentMap.EventRoomNum; i++)
        {
            Room eventRoom = BuildRoom.Build(RoomType.EVENT);
            roomQueue.Enqueue(eventRoom);
        }

        Room bossRoom = BuildRoom.Build(RoomType.BOSS, "boss");
        roomQueue.Enqueue(bossRoom);

    }
    private void BuildTestRoom()
    {
        BuildRoom.Init(currentMap);
        roomQueue = new Queue<Room>();

        Room startRoom = BuildRoom.Build(RoomType.START, "start");
        roomQueue.Enqueue(startRoom);

        Room testRoom = BuildRoom.Build(Config.instance.TestRoomType, Config.instance.TestRoomName);
        roomQueue.Enqueue(testRoom);
    }

    private void SetRooms()
    {
        Room cur = roomQueue.Dequeue();
        currentRooms.Add(cur);
        while (roomQueue.Count > 0)
        {
            cur = roomQueue.Dequeue();
            while (!ConnectRoom(currentRooms[Random.Range(0, currentRooms.Count)], cur))
            {
            }
            currentRooms.Add(cur);
        }
    }



    private bool ConnectRoom(Room room1, Room room2)
    {
        Door room1Door = null;
        for (int i = 0; i < room1.doorList.Count; i++)
        {
            if (room1.doorList[i].TargetRoom == null)
            {
                room1Door = room1.doorList[Random.Range(0, room1.doorList.Count)];
                break;
            }
            if (i == room1.doorList.Count - 1)
            {
                return false;
            }
        }

        Direction opposite = (Direction)(((int)room1Door.Dir + 2) % 4);
        Door room2Door = null;

        for (int i = 0; i < room2.doorList.Count; i++)
        {
            if (room2.doorList[i].Dir == opposite && room2.doorList[i].TargetRoom == null)
            {
                room2Door = room2.doorList[i];
                break;
            }
            if (i == room2.doorList.Count - 1)
            {
                return false;
            }
        }

        ///방배치 시작
        Vector3 offset = room2.transform.position - room2Door.transform.position;
        switch (room1Door.Dir)
        {
            case Direction.NORTH:
                room2.transform.position =
                new Vector3(room1Door.transform.position.x + offset.x
                , room1.transform.position.y + room1.size.y + space);
                break;
            case Direction.EAST:
                room2.transform.position =
                   new Vector3(room1.transform.position.x + room1.size.x + space
                   , room1Door.transform.position.y + offset.y);
                break;
            case Direction.SOUTH:
                room2.transform.position =
                    new Vector3(room1Door.transform.position.x + offset.x
                    , room1.transform.position.y - room2.size.y - space);
                break;
            case Direction.WEST:
                room2.transform.position =
                     new Vector3(room1.transform.position.x - room2.size.x - space
                     , room1Door.transform.position.y + offset.y);
                break;
        }

        if (IsOverlapped(room2))
            return false;
        else
        {
            room1Door.TargetRoom = room2;
            room1Door.ConnectedDoor = room2Door;
            room2Door.TargetRoom = room1;
            room2Door.ConnectedDoor = room1Door;
            return true;
        }

    }

    /// <summary>
    /// 해당위치에 room을 놓았을때 다른 방들이랑 겹칠까?
    /// </summary>
    private bool IsOverlapped(Room room)
    {
        foreach (Room r in currentRooms)
        {
            float compareX, compareY, disX, disY;
            disX = r.transform.position.x - room.transform.position.x;
            disY = r.transform.position.y - room.transform.position.y;
            if (disX > 0)
            {
                compareX = room.size.x;
            }
            else
            {
                compareX = r.size.x;
            }

            if (disY > 0)
            {
                compareY = room.size.y;
            }
            else
            {
                compareY = r.size.y;
            }

            if (Mathf.Abs(disX) < compareX + 1 && Mathf.Abs(disY) < compareY + 1)
            {
                return true;
            }
        }
        return false;
    }


}
