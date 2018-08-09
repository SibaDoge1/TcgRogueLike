using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;

public static class BuildRoom
{
    static Map currentMap;
    static List<string[,]> battleRooms;
    static List<string[,]> eventRooms;
    static List<string[,]> shopRooms;
    static Vector2Int size;
    static Tile[,] tiles;
    static Room room;
    static string[,] roomData;

    /// <summary>
    /// 해당 타입의 방을 지정해서 반환
    /// </summary>
    public static Room Build(RoomType type, string name)
    {
        room = InstantiateDelegate.Instantiate(Resources.Load("Room") as GameObject, currentMap.transform).GetComponent<Room>();
        room.roomType = type;

        roomData = CsvParser.ReadRoom(1, type, name);
        size = new Vector2Int(roomData.GetLength(1), roomData.GetLength(0));

        room.size = size;
        tiles = new Tile[size.x, size.y];
        room.SetTileArray(tiles);

        Draw();
        GenerateGraph();

        roomData = null;
        return room;
    }

    /// <summary>
    /// 해당타입의 방을 랜덤으로 반환
    /// </summary>
    public static Room Build(RoomType type)
    {

        room = InstantiateDelegate.Instantiate(Resources.Load("Room") as GameObject, currentMap.transform).GetComponent<Room>();
        room.roomType = type;

        roomData = GetRoomData(type);
        size = new Vector2Int(roomData.GetLength(1), roomData.GetLength(0));


        room.size = size;
        tiles = new Tile[size.x, size.y];
        room.SetTileArray(tiles);

        Draw();
        GenerateGraph();

        roomData = null;
        return room;
    }
    /// <summary>
    /// 현재 층의 룸정보파일들의 파일 이름들만 받아서 저장
    /// </summary>
    public static void Init(Map map)
    {
        currentMap = map;
        battleRooms = CsvParser.ReadRoom(map.Floor, RoomType.BATTLE);
        eventRooms = CsvParser.ReadRoom(map.Floor, RoomType.EVENT);
        shopRooms = CsvParser.ReadRoom(map.Floor, RoomType.SHOP);        
    }

    static string[,] GetRoomData(RoomType rt)
    {
        switch(rt)
        {
            case RoomType.BATTLE:
                return battleRooms[Random.Range(0, battleRooms.Count)];
            case RoomType.EVENT:
                return eventRooms[Random.Range(0, eventRooms.Count)];
            case RoomType.SHOP:
                return shopRooms[Random.Range(0, shopRooms.Count)];
            default:
                Debug.Log("Room Type ERROR");
                return null;
        }
    }


    static void Draw()
    {
        int tile; int offtile;  int entity;


        //string을 다시 tile , item, player , height 항목으로 나눕니다
        for (int i = 0; i < size.y ; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                string[] temp = roomData[i,j].Split('/');
                tile = int.Parse(temp[0]);
                offtile = int.Parse(temp[1]);
                entity = int.Parse(temp[2]);



                    Tile tempTile = InstantiateDelegate.ProxyInstantiate(ResourceLoader.instance.LoadTile(tile), room.transform).GetComponent<Tile>();
                    tempTile.SetTile(new Vector2Int(j, (size.y - 1) - i), size);
                    tiles[j, (size.y-1)-i] = tempTile;

                    if (offtile != 0)
                    {
                        OffTile ot = InstantiateDelegate.ProxyInstantiate(ResourceLoader.instance.LoadOffTile(offtile), tempTile.transform).GetComponent<OffTile>();
                        tempTile.offTile = ot;

                        if (offtile < 5)//문
                        {
                            room.doorList.Add(ot as Door);
                        }
                    }

                    if (entity != 0) //엔타이티
                    {
                        Entity et = InstantiateDelegate.ProxyInstantiate(ResourceLoader.instance.LoadEntity(entity)).GetComponent<Entity>();
                        et.SetRoom(room, new Vector2Int(j, (size.y - 1) - i));
                    }                     

            }
        }
    }


    /// <summary>
    /// tile 그래프 
    /// </summary>
     static void GenerateGraph()
    {
        //TODO : HERE IS TEMP!
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (x > 0)
                {
                    tiles[x, y].neighbours.Add(tiles[x - 1, y]);
                }
                if (x < size.x - 1)
                {
                    tiles[x, y].neighbours.Add(tiles[x + 1, y]);
                }
                if (y > 0)
                {
                    tiles[x, y].neighbours.Add(tiles[x, y - 1]);
                }
                if (y < size.y - 1)
                {
                    tiles[x, y].neighbours.Add(tiles[x, y + 1]);
                }
            }
        }
    }
    
}
