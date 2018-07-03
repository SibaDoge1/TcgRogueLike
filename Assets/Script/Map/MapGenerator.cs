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

public static class MapGenerator 
{
	#region Interface
	public static Map GetNewMap(int level,int _roomNum)
    {
		//TODO : Generate Map With Seed
		roomNum = _roomNum;
        currentRooms = new List<Room>();

        newMap = GameObject.Find("Map").GetComponent<Map>();

        SetSeeds();
        SetRooms(); 

        newMap.Room = currentRooms;
        newMap.SetStartRoom(currentRooms[0]);

        for(int i=0; i<currentRooms.Count;i++)
        {
            currentRooms[i].OpenDoors();
        }
        return newMap;
    }
    #endregion

    static Map newMap;
    static int roomNum;
    static List<Room> currentRooms;//현재 놓여진 방들
    static Queue<Room> roomQueue;//놓아야 하는 방들

    static Vector2Int mapSize;

    private static Vector2Int GetMaxBorder()
    {
        int maxX = mapSize.x/2; int maxY = mapSize.y/2;
       foreach(Room r in currentRooms)
        {
            if(r.transform.position.x>maxX)
            {
                maxX = (int)r.transform.position.x;
            }
            if(r.transform.position.y>maxY)
            {
                maxY = (int)r.transform.position.y;
            }          
        }
        return new Vector2Int(maxX, maxY);
    }
    private static Vector2Int GetMinBorder()
    {
        int minX = mapSize.x / 2; int minY = mapSize.y / 2;
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

    private static void SetSeeds()
    {
        roomQueue = new Queue<Room>();

        Room startRoom = GetRoom();
        new StartRoom(startRoom);
        roomQueue.Enqueue(startRoom);
        startRoom.name = "Start Room";

        for (int i = 2; i < roomNum; i++)
        {
            Room battleRoom = GetRoom();
            new BattleRoom(battleRoom, EnemyDatabase.pool1);
            roomQueue.Enqueue(battleRoom);
            battleRoom.name = "Battle Room"+i;
        }
        Room bossRoom = GetRoom();
        new BossRoom(bossRoom, EnemyDatabase.bossPool);
        roomQueue.Enqueue(bossRoom);
        bossRoom.name = "Boss Room";
    }

    private static void SetRooms()
    {
        //TODO : 수정
        Room cur = roomQueue.Dequeue();
        cur.SetStartRoom();
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
    private static Room GetRoom()
    {
        Room room = InstantiateDelegate.Instantiate(Resources.Load("Room/default") as GameObject,newMap.transform).GetComponent<Room>();
        return room;
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
                    new Vector3(room1Door.transform.position.x+offset.x
                    ,room1.transform.position.y+room1.size.y / 2 + room2.size.y / 2 + 2);
                break;
            case Direction.EAST:
                room2.transform.position = 
                   new Vector3(room1.transform.position.x + room1.size.x/2+room2.size.x/2 + 2
                   , room1Door.transform.position.y+offset.y);
                break;
            case Direction.SOUTH:
                room2.transform.position = 
                    new Vector3(room1Door.transform.position.x + offset.x
                    , room1.transform.position.y - room1.size.y / 2 - room2.size.y / 2 - 2);
                break;
            case Direction.WEST:
                room2.transform.position = 
                     new Vector3(room1.transform.position.x - room1.size.x / 2 - room2.size.x / 2 - 2
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
            if(Mathf.Abs(r.transform.position.x-room.transform.position.x)<=r.size.x/2+room.size.x/2 +1 
                && Mathf.Abs(r.transform.position.y-room.transform.position.y)<=r.size.y/2+room.size.y/2+1)
            {
                return true;
            }
        }
        return false;
    }


}
