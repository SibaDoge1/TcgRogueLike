using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;

public abstract class RoomSeed
{
    public RoomSeed(Room _room)
    {
        room = _room;
        SetShape();
        SetDoors();
        GenerateGraph();
    }
    public Vector2Int size;

    protected Tile[,] tiles;
    protected Room room;

    /// <summary>
    /// 타일,벽,장애물 등 전체적인 위치 지정
    /// </summary>
    public virtual void SetShape()
    {
        //임시용 타일그리기
        int count = 0;
        tiles = new Tile[size.x, size.y];
        for (int i = 0; i < size.x; i++)
        {
            count++;
            for (int j = 0; j < size.y; j++)
            {
                count++;
                Tile tempTile;
                if (count % 2 == 0)
                {
                    tempTile = InstantiateDelegate.ProxyInstantiate(Resources.Load("Tile/tile3") as GameObject, room.transform).GetComponent<Tile>();
                }
                else
                {
                    tempTile = InstantiateDelegate.ProxyInstantiate(Resources.Load("Tile/tile5") as GameObject, room.transform).GetComponent<Tile>();
                }
                tempTile.SetTile(new Vector2Int(i, j), size);
                tiles[i, j] = tempTile;
            }
        }
        room.SetTileArray(tiles);
        //임시용 벽만들기
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (i == 0 || j == 0 || j == size.y - 1 || i == size.x - 1)
                {
                    Str_Wall tempWall = InstantiateDelegate.ProxyInstantiate(Resources.Load("Structure/wall") as GameObject).GetComponent<Str_Wall>();
                    tempWall.SetRoom(room, new Vector2Int(i, j));
                }
            }
        }
    }

    /// <summary>
    /// 문 위치 설정
    /// </summary>
    public virtual void SetDoors()
    {
        List<Door> doorlist = new List<Door>();
        //임시용 랜덤 문 위치 설정
        Door northDoor = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[Random.Range(size.x / 4, size.x / 4 * 3), size.y - 1].SetOffTile(northDoor);
        northDoor.Dir = Direction.NORTH;
        doorlist.Add(northDoor);

        Door eastDoor = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[size.x - 1, Random.Range(size.y / 4, size.y / 4 * 3)].SetOffTile(eastDoor);
        eastDoor.Dir = Direction.EAST;
        doorlist.Add(eastDoor);

        Door southDoor = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[Random.Range(size.x / 4, size.x / 4 * 3), 0].SetOffTile(southDoor);
        southDoor.Dir = Direction.SOUTH;
        doorlist.Add(southDoor);

        Door westDoor = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[0, Random.Range(size.y / 4, size.y / 4 * 3)].SetOffTile(westDoor);
        westDoor.Dir = Direction.WEST;
        doorlist.Add(westDoor);

        ///shuffle
        for(int i=0; i< doorlist.Count; i++)
        {
            int rand = Random.Range(i, doorlist.Count);
            Door temp = doorlist[i];
            doorlist[i] = doorlist[rand];
            doorlist[rand] = temp;
        }
        room.doorList = doorlist;
    }



    /// <summary>
    /// tile 그래프 
    /// </summary>
    protected void GenerateGraph()
    {
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
                if (x < size.x - 1 && y < size.y - 1)
                {
                    tiles[x, y].neighbours.Add(tiles[x + 1, y + 1]);
                }
                if (x < size.x - 1 && y > 0)
                {
                    tiles[x, y].neighbours.Add(tiles[x + 1, y - 1]);
                }
                if (x > 0 && y > 0)
                {
                    tiles[x, y].neighbours.Add(tiles[x - 1, y - 1]);
                }
                if (x > 0 && y < size.y - 1)
                {
                    tiles[x, y].neighbours.Add(tiles[x - 1, y + 1]);
                }
            }
        }
    }
    
}
public class StartRoom : RoomSeed
{
    public StartRoom(Room room):base(room)
    {
        room.OpenDoors();
        room.SetStartRoom();
    }
    public override void SetShape()
    {
        size = new Vector2Int(13,13);
        room.size = size;
        base.SetShape();
    }

    public override void SetDoors()
    {
        List<Door> doorlist = new List<Door>();
        //임시용 랜덤 문 위치 설정
        Door northDoor = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[3,size.y-1].SetOffTile(northDoor);
        northDoor.Dir = Direction.NORTH;
        doorlist.Add(northDoor);

        Door northDoor2 = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[11, size.y - 1].SetOffTile(northDoor2);
        northDoor2.Dir = Direction.NORTH;
        doorlist.Add(northDoor2);

        Door eastDoor = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[size.x - 1, 3].SetOffTile(eastDoor);
        eastDoor.Dir = Direction.EAST;
        doorlist.Add(eastDoor);

        Door eastDoor2 = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[size.x - 1, 11].SetOffTile(eastDoor2);
        eastDoor2.Dir = Direction.EAST;
        doorlist.Add(eastDoor2);

        Door southDoor = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[3, 0].SetOffTile(southDoor);
        southDoor.Dir = Direction.SOUTH;
        doorlist.Add(southDoor);

        Door southDoor2 = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[11, 0].SetOffTile(southDoor2);
        southDoor2.Dir = Direction.SOUTH;
        doorlist.Add(southDoor2);

        Door westDoor = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[0, 3].SetOffTile(westDoor);
        westDoor.Dir = Direction.WEST;
        doorlist.Add(westDoor);

        Door westDoor2 = InstantiateDelegate.ProxyInstantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
        tiles[0, 11].SetOffTile(westDoor2);
        westDoor2.Dir = Direction.WEST;
        doorlist.Add(westDoor2);


        ///shuffle
        for (int i = 0; i < doorlist.Count; i++)
        {
            int rand = Random.Range(i, doorlist.Count);
            Door temp = doorlist[i];
            doorlist[i] = doorlist[rand];
            doorlist[rand] = temp;
        }
        room.doorList = doorlist;
    }
}
public class BattleRoom : RoomSeed
{
    protected List<int> enemyList;
    public BattleRoom(Room room, List<int> _enemyList) :base(room)
    {       
        enemyList = _enemyList;
        MakeEnemy();
    }

    public override void SetShape()
    {
        size = new Vector2Int(Arandom.GetRandomOddInt(5,10), Arandom.GetRandomOddInt(5,10));
        room.size = size;
        base.SetShape();
    }
    /// <summary>
    /// 적 생성
    /// </summary>
    public void MakeEnemy()
    {

    }

}
public class BossRoom : RoomSeed
{
    protected List<int> enemyList;
    public BossRoom(Room room, List<int> _enemyList) :base(room)
    {
        enemyList = _enemyList;
        MakeEnemy();
    }
    public override void SetShape()
    {
        size = new Vector2Int(13, 13);
        room.size = size;
        base.SetShape();
    }
    public void MakeEnemy()
    {

    }
}
