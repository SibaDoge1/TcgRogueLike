using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;

public static class BuildRoom
{
    static Map currentMap;
    static Dictionary<string,string[,]> battleRooms;
    static Dictionary<string,string[,]> eventRooms;
    static Dictionary<string, string[,]> bossRooms;
    static Vector2Int size;
    static Tile[,] tiles;
    static Room currentRoom;
    static string[,] roomData;

    /// <summary>
    /// 해당 타입의 방을 지정해서 반환
    /// </summary>
    public static Room Build(RoomType type, string name)
    {

        currentRoom = ArchLoader.instance.GetRoom();
        currentRoom.transform.parent = currentMap.transform;
        currentRoom.roomType = type;

        roomData = CsvParser.ReadRoom(currentMap.Floor, type, name);
        size = new Vector2Int(roomData.GetLength(0), roomData.GetLength(1));

        currentRoom.size = size;
        tiles = new Tile[size.x, size.y];
        currentRoom.SetTileArray(tiles);
        currentRoom.RoomName = name;
        UIManager.instance.RoomDebugText(currentRoom.RoomName, true);

        Draw();
        GenerateGraph();
        roomData = null;

        return currentRoom;
    }

    /// <summary>
    /// 해당타입의 방을 랜덤으로 반환
    /// </summary>
    public static Room Build(RoomType type)
    {

        currentRoom = ArchLoader.instance.GetRoom();
        currentRoom.transform.parent = currentMap.transform;
        currentRoom.roomType = type;

        KeyValuePair<string, string[,]> temp = GetRoomData(type);
        currentRoom.RoomName = temp.Key;
        roomData = temp.Value;
        size = new Vector2Int(roomData.GetLength(0), roomData.GetLength(1));

        currentRoom.size = size;
        tiles = new Tile[size.x, size.y];
        currentRoom.SetTileArray(tiles);
        UIManager.instance.RoomDebugText(currentRoom.RoomName,true);

        Draw();
        GenerateGraph();
        roomData = null;

        return currentRoom;
    }


    public static void Init(Map map)
    {
        currentMap = map;
        battleRooms = CsvParser.ReadRoom(map.Floor, RoomType.BATTLE);
        eventRooms = CsvParser.ReadRoom(map.Floor, RoomType.EVENT);
        bossRooms = CsvParser.ReadRoom(map.Floor, RoomType.BOSS);
    }


    static KeyValuePair<string, string[,]> GetRoomData(RoomType rt)
    {
        switch(rt)
        {
            case RoomType.BATTLE:
                return battleRooms.ElementAt(Random.Range(0, battleRooms.Keys.Count));
            case RoomType.EVENT:
                return eventRooms.ElementAt(Random.Range(0,eventRooms.Keys.Count));
            case RoomType.BOSS:
                return bossRooms.ElementAt(Random.Range(0, bossRooms.Keys.Count));
            default:
                Debug.Log("Room Type ERROR");
                return battleRooms.ElementAt(0);
        }
    }


    static void Draw()
    {
        short tile; short offtile;  short entity;
        Debug.Log("Draw Start , Floor : " + currentMap.Floor + " RoomType : " + currentRoom.roomType + " RoomName :" + currentRoom.RoomName);

        //string을 다시 tile , item, player , height 항목으로 나눕니다
        for (int i = 0; i < size.y ; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                string[] temp = roomData[j,i].Split('/');
                tile = short.Parse(temp[0]);
                offtile = short.Parse(temp[1]);
                entity = short.Parse(temp[2]);

                    Tile tempTile = ArchLoader.instance.GetTile(tile);
                    tempTile.Init(tile);
                    tempTile.SetRoom(currentRoom, new Vector2Int(j, (size.y - 1) - i));
                    tiles[j, (size.y-1)-i] = tempTile;

                    if (offtile != 0)
                    {
                        OffTile ot = ArchLoader.instance.GetOffTile(offtile);
                        ot.Init(offtile);
                        tempTile.offTile = ot;
                        if (offtile < 5)//문
                        {
                            currentRoom.doorList.Add(ot as OffTile_Door);
                        }else if(offtile == 101)//세이브
                        {
                            currentMap.SaveTile = ot as OffTile_Save;
                        }
                    }

                    if (entity != 0) //엔타이티
                    {
                        Entity et = ArchLoader.instance.GetEntity(entity);
                        et.Init(entity);
                        et.SetRoom(currentRoom, new Vector2Int(j, (size.y - 1) - i));
                    }                     
            }
        }
    }


    /// <summary>
    /// tile 그래프 
    /// </summary>
     static void GenerateGraph()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                //상하좌우
                if (x > 0)
                {
                    tiles[x, y].crossNeighbours.Add(tiles[x - 1, y]);
                }
                if (x < size.x - 1)
                {
                    tiles[x, y].crossNeighbours.Add(tiles[x + 1, y]);
                }
                if (y > 0)
                {
                    tiles[x, y].crossNeighbours.Add(tiles[x, y - 1]);
                }
                if (y < size.y - 1)
                {
                    tiles[x, y].crossNeighbours.Add(tiles[x, y + 1]);
                }

                //대각선 방향
                if(x>0 && y>0)
                {
                    tiles[x, y].diagonalNeighbours.Add(tiles[x-1, y-1]);
                }
                if(x<size.x-1 && y>0)
                {
                    tiles[x, y].diagonalNeighbours.Add(tiles[x + 1, y - 1]);
                }
                if (x<size.x-1 && y<size.y-1)
                {
                    tiles[x, y].diagonalNeighbours.Add(tiles[x + 1, y + 1]);
                }
                if (x>0 && y<size.y-1)
                {
                    tiles[x, y].diagonalNeighbours.Add(tiles[x - 1, y + 1]);
                }

                tiles[x, y].allNeighbours = new List<Tile>(tiles[x,y].crossNeighbours);
                tiles[x, y].allNeighbours.AddRange(tiles[x, y].diagonalNeighbours);
            }
        }
    }
    
}
