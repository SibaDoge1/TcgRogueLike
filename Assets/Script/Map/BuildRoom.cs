using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;

public static class BuildRoom
{
    static Map newMap;
    static List<string> defaultRoomData;
    static List<string> battleRoomData;
    static List<string> bossRoomData;
    static List<string> eventRoomData;
    static List<string> shopRoomData;
    static Vector2Int size;
    static Tile[,] tiles;
    static Room room;
    static string[,] roomData;

    /// <summary>
    /// 해당 타입의 방을 지정해서 반환
    /// </summary>
    public static Room Build(RoomType type, string name)
    {

        room = InstantiateDelegate.Instantiate(Resources.Load("Room") as GameObject, newMap.transform).GetComponent<Room>();
        room.roomType = type;

        roomData = CsvParser.ReadRoom(1, type, name);
        size = new Vector2Int(roomData.GetLength(1), roomData.GetLength(0));
        Debug.Log(size);
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

        room = InstantiateDelegate.Instantiate(Resources.Load("Room") as GameObject, newMap.transform).GetComponent<Room>();
        room.roomType = type;

        roomData = GetRoomData(type);
        size = new Vector2Int(roomData.GetLength(1), roomData.GetLength(0));
        Debug.Log(size);

        room.size = size;
        tiles = new Tile[size.x, size.y];
        room.SetTileArray(tiles);

        Draw();
        GenerateGraph();

        roomData = null;
        return room;
    }
    public static void SetRoomData(Map _newMap)
    {
        newMap = _newMap;

        defaultRoomData = new List<string>();
        battleRoomData = new List<string>();
        bossRoomData = new List<string>();
        eventRoomData = new List<string>();
        shopRoomData = new List<string>();


        Object[] defaultData = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/DEFAULT");
        for (int i = 0; i < defaultData.Length; i++)
        {
            defaultRoomData.Add(defaultData[i].name);
        }
        Object[] battleData = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/BATTLE");
        for (int i = 0; i < battleData.Length; i++)
        {
            battleRoomData.Add(battleData[i].name);
        }
        Object[] bossData = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/BOSS");
        for (int i = 0; i < bossData.Length; i++)
        {
            bossRoomData.Add(bossData[i].name);
        }
        Object[] eventData = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/EVENT");
        for (int i = 0; i < eventData.Length; i++)
        {
            eventRoomData.Add(eventData[i].name);
        }
        Object[] shopData = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/SHOP");
        for (int i = 0; i < shopData.Length; i++)
        {
            shopRoomData.Add(shopData[i].name);
        }
    }
     static string[,] GetRoomData(RoomType rt)
    {
        switch(rt)
        {
            case RoomType.DEFAULT:
                return CsvParser.ReadRoom(newMap.floor, rt, defaultRoomData[Random.Range(0, defaultRoomData.Count)]);
            case RoomType.BATTLE:
                return CsvParser.ReadRoom(newMap.floor, rt, battleRoomData[Random.Range(0, battleRoomData.Count)]);
            case RoomType.BOSS:
                return CsvParser.ReadRoom(newMap.floor, rt, bossRoomData[Random.Range(0, bossRoomData.Count)]);
            case RoomType.EVENT:
                return CsvParser.ReadRoom(newMap.floor, rt, eventRoomData[Random.Range(0, eventRoomData.Count)]);
            case RoomType.SHOP:
                return CsvParser.ReadRoom(newMap.floor, rt, shopRoomData[Random.Range(0, shopRoomData.Count)]);
            default:
                Debug.Log("Room Type ERROR");
                return null;
        }
    }


    static void Draw()
    {
        int tile; int offtile; int item; int entity; int eventlayer;


        //string을 다시 tile , item, player , height 항목으로 나눕니다
        for (int i = 0; i < size.y ; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                string[] temp = roomData[i,j].Split('/');
                tile = int.Parse(temp[0]);
                offtile = int.Parse(temp[1]);
                item = int.Parse(temp[2]);
                entity = int.Parse(temp[3]);
                eventlayer = int.Parse(temp[4]);


                if (tile != 0)
                {
                    Tile tempTile = InstantiateDelegate.ProxyInstantiate(Resources.Load("Tile/"+tile) as GameObject, room.transform).GetComponent<Tile>();
                    tempTile.SetTile(new Vector2Int(j, (size.y - 1) - i), size);
                    tiles[j, (size.y-1)-i] = tempTile;

                    if (offtile != 0)
                    {
                        OffTile offTile = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/" + offtile) as GameObject, tempTile.transform).GetComponent<OffTile>();
                        tempTile.offTile = offTile;
                    }
                    if (item != 0)//아이템
                    {
                    }
                    if (entity != 0) //엔타이티
                    {
                        Entity et = InstantiateDelegate.ProxyInstantiate(Resources.Load("Entity/"+ entity) as GameObject).GetComponent<Entity>();
                        et.SetRoom(room, new Vector2Int(j, (size.y - 1) - i));
                    }
                    if(eventlayer !=0)
                    {
                        EventLayer ev = InstantiateDelegate.ProxyInstantiate(Resources.Load("EventLayer/"+eventlayer) as GameObject).GetComponent<EventLayer>();
                        tempTile.eventLayer = ev;
                        if(eventlayer<5)//문
                        {
                            room.doorList.Add(ev as Door);
                        }
                    }
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
