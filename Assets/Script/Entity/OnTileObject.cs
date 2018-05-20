using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnTileObject : MonoBehaviour {

    public Room currentRoom;
    public Vector2Int pos;

    public int Hp;
    // Use this for initialization
    Sprite sprite;
	void Start () {
        sprite = GetComponent<Sprite>();
	}
	
	public virtual void SetRoom(Room room)
    {
        currentRoom = room;
        this.transform.parent = room.transform;
    }

    #region MoveMethod
    public virtual void MoveTo(Vector2Int _pos)
    {
        if (currentRoom.thisNodes[_pos.x,_pos.y].IsStandAble(this))
        {
            return;
        }

        if (currentRoom.thisNodes[pos.x, pos.y].onTile == this)
            currentRoom.thisNodes[pos.x, pos.y].onTile = null;

        pos = _pos;
        currentRoom.thisNodes[pos.x, pos.y].onTile = this;
        transform.localPosition = new Vector3(-8.5f + pos.x, -4.5f + pos.y, 0);
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
