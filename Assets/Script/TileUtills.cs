using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;


/// <summary>
/// Player의 스킬 함수 작성시 도움되는 함수들 입니다.
/// </summary>
public static class TileUtils
{
  
    /// <summary>
    /// 십자로 타일 가져오기
    /// </summary>
	public static List<Tile> CrossRange(Tile center, int radius)
    {
		List<Tile> crossList = new List<Tile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y;

        for(int i=1; i<=radius; i++)
        {
			//TODO : Array out of bound Check
			crossList.Add (GameManager.instance.GetCurrentRoom ().GetTile (new Vector2Int (x, y + 1)));
			crossList.Add (GameManager.instance.GetCurrentRoom ().GetTile (new Vector2Int (x, y - 1)));
			crossList.Add (GameManager.instance.GetCurrentRoom ().GetTile (new Vector2Int (x + 1, y)));
			crossList.Add (GameManager.instance.GetCurrentRoom ().GetTile (new Vector2Int (x - 1, y)));
        }
        return crossList;
    }
    /// <summary>
    /// 원모양으로 타일 가져오기
    /// </summary>
	public static List<Tile> CircleRange(Tile center,int radius)
    {
		List<Tile> circleList = new List<Tile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y+radius;

        for (int i = 0; i <= radius; i++)
        {
            for(int j=0; j<=i;j++)
            {
                if(j==0)
                {
					circleList.Add (GameManager.instance.GetCurrentRoom().GetTile (new Vector2Int (x, y - i)));
                    
                }
                else
                {
                    
					circleList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x - j, y - i)));
					circleList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x + j, y - i)));
                }
            }
        }

        y = (int)center.pos.y - radius;

        for(int i=0; i<= radius-1;i++)
        {
            for(int j=0; j<=i;j++)
            {
                if(j==0)
                {
					circleList.Add (GameManager.instance.GetCurrentRoom ().GetTile (new Vector2Int (x, y + i)));
                }
                else
                {
					circleList.Add (GameManager.instance.GetCurrentRoom ().GetTile (new Vector2Int (x - j, y + i)));
					circleList.Add (GameManager.instance.GetCurrentRoom ().GetTile (new Vector2Int (x + j, y + i)));
                }
            }
        }

        circleList.Remove(center);

        return circleList;
    }
    
	public static List<Tile> SquareRange(Tile center,int radius)
    {
		List<Tile> squareList = new List<Tile>();
        int x = center.pos.x; int y = center.pos.y;
        for(int i=-radius; i<=radius; i++)
        {
            for(int j= -radius; j<=radius;j++)
            {
				squareList.Add (GameManager.instance.GetCurrentRoom ().GetTile (new Vector2Int (x + i, y + j)));
            }
        }
        squareList.Remove(center);

        return squareList;
    }



    /// <summary>
    /// 플레이어가 원모양으로 주위에 있는가 체크
    /// </summary>
	public static bool AI_CircleFind(Tile center,int radius)
    {
		List<Tile> range = CircleRange(center, radius);

        for (int i = 0; i < range.Count; i++)
        {
            if (range[i].OnTileObj != null)
            {
                if (range[i].OnTileObj is Player)
                {
                    return true;
                }
            }
        }
        return false;
    }
	public static bool AI_SquareFind(Tile center,int radius)
    {
		List<Tile> range = SquareRange(center, radius);
        for(int i=0; i<range.Count;i++)
        {
            if(range[i].OnTileObj != null)
            {
                if(range[i].OnTileObj is Player)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 주어진 타일내에 플레이어가 있는가 체크
    /// </summary>
	public static bool AI_Find(List<Tile> list)
    {
		foreach(Tile v in list)
        {
            if(v.OnTileObj == Player.instance)
            {
                return true;
            }
        }
        return false;
    }
    
}


