using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public abstract class Entity : MonoBehaviour {

    public Room currentRoom;
	public Tile currentTile;
    public Vector2Int pos;

    protected int _fullHp=1;
    protected int _currentHp=1;
    protected bool isHitable = true;

    public bool IsHitable
    {
        get
        {
            return isHitable;
        }
    }
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
            HpEffect(value);
            if (value < fullHp)
                _currentHp = value;
            else
                _currentHp = fullHp;
        }
    }

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
        transform.localPosition = currentTile.transform.localPosition + new Vector3(0,0,pos.y);
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

    protected virtual IEnumerator MoveAnimationRoutine(Vector2Int origin,Vector2Int target){
		float timer = 0;
        Vector3 originPos = currentRoom.GetTile(origin).transform.localPosition;
        Vector3 targetPos = currentRoom.GetTile(target).transform.localPosition + new Vector3(0, 0, target.y);
		while (true) {
			timer += Time.deltaTime * 6;
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

	protected virtual void OnEndTurn(){
		
	}


    /// <summary>
    /// 체력이 달았으면 true반환 아니면 false반환
    /// </summary>
    public virtual bool GetDamage(int damage,Entity atker = null)
    {
        currentHp -= damage;
        return true;
    }
    public virtual bool GetDamage(float damage, Entity atker = null)
    {
        return GetDamage((int)damage,atker);
    }


    protected virtual void HpEffect(int value)
    {
        EffectDelegate.instance.MadeEffect(value - currentHp, transform.position);

        if (value <= 0)
        {
            EffectDelegate.instance.MadeEffect(CardEffectType.Blood, transform.position);
            OnDieCallback();
        }
    }
}
