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
	public List<Enemy> enemyList = new List<Enemy>();
	private Tile playerTile;
	private bool isCleared = false;
	public bool IsCleares{
		get{ return isCleared; }
	}
    Transform tileParent;

	public static int CalcRange(Vector2Int a, Vector2Int b){
		return Mathf.Max (Mathf.Abs (a.x - b.x), Mathf.Abs (a.y - b.y));
	}

	public void OnEnemyDead(Enemy enemy){
		enemyList.Remove (enemy);
		if (enemyList.Count != 0) {
			for (int i = 0; i < enemyList.Count; i++) {
				if (enemyList [i] != null) {	//하나라도 남아있으면
					return;
				}
			}
		}
		//Enemy All Dead
		isCleared = true;
		OpenDoors ();
	}

	public bool IsEnemyAlive(){
		if (enemyList == null) {
			return false;
		}
		return enemyList.Count > 0;
	}

	public Tile WorldToTile(Vector3 worldPos){
		Vector3 sizeTemp = new Vector3 (size.x / 2, size.y / 2, 0);
		Vector3 p = transform.position - sizeTemp;
		Vector3 temp = worldPos - p;
        return GetTile(new Vector2Int((int)temp.x, (int)temp.y));
	}

	public Tile GetPlayerTile(){
		return playerTile;
	}
	public void SetPlayerTile(Tile tile_){
		playerTile = tile_;
	}

	public Tile[,] GetTileArrays()
    {
        return tiles;
    }

    public List<Tile> GetSpawnableTiles()
    {
        List<Tile> temp = new List<Tile>();
        for(int i=2; i<size.x-2;i++)
        {
            for(int j=2; j<size.y-2;j++)
            {
                if (!tiles[i, j].OnTileObj)
                    temp.Add(tiles[i, j]);
            }
        }
        return temp;
    }

	public Tile GetTile(Vector2Int p)
    {
        if (p.x >= size.x || p.y >= size.y || p.x < 0 || p.y < 0)
            return null;
        else
        return tiles[p.x, p.y];
    }

    public List<Tile> GetDoorTiles()
    {
        List<Tile> temp = new List<Tile>();
        if (northRoom)
        {
            temp.Add(tiles[size.x / 2, size.y - 1]);
        }
        if (rightRoom)
        {
            temp.Add(tiles[size.x - 1, size.y / 2]);
        }
        if (southRoom)
        {
            temp.Add(tiles[size.x / 2, 0]);
        }
        if (leftRoom)
        {
            temp.Add(tiles[0, size.y / 2]);
        }
        return temp;
    }

	public void DisableRoom(){
		
	}


	public virtual void OpenDoors()
	{
		if (northRoom)
		{ 
			tiles[size.x / 2, size.y - 1].OnTileObj.currentHp = 0;
		}
		if (rightRoom)
		{
			tiles[size.x - 1, size.y / 2].OnTileObj.currentHp = 0;
		}
		if (southRoom)
		{
			tiles[size.x / 2, 0].OnTileObj.currentHp = 0;
		}
		if (leftRoom)
		{
			tiles[0, size.y / 2].OnTileObj.currentHp = 0;
		}
	}



	#region Private
    protected virtual void SetTiles()
    {
        int count = 0;
		tiles = new Tile[size.x,size.y];
        for (int i = 0; i <size.x; i++)
        {
            count++;
            for(int j=0; j<size.y; j++)
            {
                count++;
                Tile tempTile;
                if(count%2 == 0)
                {
                     tempTile = Instantiate(Resources.Load("Tile/tile3") as GameObject, tileParent).GetComponent<Tile>();
                }else
                {
                     tempTile = Instantiate(Resources.Load("Tile/tile5") as GameObject, tileParent).GetComponent<Tile>();
                }
                tempTile.SetTile(new Vector2Int(i, j), size);
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
                if(x<size.x-1 && y<size.y-1)
                {
                   	tiles[x, y].neighbours.Add(tiles[x+1, y + 1]);
                }
                if (x < size.x - 1 && y > 0)
                {
                    tiles[x, y].neighbours.Add(tiles[x + 1, y - 1]);
                }
                if (x > 0  && y > 0)
                {
                    tiles[x, y].neighbours.Add(tiles[x - 1, y - 1]);
                }
                if (x > 0 && y < size.y-1)
                {
                    tiles[x, y].neighbours.Add(tiles[x - 1, y + 1]);
                }
            }
        }
    }

	private void MakeWall()
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
	#endregion

	#region MapGenerate
    public virtual void SetDoors()
    {
        if (northRoom)
        {
           Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(northRoom);
            tiles[size.x / 2, size.y - 1].SetOffTile(temp);
        }
        if (rightRoom)
        {
            Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(rightRoom);
            tiles[size.x - 1, size.y / 2].SetOffTile(temp);
        }
        if (southRoom)
        {
            Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(southRoom);
            tiles[size.x / 2, 0].SetOffTile(temp);
        }
        if (leftRoom)
        {
            Door temp = Instantiate(Resources.Load("OffTile/Door") as GameObject).GetComponent<Door>();
            temp.SetTargetRoom(leftRoom);
            tiles[0, size.y / 2].SetOffTile(temp);
        }

    }


    private RoomSeed seed;
    public RoomSeed GetSeed()
    {
        return seed;
    }
    public void SetSeed(RoomSeed _seed)
    {
        if(seed == null)
        {
            seed = _seed;
        }else
        {
            Debug.LogError("시드 중복생성");
        }
    }
    public virtual void MakeRoom(Vector2Int _Pos,Vector2Int _Size)
	{
		pos = _Pos;
		size = _Size;
		transform.localPosition = new Vector3(2 * pos.x * 12, 2 * pos.y * 8, 0); ;
		gameObject.name = "Room_" + pos.x + "_" + pos.y;

		tileParent = transform.Find("Tiles");

		SetTiles();
		MakeWall();
	}
	#endregion
}
