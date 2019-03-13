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
}

public class MapGenerator : MonoBehaviour
{
    #region Interface
    public Map GetMap(int floor, int baNum, int evNum,bool boss,bool end)
    {
        currentMap = GetComponent<Map>();

        currentMap.Init(floor, baNum, evNum,boss,end);

        BuildRooms();
        SetRooms();

        for (int i = 0; i < connectedRooms.Count; i++)
        {
            connectedRooms[i].DestroyDoors();
        }

        currentMap.Rooms = connectedRooms;
        currentMap.SetStartRoom(startRoom);

        currentMap.MaxBorder = GetMaxBorders();
        currentMap.MinBorder = GetMinBorders();

        for(int i=0;i< connectedRooms.Count;i++)
        {
            currentMap.SetRoomOff(connectedRooms[i]);
        }

        return currentMap;
    }

    private Vector2Int GetMaxBorders()
    {
        int maxX = 0; int maxY = 0;
        foreach (Room r in connectedRooms)
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
        foreach (Room r in connectedRooms)
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
    #endregion


    Map currentMap;
    Room startRoom;
    List<Room> connectedRooms;//현재 놓여진 방들
    List<Room> roomsToSet;//놓아야 하는 방들
    public int space;



    private void BuildRooms()
    {

        BuildRoom.Init(currentMap);
        roomsToSet = new List<Room>();

        startRoom = BuildRoom.Build(RoomType.START, "start"+currentMap.Floor);
        roomsToSet.Add(startRoom);

        if (Config.instance.RoomTestMode) // Test모드
        {
            Room testRoom = BuildRoom.Build(Config.instance.TestRoomType, Config.instance.TestRoomName);
            roomsToSet.Add(testRoom);           
        }else //아닐시 Map정보에 따라 빌드
        {
                    for (int i = 0; i < currentMap.BattleRoomNum; i++)
                    {
                        Room battleRoom = BuildRoom.Build(RoomType.BATTLE);
                        roomsToSet.Add(battleRoom);
                    }
                    for (int i = 0; i < currentMap.EventRoomNum; i++)
                    {
                        Room eventRoom = BuildRoom.Build(RoomType.EVENT);
                        roomsToSet.Add(eventRoom);
                    }
                    if (currentMap.Boss)
                    { 
                        Room bossRoom = BuildRoom.Build(RoomType.BOSS, "boss"+currentMap.Floor);
                        roomsToSet.Add(bossRoom);
                    }
                    if(currentMap.End)
                    {
                        Room endRoom = BuildRoom.Build(RoomType.START, "end"+currentMap.Floor);
                        roomsToSet.Add(endRoom);
                    }                              
        }     
    }


    #region SetRooms

    /// <summary>
    /// 룸 세팅
    /// </summary>
    private void SetRooms()
    {
        connectedRooms = new List<Room>();
        Queue<Room> roomQueue = new Queue<Room>(roomsToSet);
        Room cur = roomQueue.Dequeue();
        connectedRooms.Add(cur);//StartRoom

        if (Config.instance.RoomTestMode)
        {
            Room test = roomQueue.Dequeue();
            ConnectRoom(cur, test);
            connectedRooms.Add(test);
        }else
        {
                    while (roomQueue.Count > 0)
                    {
                        cur = roomQueue.Dequeue();

                            int loopNum = 0;
                            while (!ConnectRoom(connectedRooms[loopNum], cur))
                            {
                             loopNum++;
                             if (loopNum >= connectedRooms.Count)
                                {
                                  Debug.Log("Rooms are not fit well So restart");
                                  DestroyRooms();
                                  BuildRooms();
                                  SetRooms();
                                 return;
                                 }
                             }
                            connectedRooms.Add(cur);
                            ShuffleRoomList();                  
                    }
        }
    }
    


    #endregion
    private void DestroyRooms()
    {
        for(int i=roomsToSet.Count-1; i>=0; i--)
        {
            Destroy(roomsToSet[i].gameObject);
        }
    }

    

    private bool ConnectRoom(Room room1, Room room2)
    {
        ShuffleDoor(room1);
        ShuffleDoor(room2);

        OffTile_Door room1Door = null;
        OffTile_Door room2Door = null;
        bool success = false;
        
        if (room2.RoomName.Contains("end") && room1.Distance < 3) //끝방 조건(시작방으로부터 일정거리 떨어져야 만들어짐!!!)
        {
            return false;
        }

        for (int i = 0; i < room1.doorList.Count; i++)
        {
            if (room1.doorList[i].TargetRoom == null)
            {
                room1Door = room1.doorList[i];
                room2Door = FindRightDoor(room1Door, room2);
                if (room2Door == null)
                    continue;
                else
                {
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
                    {
                        room2.transform.position = new Vector3(-9999, -9999);
                        continue;
                    }else
                    {
                        room1Door.TargetRoom = room2;
                        room1Door.ConnectedDoor = room2Door;
                        room2Door.TargetRoom = room1;
                        room2Door.ConnectedDoor = room1Door;
                        room2.Distance = room1.Distance + 1;
                        success = true;
                        break;
                    }
                }
            }
        }

        return success;        
    }

    private OffTile_Door FindRightDoor(OffTile_Door room1Door, Room room2)
    {
        Direction opposite = (Direction)(((int)room1Door.Dir + 2) % 4);
        for (int j = 0; j < room2.doorList.Count; j++)
        {
            if (room2.doorList[j].Dir == opposite && room2.doorList[j].TargetRoom == null)
            {
                return room2.doorList[j];
            }
        }
        return null;
    }
    private void ShuffleDoor(Room room)
    {
        for(int i=0; i<room.doorList.Count;i++)
        {
            OffTile_Door var = room.doorList[i];
            int ranNum = Random.Range(i, room.doorList.Count);
            room.doorList[i] = room.doorList[ranNum];
            room.doorList[ranNum] = var;
        }
    }
    private void ShuffleRoomList()
    {
        for(int i=0; i<connectedRooms.Count;i++)
        {
            Room var = connectedRooms[i];
            int ranNum = Random.Range(i, connectedRooms.Count);
            connectedRooms[i] = connectedRooms[ranNum];
            connectedRooms[ranNum] = var;
        }
    }
    /// <summary>
    /// 해당위치에 room을 놓았을때 다른 방들이랑 겹칠까?
    /// </summary>
    private bool IsOverlapped(Room room)
    {
        foreach (Room r in connectedRooms)
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
