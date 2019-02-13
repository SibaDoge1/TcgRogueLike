using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public enum Figure
{
    NONE,
    Diagonal,
    CROSS,
    HORIZION,
    VERTICAL,
    SQUARE,
    EMPTYSQUARE,
    CIRCLE,
}

/// <summary>
/// Player의 스킬 함수 작성시 도움되는 함수들 입니다.
/// </summary>
public static class TileUtils
{

    public static int CalcRange(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x)+ Mathf.Abs(a.y - b.y);
    }
    /// <summary>
    /// 가운데가 비어있는 사각형 범위의 타일을 가져옵니다.
    /// </summary>
    public static List<Tile> EmptySquareRange(Tile center, int radius)
    {
        List<Tile> tiles = TileUtils.SquareRange(center, radius);
        for(int i=1; i<radius;i++)
        {
            List<Tile> removeTiles = TileUtils.SquareRange(center, radius-i);
            for (int j = 0; j < removeTiles.Count; j++)
            {
                tiles.Remove(removeTiles[j]);
            }
        }

        return tiles;
    }
    /// <summary>
    /// X범위로 가져옴
    /// </summary>
    public static List<Tile> DiagonalRange(Tile center,int radius)
    {
        List<Tile> crossList = new List<Tile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y;

        for (int i = 1; i <= radius; i++)
        {
            crossList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+i, y + i)));
            crossList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-i, y - i)));
            crossList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + i, y-i)));
            crossList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - i, y+i)));
        }
        for (int i = crossList.Count - 1; i >= 0; i--)
        {
            if (crossList[i] == null)
            {
                crossList.RemoveAt(i);
            }
        }
        return crossList;
    }
    /// <summary>
    /// 가로범위 카드들 가져오기
    /// </summary>
    public static List<Tile> HorizonRange(Tile center, int radius)
    {
        List<Tile> horrizonList = new List<Tile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y;

        horrizonList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x -1 , y + radius)));
        horrizonList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + radius)));
        horrizonList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y + radius)));
        horrizonList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y - radius)));
        horrizonList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y - radius)));
        horrizonList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y - radius)));

        for (int i = horrizonList.Count - 1; i >= 0; i--)
        {
            if (horrizonList[i] == null)
            {
                horrizonList.RemoveAt(i);
            }
        }
        return horrizonList;
    }

    /// <summary>
    /// 세로범위 카드들 가져오기
    /// </summary>
    public static List<Tile> VerticalRange(Tile center, int radius)
    {
        List<Tile> verticalList = new List<Tile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y;

        verticalList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+radius, y+1)));
        verticalList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + radius, y)));
        verticalList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + radius, y-1)));
        verticalList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - radius, y+1)));
        verticalList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - radius, y)));
        verticalList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - radius, y-1)));

        for (int i = verticalList.Count - 1; i >= 0; i--)
        {
            if (verticalList[i] == null)
            {
                verticalList.RemoveAt(i);
            }
        }
        return verticalList;
    }

    /// <summary>
    /// 십자로 타일 가져오기
    /// </summary>
    public static List<Tile> CrossRange(Tile center, int radius)
    {
        List<Tile> crossList = new List<Tile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y;

        for (int i = 1; i <= radius; i++)
        {
			crossList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + i)));
            crossList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y - i)));
            crossList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + i, y)));
            crossList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - i, y)));
        }
		for (int i = crossList.Count - 1; i >= 0; i--) {
			if (crossList [i] == null) {
				crossList.RemoveAt (i);
			}
		}
        return crossList;
    }
    /// <summary>
    /// 원모양으로 타일 가져오기
    /// </summary>
	public static List<Tile> CircleRange(Tile center, int radius)
    {
        List<Tile> circleList = new List<Tile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y + radius;

        for (int i = 0; i <= radius; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                if (j == 0)
                {
                    circleList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y - i)));

                }
                else
                {

                    circleList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - j, y - i)));
                    circleList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + j, y - i)));
                }
            }
        }

        y = (int)center.pos.y - radius;

        for (int i = 0; i <= radius - 1; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                if (j == 0)
                {
                    circleList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + i)));
                }
                else
                {
                    circleList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - j, y + i)));
                    circleList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + j, y + i)));
                }
            }
        }

        circleList.Remove(center);
        for (int i = circleList.Count - 1; i >= 0; i--)
        {
            if (circleList[i] == null)
            {
                circleList.RemoveAt(i);
            }
        }

        return circleList;
    }
    /// <summary>
    /// 꽉찬 네모모양으로 타일 가져오기
    /// </summary>
    public static List<Tile> SquareRange(Tile center, int radius)
    {
        List<Tile> squareList = new List<Tile>();
        int x = center.pos.x; int y = center.pos.y;
        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                squareList.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + i, y + j)));
            }
        }
        squareList.Remove(center);
		for (int i = squareList.Count - 1; i >= 0; i--) {
			if (squareList [i] == null) {
				squareList.RemoveAt (i);
			}
		}

        return squareList;
    }
    public static List<Tile> Range(Tile center,int radius,Figure figure)
    {
        List<Tile> range;
        switch (figure)
        {
            case Figure.Diagonal:
                range = DiagonalRange(center, radius);
                break;
            case Figure.CROSS:
                range = CrossRange(center, radius);
                break;
            case Figure.CIRCLE:
                range = CircleRange(center, radius);
                break;
            case Figure.SQUARE:
                range = SquareRange(center, radius);
                break;
            case Figure.EMPTYSQUARE:
                range = EmptySquareRange(center, radius);
                break;
            case Figure.HORIZION:
                range = HorizonRange(center, radius);
                break;
            case Figure.VERTICAL:
                range = VerticalRange(center, radius);
                break;
            default:
                range = null;
                break;
        }
        return range;
    }



    #region PlayerSkill
    /// <summary>
    /// 근처의 적 리스트 가져옵니다.
    /// </summary>
    public static List<Enemy> GetEnemies(Tile center,int radius,Figure figure)
    {
        List<Enemy> targets = new List<Enemy>();
        List<Tile> range;
        switch (figure)
        {
            case Figure.Diagonal:
                range = DiagonalRange(center, radius);
                break;
            case Figure.CROSS:
                range = CrossRange(center, radius);
                break;
            case Figure.CIRCLE:
                range = CircleRange(center, radius);
                break;
            case Figure.SQUARE:
                range = SquareRange(center, radius);
                break;
            case Figure.EMPTYSQUARE:
                range = EmptySquareRange(center, radius);
                break;
            case Figure.HORIZION:
                range = HorizonRange(center, radius);
                break;
            case Figure.VERTICAL:
                range = VerticalRange(center, radius);
                break;
            default:
                range = new List<Tile>();
                break;
        }
        for (int i=0; i<range.Count;i++)
        {
            if(range[i].OnTileObj && range[i].OnTileObj is Enemy)
            {
                targets.Add(range[i].OnTileObj as Enemy);
            }
        }
        return targets;
    }

    /// <summary>
    /// 타일리스트에 있는 적들을 가져옴
    /// </summary>
    public static List<Enemy> GetEnemies(List<Tile> range)
    {
        List<Enemy> targets = new List<Enemy>();

        for (int i = 0; i < range.Count; i++)
        {
            if (range[i].OnTileObj && range[i].OnTileObj is Enemy)
            {
                targets.Add(range[i].OnTileObj as Enemy);
            }
        }
        return targets;
    }

    /// <summary>
    /// 타일리스트의 적들중 num수만큼만 가져옴
    /// </summary>
    public static List<Enemy> GetEnemies(List<Tile> range,int num)
    {
        List<Enemy> enemies = new List<Enemy>();
        List<Enemy> targets = new List<Enemy>();

        for (int i = 0; i < range.Count; i++)
        {
            if (range[i].OnTileObj && range[i].OnTileObj is Enemy)
            {
                enemies.Add(range[i].OnTileObj as Enemy);
            }
        }
        if(enemies.Count>num)
        {
            for (int i = 0; i < num; i++)
            {
                int ran = Random.Range(i, enemies.Count);
                Enemy t = enemies[i];
                enemies[i] = enemies[ran];
                enemies[ran] = t;
                targets.Add(enemies[i]);
            }
        }else
        {
            targets = enemies;
        }

        return targets;
    }

    /// <summary>
    /// 오토타겟하여 주변의 적을 가져옴
    /// </summary>
    public static Enemy AutoTarget(Tile center, int radius,Figure figure)
    {
        List<Enemy> targets = new List<Enemy>();
        List<Tile> range;
        switch (figure)
        {
            case Figure.Diagonal:
                range = DiagonalRange(center, radius);
                break;
            case Figure.CROSS:
                range = CrossRange(center, radius);
                break;
            case Figure.CIRCLE:
                range = CircleRange(center, radius);
                break;
            case Figure.SQUARE:
                range = SquareRange(center, radius);
                break;
            case Figure.EMPTYSQUARE:
                range = EmptySquareRange(center, radius);
                break;
            case Figure.HORIZION:
                range = HorizonRange(center, radius);
                break;
            case Figure.VERTICAL:
                range = VerticalRange(center, radius);
                break;
            default:
                range = new List<Tile>();
                break;
        }
        for (int i = 0; i < range.Count; i++)
        {
            if (range[i].OnTileObj && range[i].OnTileObj is Enemy)
            {
                targets.Add(range[i].OnTileObj as Enemy);
            }
        }
		if (targets.Count >0)
			return targets [Random.Range (0, targets.Count)];
		else
			return null;
    }

	public static bool IsEnemyInRange(Tile center, int radius,Figure figure)
	{
        List<Tile> range;
        switch (figure)
        {
            case Figure.Diagonal:
                range = DiagonalRange(center, radius);
                break;
            case Figure.CROSS:
                range = CrossRange(center, radius);
                break;
            case Figure.CIRCLE:
                range = CircleRange(center, radius);
                break;
            case Figure.SQUARE:
                range = SquareRange(center, radius);
                break;
            case Figure.EMPTYSQUARE:
                range = EmptySquareRange(center, radius);
                break;
            case Figure.HORIZION:
                range = HorizonRange(center, radius);
                break;
            case Figure.VERTICAL:
                range = VerticalRange(center, radius);
                break;
            default:
                range = new List<Tile>();
                break;
        }
        for (int i = 0; i < range.Count; i++)
		{
			if (range[i].OnTileObj && range[i].OnTileObj is Enemy)
			{
				return true;
			}
		}
		return false;
	}
    public static bool IsEnemyInRange(List<Tile> t)
    {
        for(int i=0; i<t.Count;i++)
        {
            if(t[i].OnTileObj && t[i].OnTileObj is Enemy)
            {
                return true;
            }
        }
        return false;
    }

    #endregion
    /// <summary>
    /// 2타일의 위치관계를 통해, 동,서,남,북 방향을 구해서 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public static Vector2Int GetDir(Tile center,Tile target)
    {        
        Vector2Int v = target.pos - center.pos;
        if(Mathf.Abs(v.x)>Mathf.Abs(v.y))
        {
            return new Vector2Int(v.x /Mathf.Abs(v.x), 0);
        }else
        {
            return new Vector2Int(0,v.y/Mathf.Abs(v.y));
        }
    }

    #region AI Utils
    public static bool AI_Find(List<Tile> tiles)
    {
        for(int i=0; i<tiles.Count;i++)
        {
            if (tiles[i].OnTileObj != null)
            {
                if (tiles[i].OnTileObj is Player)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 플레이어가 원모양으로 주위에 있는가 체크
    /// </summary>
    public static bool AI_CircleFind(Tile center, int radius)
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
    public static bool AI_SquareFind(Tile center, int radius)
    {
        List<Tile> range = SquareRange(center, radius);
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
    public static bool AI_CrossFind(Tile center, int radius)
    {
        List<Tile> range = CrossRange(center, radius);
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
    public static bool AI_DiagonalFind(Tile center, int radius)
    {
        List<Tile> range = DiagonalRange(center, radius);
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
    #endregion
}


