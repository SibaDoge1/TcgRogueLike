using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public abstract class Entity : MonoBehaviour {

    public Room currentRoom;
	public Tile currentTile;
    public Vector2Int pos;



    protected SpriteRenderer sprite;
	protected virtual void  Awake () {
        sprite = GetComponent<SpriteRenderer>();
        
	}
	
	public virtual void SetRoom(Room room,Vector2Int _pos)
    {		
        currentRoom = room;
        this.transform.parent = room.transform;
        this.Teleport(_pos);
    }
    public virtual void SetRoom(Room room, Tile _pos)
    {
        currentRoom = room;
        this.transform.parent = room.transform;
        this.Teleport(_pos.pos);
    }
    #region MoveMethod
    public virtual bool Teleport(Vector2Int _pos){
		if (!currentRoom.GetTile(_pos).IsStandAble(this))
		{
			return false;
		}

		if (currentRoom.GetTile(pos) && currentRoom.GetTile(pos).OnTileObj == this)
			currentRoom.GetTile(pos).OnTileObj = null;

        if (moveAnim != null)
        {
            StopCoroutine(moveAnim);
        }
        pos = _pos;
		currentTile = currentRoom.GetTile(pos);
        currentTile.OnTileObj = this;
        currentTile.SomethingUpOnThis(this);
        transform.localPosition = currentTile.transform.localPosition;
        return true;
	}

    Coroutine moveAnim;
    public virtual bool MoveTo(Vector2Int _pos)
    {
        if (!currentRoom.GetTile(_pos).IsStandAble(this))
        {
			return false;
        }

        if (currentRoom.GetTile(pos).OnTileObj == this)
            currentRoom.GetTile(pos).OnTileObj = null;

        if (moveAnim != null)
        {
            StopCoroutine(moveAnim);
        }
        moveAnim = StartCoroutine(MoveAnimationRoutine(pos,_pos));

        pos = _pos;

        currentTile = currentRoom.GetTile(pos);
        currentTile.OnTileObj = this;
        return true;
    }

    public static float moveSpeed = 6;
    protected virtual IEnumerator MoveAnimationRoutine(Vector2Int origin,Vector2Int target){
		float timer = 0;
        Vector3 originPos = currentRoom.GetTile(origin).transform.localPosition;
        Vector3 targetPos = currentRoom.GetTile(target).transform.localPosition;
		while (true) {
			timer += Time.deltaTime * moveSpeed;
			if (timer > 1) {
				break;
			}
			transform.localPosition = Vector3.Lerp (originPos, targetPos, timer);
			yield return null;
		}
        transform.localPosition = targetPos;
	}

    protected virtual void OnDieCallback()
    {
        currentTile.OnTileObj = null;
        gameObject.SetActive(false);
    }
    #endregion



    public void Destroy()
    {
        OnDieCallback();
    }
}
