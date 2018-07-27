using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

/// <summary>
/// ROOM 클래스 , 타일 정보와 방안의 적 정보를 가지고있다.
/// 생성에 관한것은 RoomSeed 클래스에서 처리
/// </summary>
public class Room : MonoBehaviour
{
    public Vector2Int size;
    public List<Door> doorList;
	Tile[,] tiles;

    public RoomType roomType;
	public List<Enemy> enemyList = new List<Enemy>();
    private bool isVisited = false;
    public bool IsVisited
    {
        get { return isVisited; }
        set { isVisited = value; }
    }
	private bool isCleared = false;
	public bool IsCleares{
		get{ return isCleared; }
	}


	public void SetStartRoom(){
		isCleared = true;
        OpenDoors();
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
		GameManager.instance.OnPlayerClearRoom ();
	}

	public bool IsEnemyAlive(){
		if (enemyList == null) {
			return false;
		}
		return enemyList.Count > 0;
	}

	public Tile[,] GetTileArrays()
    {
        return tiles;
    }
    /// <summary>
    /// null인 tile은 제외
    /// </summary>
    /// <returns></returns>
    public List<Tile> GetTileToList()
    {
        List<Tile> temp = new List<Tile>();
       for(int i=0;i<size.x;i++)
        {
            for(int j=0;j<size.y;j++)
            {
                if(tiles[i,j] != null)
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
    public void SetTileArray(Tile[,] t)
    {
        tiles = t;
    }

	public void DisableRoom()
    {
		
	}

    public void DeleteDoors()
    {
        for (int i = doorList.Count-1; i >= 0; i--)
        {
            if (doorList[i].TargetRoom == null)
            {
                doorList.RemoveAt(i);
            }
        }
    }


    public virtual void OpenDoors()
	{
        for (int i = 0; i < doorList.Count; i++)
        {
            if(doorList[i].TargetRoom != null)
            doorList[i].ThisTile.OnTileObj.currentHp = 0;
        }
    }
    
}
