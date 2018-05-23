using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public class Room : MonoBehaviour
{
	public Vector2Int pos;
    public Vector2Int size;
	public Room northRoom, southRoom, leftRoom, rightRoom;//Neighbours
	Tile[,] tiles;
    public List<ITurnAble> TurnalbeList = new List<ITurnAble>();
    public Player player;

    Transform tileParent;
	public Tile[,] GetTileArrays()
    {
        return tiles;
    }
	public Tile GetTile(Vector2Int p)
    {
        return tiles[p.x, p.y];
    }

    public virtual void DoTurn()
    {
        foreach(ITurnAble it in TurnalbeList)
        {
            it.DoAct();
        }
    }
	public virtual void SetRoomPos(Vector2Int _Pos,Vector2Int _Size)
    {
        pos = _Pos;
        size = _Size;
        transform.localPosition = MapGenerator.instance.GetRoomPosition(pos);
        gameObject.name = "Room_" + pos.x + "_" + pos.y;

        tileParent = transform.Find("Tiles");

        SetTiles();
        MakeWall();
    }

    protected virtual void SetTiles()
    {
		tiles = new Tile[size.x,size.y];
        for (int i = 0; i <size.x; i++)
        {
            for(int j=0; j<size.y; j++)
            {
				Tile tempTile = Instantiate(Resources.Load("Tile/default") as GameObject, tileParent).GetComponent<Tile>();
              	tempTile.SetTile(new Vector2Int(i, j));
                tiles[i, j] = tempTile;
            }
        }
        GenerateGraph();
    }
    /// <summary>
    /// 이웃타일 등록
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
            }
        }
    }

    public void MakeWall()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (i == 0 || j == 0 || j == size.y - 1 || i == size.x - 1)
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
            tiles[size.x / 2, size.y - 1].OffTileObj = temp;
        }
        if (rightRoom)
        {
            Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(rightRoom);
            tiles[size.x - 1, size.y / 2].OffTileObj = temp;
        }
        if (southRoom)
        {
            Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(southRoom);
            tiles[size.x / 2, 0].OffTileObj = temp;
        }
        if (leftRoom)
        {
            Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(leftRoom);
            tiles[0, size.y / 2].OffTileObj = temp;
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
            tiles[size.x / 2, size.y - 1].OnTileObj.DestroyThis();
        }
        if (rightRoom)
        {
            tiles[size.x - 1, size.y / 2].OnTileObj.DestroyThis();
        }
        if (southRoom)
        {
            tiles[size.x / 2, 0].OnTileObj.DestroyThis();
        }
        if (leftRoom)
        {
            tiles[0, size.y / 2].OnTileObj.DestroyThis();
        }
    }
}
