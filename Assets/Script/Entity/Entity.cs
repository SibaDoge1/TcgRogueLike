using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public abstract class Entity : MonoBehaviour
{
    public Room currentRoom;
	public Tile currentTile;
    public Vector2Int pos;

    public short entityNum;

    public virtual void Init(short _entityNum)
    {
        entityNum = _entityNum;
        gameObject.name = "" + entityNum;
    }

    protected SpriteRenderer spriteRender;
	protected virtual void  Awake ()
    {
        SetSpriteRender();
    }

    protected virtual void SetSpriteRender()
    {
        spriteRender = GetComponent<SpriteRenderer>();
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
        transform.localPosition = currentTile.transform.localPosition - (Vector3.back*_pos.y);
        return true;
	}

    Coroutine moveAnim;
    public virtual bool MoveTo(Vector2Int _pos)
    {
        
        if (currentRoom.GetTile(_pos) == null ||!currentRoom.GetTile(_pos).IsStandAble(this))
        {
			return false;
        }

        if (currentRoom.GetTile(pos).OnTileObj == this)
            currentRoom.GetTile(pos).OnTileObj = null;

        if (moveAnim != null)
        {
            StopCoroutine(moveAnim);
        }

        if (spriteRender == null)
            spriteRender = GetComponent<SpriteRenderer>();
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
        Vector3 targetPos = currentRoom.GetTile(target).transform.localPosition - (Vector3.back * target.y);
        while (true)
        {
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



    public void DestroyThis()
    {
        OnDieCallback();
    }
}
