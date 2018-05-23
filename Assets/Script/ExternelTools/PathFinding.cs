using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;


public class PathFinding : MonoBehaviour
{
    public static PathFinding instance;
    private void Awake()
    {
        instance = this;
    }



	Tile _targetTile;

	float CostToEnterTile(Tile source, Tile target)
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
        if (temp == Vector2.right || temp == Vector2.left)
        {
            return 1 - 0.000001f;
        }
        else
        {
            return 1f;
        }

    }




    /// <summary>
    /// calculate Distance
    /// </summary>
     float calculateVector(Vector2 a, Vector2 b)
    {
        return Mathf.Abs((a - b).x) + Mathf.Abs((a - b).y);
    }

    /// <summary>
    /// pathFinding Using A*
    /// </summary>
	public void GeneratePathTo(OnTileObject obje,Tile targetTile)
    {
        OnTileObject selectedUnit = obje;
		Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
		Dictionary<Tile, float> distance = new Dictionary<Tile, float>();
        _targetTile = targetTile;
		Tile target;

        // Setup the "Q" -- the list of nodes we haven't checked yet.

		Tile source = selectedUnit.currentTile;

        target = _targetTile;

        //source.distance = 0;
        prev[source] = null;
        prev[target] = null;
		List<Tile> openList = new List<Tile>();
		List<Tile> closedList = new List<Tile>();

        openList.Add(source);

		foreach(Tile v in obje.currentRoom.GetTileArrays())
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



			foreach (Tile v in current.neighbours)
            {

                if (closedList.Contains(v) || CostToEnterTile(current,v) == Mathf.Infinity)
                    continue;

                    float alt = distance[current] + CostToEnterTile(current, v) + calculateVector(v.pos, target.pos);
                if (alt < distance[v]|| !openList.Contains(v))
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
        PathBlocked(obje,targetTile);

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



            if (currentPath == null)
                return;
            if (currentPath[1].OnTileObj != null)
            {
                return;
            }

            selectedUnit.MoveTo(new Vector2Int(currentPath[1].pos.x, currentPath[1].pos.y));
            currentPath.RemoveAt(0);

            if (currentPath.Count == 1)
            {
                currentPath = null;
            }


    }

   





    #region AI Path Block
	float costToEnterTileForAPB(Tile source, Tile target)
    {
        if (target.OnTileObj is Structure)
        {
           return Mathf.Infinity;
        }

        Vector2 temp = source.pos - target.pos;
        if (temp == Vector2.right || temp == Vector2.left)
        {
            return 1;
        }
        else
        {
            return 1 - 0.000001f;
        }
    }

    /// <summary>
    /// 길 막혔어도 가까이라도 가게 해주는 함수입니다. 
    /// </summary>
	public void PathBlocked(OnTileObject obje, Tile targetTile)
    {
        OnTileObject selectedUnit = obje;
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



			foreach (Tile v in current.neighbours)
            {
                if (v == null)
                    continue;
                if (closedList.Contains(v) || costToEnterTileForAPB(current, v) == Mathf.Infinity)
                    continue;
                //기본 pathFinding과 다른부분!!
                float alt = distance[current] + costToEnterTileForAPB(current, v) + calculateVector(v.pos, target.pos);
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
            return;
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

  
            if (currentPath == null)
                return;
            if (currentPath[1].OnTileObj != null)
                return;

            selectedUnit.MoveTo(currentPath[1].pos);
            currentPath.RemoveAt(0);


            if (currentPath.Count == 1)
            {
                currentPath = null;
            }
        }

    }


    #endregion
