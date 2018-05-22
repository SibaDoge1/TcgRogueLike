using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public  class Room : MonoBehaviour
{
	public Vector2Int Pos;
    public Vector2Int Size;
	public Room northRoom, southRoom, leftRoom, rightRoom;//Neighbours
    TheTile[,] Tiles;
    public List<ITurnAble> TurnalbeList = new List<ITurnAble>();

    Transform OO;
    Transform TT;
    public TheTile[,] GetTileArrays()
    {
        return Tiles;
    }
    public TheTile GetTile(Vector2Int p)
    {
        return Tiles[p.x, p.y];
    }

    public virtual void DoTurn(int turn)
    {
        foreach(ITurnAble it in TurnalbeList)
        {
            it.DoAct(turn);
        }
    }
	public virtual void SetRoomPos(Vector2Int _Pos,Vector2Int _Size)
    {
        Pos = _Pos;
        Size = _Size;
        transform.localPosition = MapGenerator.instance.GetRoomPosition(Pos);
        gameObject.name = "Room_" + Pos.x + "_" + Pos.y;

        TT = transform.Find("Tiles");

        SetTiles();
        MakeWall();
    }

    protected virtual void SetTiles()
    {
        Tiles = new TheTile[Size.x,Size.y];
        for (int i = 0; i <Size.x; i++)
        {
            for(int j=0; j<Size.y; j++)
            {
              TheTile tempTile = Instantiate(Resources.Load("Tile/default") as GameObject, TT).GetComponent<TheTile>();
              tempTile.SetTile(new Vector2Int(i, j));
                Tiles[i, j] = tempTile;
            }
        }
        GenerateGraph();
    }
    /// <summary>
    /// 이웃타일 등록
    /// </summary>
    protected void GenerateGraph()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if (x > 0)
                {
                    Tiles[x, y].neighbours.Add(Tiles[x - 1, y]);
                }
                if (x < Size.x - 1)
                {
                    Tiles[x, y].neighbours.Add(Tiles[x + 1, y]);
                }
                if (y > 0)
                {
                    Tiles[x, y].neighbours.Add(Tiles[x, y - 1]);
                }
                if (y < Size.y - 1)
                {
                    Tiles[x, y].neighbours.Add(Tiles[x, y + 1]);
                }
            }
        }
    }

    public void MakeWall()
    {
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                if (i == 0 || j == 0 || j == Size.y - 1 || i == Size.x - 1)
                {
                    OnTileObject tempWall = Instantiate(Resources.Load("Structure/wall") as GameObject, transform).GetComponent<OnTileObject>();
                    tempWall.SetRoom(this, new Vector2Int(i, j));
                }
            }
        }
    }

    public virtual void SetDoors()
    {
        if (northRoom)
        {
           Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(northRoom);
            Tiles[Size.x / 2, Size.y - 1].TileInfo = temp;
        }
        if (rightRoom)
        {
            Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(rightRoom);
            Tiles[Size.x - 1, Size.y / 2].TileInfo = temp;
        }
        if (southRoom)
        {
            Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(southRoom);
            Tiles[Size.x / 2, 0].TileInfo = temp;
        }
        if (leftRoom)
        {
            Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(leftRoom);
            Tiles[0, Size.y / 2].TileInfo = temp;
        }
        OpenDoors();
    }
    public void TempMakingEnemy()
    {
        Vector2Int temp1 = new Vector2Int(Random.Range(2,8), Random.Range(2,5));
        Enemy temp = Instantiate(Resources.Load("Enemy/Goblin") as GameObject).GetComponent<Enemy>();
        temp.SetRoom(this, temp1);
    }
    public virtual void OpenDoors()
    {
        if (northRoom)
        { 
            Tiles[Size.x / 2, Size.y - 1].onTile.DestroyThis();
        }
        if (rightRoom)
        {
            Tiles[Size.x - 1, Size.y / 2].onTile.DestroyThis();
        }
        if (southRoom)
        {
            Tiles[Size.x / 2, 0].onTile.DestroyThis();
        }
        if (leftRoom)
        {
            Tiles[0, Size.y / 2].onTile.DestroyThis();
        }
    }
}
