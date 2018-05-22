using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TheTile :MonoBehaviour
{
    #region variables
    public List<TheTile> neighbours;
    SpriteRenderer sprite;
    public Vector2Int pos;
    public OnTileObject _onTile;//타일위에 있는 mapObject
    public OnTileObject onTile
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
    private OffTile _OffTile;
    public OffTile TileInfo
    {
        set
        {
            if (_OffTile != null)
                Destroy(_OffTile.gameObject);
            _OffTile = value;
            _OffTile.setTile(this);
        }
    }
    #endregion
    /// <summary>
    /// Defalut : GroundTile
    /// </summary>
    public void SetTile(Vector2Int _pos)
    {
        pos = _pos;
        transform.localPosition = MapGenerator.instance.GetTilePosition(_pos);

        name = "Tile" + _pos;
    }

    public bool IsStandAble(OnTileObject ot)
    {
        if(_OffTile !=null)
        {
           return _OffTile.IsStandAble(ot);
        }
        else
        {
            if (onTile)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public void SomethingUpOnThis(OnTileObject ot)
    {
        if (_OffTile != null)
        {
            _OffTile.SomethingUpOnThis(ot);
        }
    }
}
