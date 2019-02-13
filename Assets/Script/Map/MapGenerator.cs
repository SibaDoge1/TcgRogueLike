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
    SHOP
}

public class MapGenerator : MonoBehaviour
{
    #region Interface
    public Map GetMap(int floor, int baNum, int evNum, int shNum)
    {
        currentMap = GetComponent<Map>();

        currentMap.Floor = floor;
        currentMap.BattleRoomNum = baNum;
        currentMap.EventRoomNum = evNum;
        currentMap.ShopRoomNum = shNum;

        BuildRooms();
        SetRooms();

        for (int i = 0; i < currentRooms.Count; i++)
        {
            currentRooms[i].DestroyDoors();
        }

        currentMap.Rooms = currentRooms;
        currentMap.SetStartRoom(startRoom);

        currentMap.MaxBorder = GetMaxBorders();
        currentMap.MinBorder = GetMinBorders();

        for(int i=0;i< currentRooms.Count;i++)
        {
            currentMap.SetRoomOff(currentRooms[i]);
        }

        return currentMap;
    }

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
    #endregion


    Map currentMap;
    Room startRoom;
    List<Room> currentRooms;//현재 놓여진 방들
    List<Room> roomsToSet;//놓아야 하는 방들
    public int space = 3;



    private void BuildRooms()
    {

        if(Config.instance.RoomTestMode) // Test모드
        {
            BuildRoom.Init(currentMap);
            roomsToSet = new List<Room>();

            startRoom = BuildRoom.Build(RoomType.START, "start");
            roomsToSet.Add(startRoom);

            Room testRoom = BuildRoom.Build(Config.instance.TestRoomType, Config.instance.TestRoomName);
            roomsToSet.Add(testRoom);

            return;
        }else //아닐시 , floor마다 구분
        {
            switch (currentMap.Floor)
            {
                case 1:

                    break;
                case 2:
                case 3:

                    break;
                case 4:
                    BuildRoom.Init(currentMap);
                    roomsToSet = new List<Room>();

                    startRoom = BuildRoom.Build(RoomType.START, "start");
                    roomsToSet.Add(startRoom);

                    for (int i = 0; i < currentMap.BattleRoomNum; i++)
                    {
                        Room battleRoom = BuildRoom.Build(RoomType.BATTLE);
                        roomsToSet.Add(battleRoom);
                    }
                    for (int i = 0; i < currentMap.ShopRoomNum; i++)
                    {
                        Room shopRoom = BuildRoom.Build(RoomType.SHOP);
                        roomsToSet.Add(shopRoom);
                    }
                    for (int i = 0; i < currentMap.EventRoomNum; i++)
                    {
                        Room eventRoom = BuildRoom.Build(RoomType.EVENT);
                        roomsToSet.Add(eventRoom);
                    }

                    Room bossRoom = BuildRoom.Build(RoomType.BOSS, "boss");
                    roomsToSet.Add(bossRoom);
                    break;
                case 5:

                    break;
            }

            
        }     
    }


    #region SetRooms

    /// <summary>
    /// 룸 세팅
    /// </summary>
    private void SetRooms()
    {
        if(Config.instance.RoomTestMode)
        {
            currentRooms = new List<Room>();
            Queue<Room> roomQueue = new Queue<Room>(roomsToSet);
            Room start = roomQueue.Dequeue();
            currentRooms.Add(start);//StartRoom
            Room test = roomQueue.Dequeue();
            ConnectRoom(start, test);
            currentRooms.Add(test);
        }else
        {
                    currentRooms = new List<Room>();
                    Queue<Room> roomQueue = new Queue<Room>(roomsToSet);
                    Room cur = roomQueue.Dequeue();
                    currentRooms.Add(cur);

                    while (roomQueue.Count > 0)
                    {
                        cur = roomQueue.Dequeue();
                        int loopNum = 0;
                        while (!ConnectRoom(currentRooms[loopNum], cur))
                        {
                            loopNum++;
                            if (loopNum >= currentRooms.Count)
                            {
                                Debug.Log("Rooms are not fit well So restart");
                                DestroyRooms();
                                BuildRooms();
                                SetRooms();
                                return;
                            }
                        }
                        currentRooms.Add(cur);
                        ShuffleRoomList();
                    }
            //현재 : currentRoom리스트 단순 배치
            //TODO : 끝방은 첫방으로부터 일정 distance 이상되야 연결되게 구현
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

        OffTile_Door room1Door = null;
        for (int i = 0; i < room1.doorList.Count; i++)
        {
            if (room1.doorList[i].TargetRoom == null)
            {              
                room1Door = room1.doorList[i];
                break;
            }
            if (i == room1.doorList.Count - 1)
            {
                return false;    
            }
        }

        Direction opposite = (Direction)(((int)room1Door.Dir + 2) % 4);
        OffTile_Door room2Door = null;

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
            room2.distance = room1.distance + 1;
            ShuffleDoor(room1);
            ShuffleDoor(room2);
            return true;
        }
        
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
        for(int i=0; i<currentRooms.Count;i++)
        {
            Room var = currentRooms[i];
            int ranNum = Random.Range(i, currentRooms.Count);
            currentRooms[i] = currentRooms[ranNum];
            currentRooms[ranNum] = var;
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
