using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tileState
{
    DEFAULT,//시야에 보이지않음
    ONRANGE,//시야에 들어옴
    EXPLORED,//탐색한적 있음
}
public abstract class TheTile : MonoBehaviour {
    #region variables
    public List<TheTile> neighbours;
    SpriteRenderer sprite;
    public int x, y;
    public bool isStandAble;
    public Room thisRoom; 

    /*private offTileObject _offTile;//타일위에 올라가지 않는 오브젝트 ex)덫
     public virtual offTileObject offTile
     {
         get
         {
             return _offTile;
         }
         set
         {
             _offTile = value;
         }
     }*/

    protected tileState _thisState;
    public tileState thisState
    {
        get
        {
            return _thisState;
        }
        set
        {
            _thisState = value;
        }
  }
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
            OnTileChanged(_onTile);
        }
    }
    #endregion


    public virtual void MapUpdate(int _x,int _y)
    {
        sprite = GetComponent<SpriteRenderer>();
        x = _x;
        y = _y;

        name = "Tile_" + x + "+" + y;
        transform.position = new Vector2(_x,_y);
    }
    protected virtual void OnTileChanged(OnTileObject OT)
    {

    }

}
