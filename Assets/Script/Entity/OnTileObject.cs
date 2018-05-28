using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public abstract class OnTileObject : MonoBehaviour {

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
			if (value >= fullHp) {
				EffectDelegate.instance.MadeEffect (fullHp - _currentHp, transform.position); 
				_currentHp = fullHp;
			} else {
				EffectDelegate.instance.MadeEffect (value - _currentHp, transform.position); 
				_currentHp = value;
			}

			if (_currentHp <= 0) {
				EffectDelegate.instance.MadeEffect (CardEffectType.Blood, transform.position);
				OnDieCallback ();
			}
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

    #region MoveMethod
	public virtual bool Teleport(Vector2Int _pos){
		if (!currentRoom.GetTile(_pos).IsStandAble(this))
		{
			return false;
		}

		if (currentRoom.GetTile(pos).OnTileObj == this)
			currentRoom.GetTile(pos).OnTileObj = null;

		pos = _pos;
		currentTile = currentRoom.GetTile(pos);
		currentTile.OnTileObj = this;
		transform.localPosition = currentTile.transform.localPosition;

		return true;
	}
	public virtual bool MoveTo(Vector2Int _pos)
    {
        if (!currentRoom.GetTile(_pos).IsStandAble(this))
        {
			return false;
        }

        if (currentRoom.GetTile(pos).OnTileObj == this)
            currentRoom.GetTile(pos).OnTileObj = null;

        pos = _pos;
        currentTile = currentRoom.GetTile(pos);
        currentTile.OnTileObj = this;
		StartCoroutine (MoveAnimationRoutine ());

		return true;
    }

	private IEnumerator MoveAnimationRoutine(){
		float timer = 0;
		Vector3 originPos = transform.localPosition;
		while (true) {
			timer += Time.deltaTime * 5;
			if (timer > 1) {
				break;
			}
			transform.localPosition = Vector3.Lerp (originPos, currentTile.transform.localPosition, timer);
			yield return null;
		}
		transform.localPosition = currentTile.transform.localPosition;
		OnEndTurn ();
	}

    protected virtual void OnDieCallback()
    {
        sprite.enabled = false;
        currentTile.OnTileObj = null;
        gameObject.SetActive(false);
    }
    #endregion

	protected virtual void OnEndTurn(){
		
	}
   /* protected virtual bool AttackThis(int dam,CardAttribute atr)
    {
        if(!isHitable)
        {
            return false;
        }



        return true;
    }*/
}
