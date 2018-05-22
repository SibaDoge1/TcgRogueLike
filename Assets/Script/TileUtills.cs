using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// Player의 스킬 함수 작성시 도움되는 함수들 입니다.
/// </summary>
public static class TileUtils
{
  
    /// <summary>
    /// 십자로 타일 가져오기
    /// </summary>
    public static List<TheTile> CrossRange(TheTile center, int radius)
    {
        List<TheTile> crossList = new List<TheTile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y;

        for(int i=1; i<=radius; i++)
        {
            crossList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x,y+1)));
            crossList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x, y - 1)));
            crossList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x +1, y)));
            crossList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x -1, y)));
        }
        return crossList;
    }
    /// <summary>
    /// 원모양으로 타일 가져오기
    /// </summary>
    public static List<TheTile> CircleRange(TheTile center,int radius)
    {
        List<TheTile> circleList = new List<TheTile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y+radius;

        for (int i = 0; i <= radius; i++)
        {
            for(int j=0; j<=i;j++)
            {
                if(j==0)
                {
                    circleList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x, y - i)));
                    
                }
                else
                {
                    
                    circleList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x - j, y - i)));
                    circleList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x + j, y - i)));
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
                    circleList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x, y + i)));
                }
                else
                {
                    circleList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x - j, y + i)));
                    circleList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x + j, y + i)));
                }
            }
        }

        circleList.Remove(center);

        return circleList;
    }
    
    public static List<TheTile> SquareRange(TheTile center,int radius)
    {
        List<TheTile> squareList = new List<TheTile>();
        int x = center.pos.x; int y = center.pos.y;
        for(int i=-radius; i<=radius; i++)
        {
            for(int j= -radius; j<=radius;j++)
            {
                squareList.Add(TurnManager.instance.currentRoom.GetTile(new Vector2Int(x +i, y+j)));
            }
        }
        squareList.Remove(center);

        return squareList;
    }



    /// <summary>
    /// 플레이어가 원모양으로 주위에 있는가 체크
    /// </summary>
    public static bool AI_CircleFind(TheTile center,int radius)
    {
        List<TheTile> range = CircleRange(center, radius);

        for (int i = 0; i < range.Count; i++)
        {
            if (range[i].onTile != null)
            {
                if (range[i].onTile is Player)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static bool AI_SquareFind(TheTile center,int radius)
    {
        List<TheTile> range = SquareRange(center, radius);
        for(int i=0; i<range.Count;i++)
        {
            if(range[i].onTile != null)
            {
                if(range[i].onTile is Player)
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
    public static bool AI_Find(List<TheTile> list)
    {
        foreach(TheTile v in list)
        {
            if(v.onTile == Player.instance)
            {
                return true;
            }
        }
        return false;
    }
    
}


