using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public static class MapGenerator 
{



	#region Interface
	public static Map GetNewMap(int seed,Vector2Int _mapSize,int _roomNum)
    {
		//TODO : Generate Map With Seed
		mapSize = _mapSize;
		roomNum = _roomNum;
		rooms = new Room[mapSize.x, mapSize.y];

		if(roomNum > mapSize.x*mapSize.y)
		{
			roomNum = mapSize.x * mapSize.y;
		}
        Map newMap = GameObject.Find("Map").GetComponent<Map>();

        CreateRooms(newMap); 
		SetRoomDoors(newMap);

		Room startRoom = currentRooms [0];
        newMap.SetStartRoom(startRoom);
        newMap.Room = currentRooms;

        return newMap;
    }
    #endregion

    static Room[,] rooms;
    static Vector2Int mapSize;
    static int roomNum;
	static Vector2Int roomSize = new Vector2Int(12,8);
    static List<Room> currentRooms = new List<Room>();






	private static void CreateRooms(Map newMap)
    {
	    List<Room> exploreRooms = new List<Room>();

        Vector2Int startPos =  new Vector2Int(Mathf.RoundToInt(mapSize.x / 2), Mathf.RoundToInt(mapSize.y / 2));
        Room startRoom = InstantiateDelegate.Instantiate(Resources.Load("Room/default") as GameObject, newMap.transform).GetComponent<Room>();
        startRoom.SetRoomPos(new Vector2Int(startPos.x, startPos.y),roomSize,0);
        rooms[startPos.x, startPos.y] = startRoom;
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
            Vector2Int target = tempRoom.pos+getRandomDir();
            if(CheckAvailPos(target))
            {
                
                Room newRoom = InstantiateDelegate.Instantiate(Resources.Load("Room/default") as GameObject, newMap.transform).GetComponent<Room>();
                rooms[target.x, target.y] = newRoom;
                if(currentRooms.Count == roomNum-1)
                    newRoom.SetRoomPos(target, roomSize,1);
                else
                    newRoom.SetRoomPos(target, roomSize,0);
                currentRooms.Add(newRoom);
                exploreRooms.Add(newRoom);
                newRoom.GenerateEnemy();
            }
        }
    }
    private static void SetRoomDoors(Map newMap)
    {
        for(int i=0; i< currentRooms.Count;i++)
        {
            Vector2Int temp = currentRooms[i].pos;
            if(temp.y+1<mapSize.y && rooms[temp.x,temp.y+1] != null)
            {
                currentRooms[i].northRoom = rooms[temp.x,temp.y+1];
            }
            if(temp.x+1<mapSize.x && rooms[temp.x+1, temp.y] != null)
            {
                currentRooms[i].rightRoom = rooms[temp.x + 1, temp.y];
            }
            if (temp.y-1>=0 && rooms[temp.x, temp.y - 1] != null)
            {
                currentRooms[i].southRoom = rooms[temp.x, temp.y - 1];
            }
            if(temp.x-1>=0 && rooms[temp.x-1,temp.y]!=null)
            {
                currentRooms[i].leftRoom = rooms[temp.x - 1, temp.y];
            }
            currentRooms[i].SetDoors();
        }
        
    }


    private static bool CheckAvailPos(Vector2Int target)
    {
        if(target.x >=0 && target.x <mapSize.x
            && target.y>=0 && target.y<mapSize.y &&
            rooms[target.x,target.y]==null)
        {
            return true;
        }else
        {
            return false;
        }
    }
    private static Vector2Int getRandomDir()
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
    private static bool CheckExploreAble(Room temp)
    {
        int count = 0;
        if(temp.pos.x==0 || rooms[temp.pos.x-1,temp.pos.y]!=null)//왼쪽 체크
        {
            count++;
        }
        if (temp.pos.x == mapSize.x-1 || rooms[temp.pos.x + 1, temp.pos.y] != null)//오른쪽 체크
        {
            count++;
        }
        if (temp.pos.y == mapSize.y-1 || rooms[temp.pos.x, temp.pos.y+1] != null)//위쪽 체크
        {
            count++;
        }
        if (temp.pos.y == 0 || rooms[temp.pos.x, temp.pos.y-1] != null)//아래쪽 체크
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
