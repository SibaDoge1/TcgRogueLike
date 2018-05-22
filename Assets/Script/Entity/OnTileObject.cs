using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnTileObject : MonoBehaviour {

    public Room currentRoom;
    public TheTile currentTile;
    public Vector2Int pos;

    protected int _fullHp=1;
    protected int _currentHp=1;
    
    public virtual int fullHp
    {
        get
        {
            return _fullHp;
        }
        set
        {
            _fullHp = value;
        }
    }
    public virtual int currentHp
    {
        get
        {
            return _currentHp;
        }
        set
        {
            if(value>=fullHp)
            {
                _currentHp = fullHp;
            }
            else
            {
                _currentHp = value;
            }

            if(_currentHp<=0)
            {
                DestroyThis();
            }
        }
    }

    // Use this for initialization
    SpriteRenderer sprite;
	void Awake () {
        sprite = GetComponent<SpriteRenderer>();
	}
	
	public virtual void SetRoom(Room room,Vector2Int _pos)
    {
        currentRoom = room;
        this.transform.parent = room.transform;
        this.MoveTo(_pos);
    }

    #region MoveMethod
    public virtual void MoveTo(Vector2Int _pos)
    {
        if (!currentRoom.GetTile(_pos).IsStandAble(this))
        {
            return;
        }

        if (currentRoom.GetTile(pos).onTile == this)
            currentRoom.GetTile(pos).onTile = null;

        pos = _pos;
        currentTile = currentRoom.GetTile(pos);
        currentTile.onTile = this;
        transform.localPosition = currentTile.transform.localPosition;
    }

    public virtual void DestroyThis()
    {
        sprite.enabled = false;
        currentTile.onTile = null;
        gameObject.SetActive(false);
    }
    public virtual void MoveUp()
    {
        MoveTo(pos + new Vector2Int(0, 1));
    }
    public virtual void MoveRight()
    {
        MoveTo(pos + new Vector2Int(1, 0));
    }
    public virtual void MoveLeft()
    {
        MoveTo(pos + new Vector2Int(-1,0));
    }
    public virtual void MoveDown()
    {
        MoveTo(pos + new Vector2Int(0, -1));
    }
    #endregion
}
