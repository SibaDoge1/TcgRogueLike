using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PathFinding : MonoBehaviour
{
    public static PathFinding instance;
    private void Awake()
    {
        instance = this;
    }



    TheTile _targetTile;

     float CostToEnterTile(TheTile source, TheTile target)
    {
        if (target.onTile != null)
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
     public void GeneratePathTo(OnTileObject obje,TheTile targetTile)
    {
        OnTileObject selectedUnit = obje;
        Dictionary<TheTile, TheTile> prev = new Dictionary<TheTile, TheTile>();
        Dictionary<TheTile, float> distance = new Dictionary<TheTile, float>();
        _targetTile = targetTile;
        TheTile target;

        // Setup the "Q" -- the list of nodes we haven't checked yet.

        TheTile source = selectedUnit.currentTile;

        target = _targetTile;

        //source.distance = 0;
        prev[source] = null;
        prev[target] = null;
        List<TheTile> openList = new List<TheTile>();
        List<TheTile> closedList = new List<TheTile>();

        openList.Add(source);

        foreach(TheTile v in obje.currentRoom.GetTileArrays())
        {
            distance[v] = Mathf.Infinity;
        }
        distance[source] = 0;

        while (openList.Count > 0)
        {
            TheTile current = openList[0];
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



            foreach (TheTile v in current.neighbours)
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
         

       
        List<TheTile> currentPath = new List<TheTile>();
        TheTile now = target;
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
            if (currentPath[1].onTile != null)
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
     float costToEnterTileForAPB(TheTile source, TheTile target)
    {
        if (target.onTile is Structure)
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
    public void PathBlocked(OnTileObject obje, TheTile targetTile)
    {
        OnTileObject selectedUnit = obje;
        Dictionary<TheTile, TheTile> prev = new Dictionary<TheTile, TheTile>();
        Dictionary<TheTile, float> distance = new Dictionary<TheTile, float>();
        _targetTile = targetTile;
        TheTile target;

        TheTile source = selectedUnit.currentTile;

        target = _targetTile;

        prev[source] = null;
        prev[target] = null;
        List<TheTile> openList = new List<TheTile>();
        List<TheTile> closedList = new List<TheTile>();

        openList.Add(source);

        while (openList.Count > 0)
        {
            TheTile current = openList[0];
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



            foreach (TheTile v in current.neighbours)
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



        List<TheTile> currentPath = new List<TheTile>();
        TheTile now = target;
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
            if (currentPath[1].onTile != null)
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
