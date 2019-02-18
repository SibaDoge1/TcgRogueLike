using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;


public static class PathFinding 
{

	static Tile _targetTile;
    static bool isVerticalLarge;// 목표와의 세로 방향사이의 거리가 더 큽니까?

    static float CostToEnterTile(Tile source, Tile target)
    {
        if (target.OnTileObj != null)
        {
            if(target == _targetTile)
            {
                return 1;
            }
            else
            {
                return Mathf.Infinity;
            }
        }

        Vector2 temp = source.pos - target.pos;
        if (isVerticalLarge)
        {
            if (Mathf.Abs(temp.y) > Mathf.Abs(temp.x))//가로축 타일
            {
                return 1;
            }
            else
            {
                return 1 - 0.000001f;
            }
        }
        else
        {
            if (Mathf.Abs(temp.y) > Mathf.Abs(temp.x))//가로축 타일
            {
                return 1 - 0.000001f;
            }
            else
            {
                return 1;

            }
        }
    }




    /// <summary>
    /// calculate Distance
    /// </summary>
    static float calculateVector(Vector2 a, Vector2 b)
    {
        return Mathf.Abs((a - b).x) + Mathf.Abs((a - b).y);
    }




    /// <summary>
    /// pathFinding Using A*, 상하좌우 방향
    /// </summary>
    static public List<Tile> GenerateCrossPath(Entity obje,Tile target)
	{

		Entity selectedUnit = obje;
		Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile> ();
		Dictionary<Tile, float> distance = new Dictionary<Tile, float> ();
		_targetTile = target;

        if(Mathf.Abs(target.pos.x - obje.pos.x)>Mathf.Abs(target.pos.y-obje.pos.y))
        {
            isVerticalLarge = true;
        } else
        {
            isVerticalLarge = false;
        }

		Tile source = selectedUnit.currentTile;

		if (target == obje.currentTile) {
			List<Tile> temp = new List<Tile> ();
			temp.Add (source);
			return temp;
		}
		//source.distance = 0;
		prev [source] = null;
		prev [target] = null;
		List<Tile> openList = new List<Tile> ();
		List<Tile> closedList = new List<Tile> ();

		openList.Add (source);

		foreach (Tile v in obje.currentRoom.GetTileArrays()) {
			distance [v] = Mathf.Infinity;
		}
		distance [source] = 0;

		while (openList.Count > 0) {
			Tile current = openList [0];
			for (int i = 0; i < openList.Count; i++) {
                
				if (distance [openList [i]] < distance [current]) {
					current = openList [i];
				}
			}
			openList.Remove (current);
			closedList.Add (current);

			if (current == target) {
				break;  
			}



			foreach (Tile v in current.crossNeighbours) {

				if (closedList.Contains (v) || CostToEnterTile (current, v) == Mathf.Infinity)
					continue;

				float alt = distance [current] + CostToEnterTile (current, v) + calculateVector (v.pos, target.pos);
				if (alt < distance [v] || !openList.Contains (v)) {
					distance [v] = alt;
					prev [v] = current;
					if (!openList.Contains (v))
						openList.Add (v);
				}               
			}
		}

		///AI가 길찾기 알고리즘 발동시 , 막히는길이 생길경우가 있다, 그 경우 character를 무시하고 경로를 선택한다.
		if (prev [target] == null) {
            return PathBlocked_Cross(obje, target);
		}
         

       
		List<Tile> currentPath = new List<Tile> ();
		Tile now = target;
		// Step through the "prev" chain and add it to our path
		while (now != null) {
			currentPath.Add (now);
			now = prev [now];
		}

		// Right now, currentPath describes a route from out target to our source
		// So we need to invert it!

		currentPath.Reverse ();
		currentPath.Remove (source);
		return currentPath;
	}

    /// <summary>
    /// pathFinding Using A*, 대각선 방향
    /// </summary>
    static public List<Tile> GenerateDiagonalPath(Entity obje, Tile target)
    {

        Entity selectedUnit = obje;
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        Dictionary<Tile, float> distance = new Dictionary<Tile, float>();
        _targetTile = target;

        if (Mathf.Abs(target.pos.x - obje.pos.x) > Mathf.Abs(target.pos.y - obje.pos.y))
        {
            isVerticalLarge = true;
        }
        else
        {
            isVerticalLarge = false;
        }

        Tile source = selectedUnit.currentTile;

        if (target == obje.currentTile)
        {
            List<Tile> temp = new List<Tile>();
            temp.Add(source);
            return temp;
        }
        //source.distance = 0;
        prev[source] = null;
        prev[target] = null;
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(source);

        foreach (Tile v in obje.currentRoom.GetTileArrays())
        {
            distance[v] = Mathf.Infinity;
        }
        distance[source] = 0;

        while (openList.Count > 0)
        {
            Tile current = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {

                if (distance[openList[i]] < distance[current])
                {
                    current = openList[i];
                }
            }
            openList.Remove(current);
            closedList.Add(current);

            if (current == target)
            {
                break;
            }



            foreach (Tile v in current.diagonalNeighbours)
            {

                if (closedList.Contains(v) || CostToEnterTile(current, v) == Mathf.Infinity)
                    continue;

                float alt = distance[current] + CostToEnterTile(current, v) + calculateVector(v.pos, target.pos);
                if (alt < distance[v] || !openList.Contains(v))
                {
                    distance[v] = alt;
                    prev[v] = current;
                    if (!openList.Contains(v))
                        openList.Add(v);
                }
            }
        }

        ///AI가 길찾기 알고리즘 발동시 , 막히는길이 생길경우가 있다, 그 경우 character를 무시하고 경로를 선택한다.
        if (prev[target] == null)
        {
            return PathBlocked_Cross(obje, target);
        }



        List<Tile> currentPath = new List<Tile>();
        Tile now = target;
        // Step through the "prev" chain and add it to our path
        while (now != null)
        {
            currentPath.Add(now);
            now = prev[now];
        }

        // Right now, currentPath describes a route from out target to our source
        // So we need to invert it!

        currentPath.Reverse();
        currentPath.Remove(source);
        return currentPath;
    }

    /// <summary>
    /// pathFinding Using A*, 8방향
    /// </summary>
    static public List<Tile> GenerateAllDirectionPath(Entity obje, Tile target)
    {

        Entity selectedUnit = obje;
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        Dictionary<Tile, float> distance = new Dictionary<Tile, float>();
        _targetTile = target;

        if (Mathf.Abs(target.pos.x - obje.pos.x) > Mathf.Abs(target.pos.y - obje.pos.y))
        {
            isVerticalLarge = true;
        }
        else
        {
            isVerticalLarge = false;
        }

        Tile source = selectedUnit.currentTile;

        if (target == obje.currentTile)
        {
            List<Tile> temp = new List<Tile>();
            temp.Add(source);
            return temp;
        }
        //source.distance = 0;
        prev[source] = null;
        prev[target] = null;
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(source);

        foreach (Tile v in obje.currentRoom.GetTileArrays())
        {
            distance[v] = Mathf.Infinity;
        }
        distance[source] = 0;

        while (openList.Count > 0)
        {
            Tile current = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {

                if (distance[openList[i]] < distance[current])
                {
                    current = openList[i];
                }
            }
            openList.Remove(current);
            closedList.Add(current);

            if (current == target)
            {
                break;
            }


            foreach (Tile v in current.allNeighbours)
            {

                if (closedList.Contains(v) || CostToEnterTile(current, v) == Mathf.Infinity)
                    continue;

                float alt = distance[current] + CostToEnterTile(current, v) + calculateVector(v.pos, target.pos);
                if (alt < distance[v] || !openList.Contains(v))
                {
                    distance[v] = alt;
                    prev[v] = current;
                    if (!openList.Contains(v))
                        openList.Add(v);
                }
            }
        }

        ///AI가 길찾기 알고리즘 발동시 , 막히는길이 생길경우가 있다, 그 경우 character를 무시하고 경로를 선택한다.
        if (prev[target] == null)
        {
            return PathBlocked_Cross(obje, target);
        }



        List<Tile> currentPath = new List<Tile>();
        Tile now = target;
        // Step through the "prev" chain and add it to our path
        while (now != null)
        {
            currentPath.Add(now);
            now = prev[now];
        }

        // Right now, currentPath describes a route from out target to our source
        // So we need to invert it!

        currentPath.Reverse();
        currentPath.Remove(source);
        return currentPath;
    }





    #region Path Block
    static float costToEnterTileForPB(Tile source, Tile target)
    {
		if (target.OnTileObj is Structure)
        {
           return Mathf.Infinity;
        }


        Vector2 temp = source.pos - target.pos;
        if (isVerticalLarge)
        {
            if (Mathf.Abs(temp.y) > Mathf.Abs(temp.x))//가로축 타일
            {
                return 1;
            }
            else
            {
                return 1 - 0.000001f;
            }
        }
        else
        {
            if (Mathf.Abs(temp.y) > Mathf.Abs(temp.x))//가로축 타일
            {
                return 1 - 0.000001f;
            }
            else
            {
                return 1;

            }
        }
    }

    /// <summary>
    /// 길 막혔다면 캐릭터 무시하고 계산하는 함수입니다. 
    /// </summary>

    static private List<Tile> PathBlocked_Cross(Entity obje, Tile targetTile)
    {
        Entity selectedUnit = obje;
		Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
		Dictionary<Tile, float> distance = new Dictionary<Tile, float>();
        _targetTile = targetTile;
		Tile target;

		Tile source = selectedUnit.currentTile;

        target = _targetTile;

        prev[source] = null;
        prev[target] = null;
		List<Tile> openList = new List<Tile>();
		List<Tile> closedList = new List<Tile>();

        foreach (Tile v in obje.currentRoom.GetTileArrays())
        {
            distance[v] = Mathf.Infinity;
        }
        distance[source] = 0;

        openList.Add(source);

        while (openList.Count > 0)
        {
			Tile current = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (distance[openList[i]] < distance[current])
                {
                    current = openList[i];
                }
            }
            openList.Remove(current);
            closedList.Add(current);

            if (current == target)
            {
                break;
            }



			foreach (Tile v in current.crossNeighbours)
            {
                if (v == null)
                    continue;
                if (closedList.Contains(v) || costToEnterTileForPB(current, v) == Mathf.Infinity)
                    continue;
                //기본 pathFinding과 다른부분!!
                float alt = distance[current] + costToEnterTileForPB(current, v) + calculateVector(v.pos, target.pos);
                if (alt < distance[v] || !openList.Contains(v))
                {
                    distance[v] = alt;
                    prev[v] = current;
                    if (!openList.Contains(v))
                        openList.Add(v);
                }
            }
        }

        ///진짜 아예 길이없넹..
        if (prev[target] == null)
        {
            List<Tile> temp = new List<Tile>();
            temp.Add(source);
            return temp;
        }



		List<Tile> currentPath = new List<Tile>();
		Tile now = target;
        while (now != null)
        {
            currentPath.Add(now);
            now = prev[now];
        }

        currentPath.Reverse();

        currentPath.Remove(source);
        return currentPath;
        }

    static private List<Tile> PathBlocked_Diagonal(Entity obje, Tile targetTile)
    {
        Entity selectedUnit = obje;
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        Dictionary<Tile, float> distance = new Dictionary<Tile, float>();
        _targetTile = targetTile;
        Tile target;

        Tile source = selectedUnit.currentTile;

        target = _targetTile;

        prev[source] = null;
        prev[target] = null;
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        foreach (Tile v in obje.currentRoom.GetTileArrays())
        {
            distance[v] = Mathf.Infinity;
        }
        distance[source] = 0;

        openList.Add(source);

        while (openList.Count > 0)
        {
            Tile current = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (distance[openList[i]] < distance[current])
                {
                    current = openList[i];
                }
            }
            openList.Remove(current);
            closedList.Add(current);

            if (current == target)
            {
                break;
            }



            foreach (Tile v in current.diagonalNeighbours)
            {
                if (v == null)
                    continue;
                if (closedList.Contains(v) || costToEnterTileForPB(current, v) == Mathf.Infinity)
                    continue;
                //기본 pathFinding과 다른부분!!
                float alt = distance[current] + costToEnterTileForPB(current, v) + calculateVector(v.pos, target.pos);
                if (alt < distance[v] || !openList.Contains(v))
                {
                    distance[v] = alt;
                    prev[v] = current;
                    if (!openList.Contains(v))
                        openList.Add(v);
                }
            }
        }

        ///진짜 아예 길이없넹..
        if (prev[target] == null)
        {
            List<Tile> temp = new List<Tile>();
            temp.Add(source);
            return temp;
        }



        List<Tile> currentPath = new List<Tile>();
        Tile now = target;
        while (now != null)
        {
            currentPath.Add(now);
            now = prev[now];
        }

        currentPath.Reverse();

        currentPath.Remove(source);
        return currentPath;
    }

    static private List<Tile> PathBlocked_All(Entity obje, Tile targetTile)
    {
        Entity selectedUnit = obje;
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        Dictionary<Tile, float> distance = new Dictionary<Tile, float>();
        _targetTile = targetTile;
        Tile target;

        Tile source = selectedUnit.currentTile;

        target = _targetTile;

        prev[source] = null;
        prev[target] = null;
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        foreach (Tile v in obje.currentRoom.GetTileArrays())
        {
            distance[v] = Mathf.Infinity;
        }
        distance[source] = 0;

        openList.Add(source);

        while (openList.Count > 0)
        {
            Tile current = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (distance[openList[i]] < distance[current])
                {
                    current = openList[i];
                }
            }
            openList.Remove(current);
            closedList.Add(current);

            if (current == target)
            {
                break;
            }



            foreach (Tile v in current.allNeighbours)
            {
                if (v == null)
                    continue;
                if (closedList.Contains(v) || costToEnterTileForPB(current, v) == Mathf.Infinity)
                    continue;
                //기본 pathFinding과 다른부분!!
                float alt = distance[current] + costToEnterTileForPB(current, v) + calculateVector(v.pos, target.pos);
                if (alt < distance[v] || !openList.Contains(v))
                {
                    distance[v] = alt;
                    prev[v] = current;
                    if (!openList.Contains(v))
                        openList.Add(v);
                }
            }
        }

        ///진짜 아예 길이없넹..
        if (prev[target] == null)
        {
            List<Tile> temp = new List<Tile>();
            temp.Add(source);
            return temp;
        }



        List<Tile> currentPath = new List<Tile>();
        Tile now = target;
        while (now != null)
        {
            currentPath.Add(now);
            now = prev[now];
        }

        currentPath.Reverse();

        currentPath.Remove(source);
        return currentPath;
    }

    #endregion
}


