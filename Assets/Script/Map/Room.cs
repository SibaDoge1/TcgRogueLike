using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public enum RoomType
{
    DEFAULT,
    BATTLE,
    EVENT,
    BOSS,
    SHOP
}
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
	private bool isCleared = false;
	public bool IsCleares{
		get{ return isCleared; }
	}

	public static int CalcRange(Vector2Int a, Vector2Int b){
		return Mathf.Max (Mathf.Abs (a.x - b.x), Mathf.Abs (a.y - b.y));
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

	public Tile WorldToTile(Vector3 worldPos){
		Vector3 temp = worldPos - transform.position + new Vector3(0.5f,-0.5f,0);
        temp = new Vector3(-temp.y,temp.x, 0);
        return GetTile(new Vector2Int((int)(temp.x), (int)temp.y));
	}

	public Tile[,] GetTileArrays()
    {
        return tiles;
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
    public List<Vector2Int> GetDoorPos()
    {
        List<Vector2Int> temp = new List<Vector2Int>();
        for(int i=0; i<doorList.Count;i++)
        {
            if (doorList[i].TargetRoom != null)
                temp.Add(doorList[i].ThisTile.pos);
        }
        return temp;
    }

	public void DisableRoom(){
		
	}

    public void DeleteDoors()
    {
        for (int i = doorList.Count-1; i > 0; i--)
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
