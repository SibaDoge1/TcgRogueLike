using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Node
{
    #region variables
    public List<Node> neighbours;
    SpriteRenderer sprite;
    public Vector2Int pos;

    private OnTileObject _onTile;//타일위에 있는 mapObject

    public virtual OnTileObject onTile
    {
        get
        {
            return _onTile;
        }
        set
        {
            _onTile = value;
            SomethingUpOnThis(_onTile);
        }
    }
    #endregion

    public Node(Vector2Int _pos)
    {
        pos = _pos;
    }

    public abstract void SomethingUpOnThis(OnTileObject ot);


    public virtual bool IsStandAble(OnTileObject me)
    {
        if(onTile)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
