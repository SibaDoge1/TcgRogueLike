using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapGenerator : MonoBehaviour
{

    public static MapGenerator instance;
    private void Awake()
    {
        instance = this;
    }



    public Map GetNewMap(int seed)
    {
        //Room Gen

        //Room Reference Connect;

        //Set Start Room;
        Room startRoom = GetComponent<Room>();

        Map result = new Map(startRoom);
        return result;
    }



    public Vector2Int mapSize;
    public int roomNum;
    public  Vector2Int roomOffset;
    
    public Vector3 GetRoomPosition(Vector2Int pos)
    {
        return new Vector3(2*pos.x * roomOffset.x,2*pos.y *roomOffset.y, 0);
    }
    public Vector3 GetTilePosition(Vector2Int pos)
    {
        return new Vector3(-roomOffset.x / 2 + 0.5f + pos.x, -roomOffset.y / 2 + 0.5f + pos.y, 0);
    }
     
    Room[,] Rooms;
    List<Room> currentRooms = new List<Room>();
    List<Room> exploreRooms = new List<Room>();
    void Start()
    {       
        //SetMap();
    }

    void SetMap()
    {
        
        Rooms = new Room[mapSize.x, mapSize.y];

        if(roomNum > mapSize.x*mapSize.y)
        {
            roomNum = mapSize.x * mapSize.y;
        }

        CreateRooms(); 
        SetRoomDoors();
        Player.instance.SpawnToRoom(currentRooms[0]);
        //currentRooms[0].OpenDoors();
    }



    void CreateRooms()
    {
        Vector2Int startPos =  new Vector2Int(Mathf.RoundToInt(mapSize.x / 2), Mathf.RoundToInt(mapSize.y / 2));
        Room startRoom = Instantiate(Resources.Load("Room/default") as GameObject, transform).GetComponent<Room>();
        startRoom.SetRoomPos(new Vector2Int(startPos.x, startPos.y),roomOffset);
        Rooms[startPos.x, startPos.y] = startRoom;
        currentRooms.Add(startRoom);
        exploreRooms.Add(startRoom);
        while(roomNum>currentRooms.Count)
        {
            Room tempRoom = exploreRooms[exploreRooms.Count - 1];
            if(!CheckExploreAble(tempRoom))
            {
                exploreRooms.RemoveAt(exploreRooms.Count - 1);
                continue;
            }
            Vector2Int target = tempRoom.Pos+getRandomDir();
            if(CheckAvailPos(target))
            {
                
                Room newRoom = Instantiate(Resources.Load("Room/default") as GameObject, transform).GetComponent<Room>();
                Rooms[target.x, target.y] = newRoom;
                newRoom.SetRoomPos(target,roomOffset);
                currentRooms.Add(newRoom);
                exploreRooms.Add(newRoom);
                newRoom.TempMakingEnemy();
            }
        }
    }
    void SetRoomDoors()
    {
        for(int i=0; i< currentRooms.Count;i++)
        {
            Vector2Int temp = currentRooms[i].Pos;
            if(temp.y+1<mapSize.y && Rooms[temp.x,temp.y+1] != null)
            {
                currentRooms[i].northRoom = Rooms[temp.x,temp.y+1];
            }
            if(temp.x+1<mapSize.x && Rooms[temp.x+1, temp.y] != null)
            {
                currentRooms[i].rightRoom = Rooms[temp.x + 1, temp.y];
            }
            if (temp.y-1>=0 && Rooms[temp.x, temp.y - 1] != null)
            {
                currentRooms[i].southRoom = Rooms[temp.x, temp.y - 1];
            }
            if(temp.x-1>=0 && Rooms[temp.x-1,temp.y]!=null)
            {
                currentRooms[i].leftRoom = Rooms[temp.x - 1, temp.y];
            }
            currentRooms[i].SetDoors();
        }
        
    }


    bool CheckAvailPos(Vector2Int target)
    {
        if(target.x >=0 && target.x <mapSize.x
            && target.y>=0 && target.y<mapSize.y && 
            Rooms[target.x,target.y]==null)
        {
            return true;
        }else
        {
            return false;
        }
    }
    Vector2Int getRandomDir()
    {
       int randomValue = Random.Range(0, 4);
        switch(randomValue)
        {
            case 0: //UP
                return Vector2Int.up;
            case 1: //RIGHT
                return Vector2Int.right;
            case 2: //DOWN
                return Vector2Int.down;
            case 3: //LEFT
                return Vector2Int.left;
            default:
                return Vector2Int.up;
        }
    }
    /// <summary>
    /// 사방이 막혀있는 방이라면 false값을 리턴합니다.
    /// </summary>
    /// <returns></returns>
    bool CheckExploreAble(Room temp)
    {
        int count = 0;
        if(temp.Pos.x==0 || Rooms[temp.Pos.x-1,temp.Pos.y]!=null)//왼쪽 체크
        {
            count++;
        }
        if (temp.Pos.x == mapSize.x-1 || Rooms[temp.Pos.x + 1, temp.Pos.y] != null)//오른쪽 체크
        {
            count++;
        }
        if (temp.Pos.y == mapSize.y-1 || Rooms[temp.Pos.x, temp.Pos.y+1] != null)//위쪽 체크
        {
            count++;
        }
        if (temp.Pos.y == 0 || Rooms[temp.Pos.x, temp.Pos.y-1] != null)//아래쪽 체크
        {
            count++;
        }


        if (count==4)
        {
            return false; 
        }
        else
        {
            return true;
        }
    }
}
