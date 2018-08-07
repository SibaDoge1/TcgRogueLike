using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

public static class MapGenerator 
{
	#region Interface
	public static Map GetNewMap(int floor,int _roomNum)
    {
		roomNum = _roomNum;
        currentRooms = new List<Room>();

        newMap = GameObject.Find("Map").GetComponent<Map>();
        newMap.floor = floor;

        BuildRooms();
        SetRooms();

        for (int i = 0; i < currentRooms.Count; i++)
        {
            currentRooms[i].DeleteDoors();
        }

        newMap.Rooms = currentRooms;
        newMap.SetStartRoom(currentRooms[0]);

        newMap.maxBorder = GetMaxBorders();
        newMap.minBorder = GetMinBorders();

        currentRooms[0].SetStartRoom();

        return newMap;
    }
    #endregion


    static Map newMap;
    static int roomNum;
    static List<Room> currentRooms;//현재 놓여진 방들
    static Queue<Room> roomQueue;//놓아야 하는 방들
    public static int space = 3;

    private static Vector2Int GetMaxBorders()
    {
        int maxX = 0; int maxY = 0;
       foreach(Room r in currentRooms)
        {
            Vector2Int temp = new Vector2Int((int)r.transform.position.x,
                (int)r.transform.position.y) + r.size;
            if(temp.x>maxX)
            {
                maxX = temp.x;
            }
            if(temp.y>maxY)
            {
                maxY = temp.y;
            }          
        }
        return new Vector2Int(maxX, maxY);
    }
    private static Vector2Int GetMinBorders()
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

    private static void GetRoomData(int floor)
    {
      TextAsset[] res =  Resources.LoadAll("RoomData/Floor" + floor) as TextAsset[];
      for(int i=0; i<res.Length; i++)
        {
            Debug.Log(res[i].text);
        }
    }

    private static void BuildRooms()
    {
        BuildRoom.Init(newMap);
        roomQueue = new Queue<Room>();
        Room startRoom = BuildRoom.Build(RoomType.START, "start");
        roomQueue.Enqueue(startRoom);

        if (Config.instance.RoomTestMode)
        {
            Room testRoom = BuildRoom.Build(Config.instance.TestRoomType,Config.instance.TestRoomName);
            roomQueue.Enqueue(testRoom);
        }else
        {
            for (int i = 2; i < roomNum; i++)
            {
                Room battleRoom = BuildRoom.Build(RoomType.BATTLE);
                roomQueue.Enqueue(battleRoom);
            }

            Room bossRoom = BuildRoom.Build(RoomType.BOSS);
            roomQueue.Enqueue(bossRoom);
        }        
    }

    private static void SetRooms()
    {
        Room cur = roomQueue.Dequeue();
        currentRooms.Add(cur);
        while(roomQueue.Count > 0)
        {
            cur = roomQueue.Dequeue();
            while(!ConnectRoom(currentRooms[Random.Range(0, currentRooms.Count)], cur))
            {
            }
            currentRooms.Add(cur);
        }
    }



    private static bool ConnectRoom(Room room1,Room room2)
    {
        Door room1Door = null;
        for (int i=0; i<room1.doorList.Count;i++)
        {
            if (room1.doorList[i].TargetRoom == null)
            {
                room1Door = room1.doorList[Random.Range(0, room1.doorList.Count)];               
                break;
            }
            if(i == room1.doorList.Count-1)
            {
                return false;
            }
        }

        Direction opposite = (Direction)(((int)room1Door.Dir + 2) % 4);
        Door room2Door = null;

        for (int i=0; i<room2.doorList.Count;i++)
        {
            if(room2.doorList[i].Dir == opposite && room2.doorList[i].TargetRoom == null)
            {
                room2Door = room2.doorList[i];
                break;
            }
            if(i == room2.doorList.Count-1)
            {
                return false;
            }
        }

        ///방배치 시작
        Vector3 offset = room2.transform.position- room2Door.transform.position;
        switch (room1Door.Dir)
        {
            case Direction.NORTH:
                    room2.transform.position =
                    new Vector3(room1Door.transform.position.x + offset.x
                    ,room1.transform.position.y + room1.size.y +space);
                break;
            case Direction.EAST:             
                room2.transform.position = 
                   new Vector3(room1.transform.position.x + room1.size.x + space
                   , room1Door.transform.position.y+offset.y);
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
    private static bool IsOverlapped(Room room)
    {
        foreach(Room r in currentRooms)
        {
            float compareX, compareY,disX,disY;
            disX = r.transform.position.x - room.transform.position.x;
            disY = r.transform.position.y - room.transform.position.y;
            if (disX >0)
            {
                compareX = room.size.x;
            }else
            {
                compareX = r.size.x;
            }

            if(disY>0)
            {
                compareY = room.size.y;
            }else
            {
                compareY = r.size.y;
            }

            if(Mathf.Abs(disX)<compareX+1 && Mathf.Abs(disY)<compareY+1)
            {
                return true;
            }
        }
        return false;
    }


}
