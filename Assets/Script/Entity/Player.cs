using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private void Start()
    {
        _fullHp = SettingHp; _currentHp = SettingHp;
        UIManager.instance.HpUpdate(currentHp, fullHp);
    }
    //문을 통해서 이동
    public void EnterRoom(Door door)
    {
        Vector2Int temp;
        bool isFlipped=false;

        if (currentRoom != null)
           currentTile.OnTileObj = null;

		//Spawn Position Set
        if (door.Dir == Direction.NORTH)
        {
            temp = door.ConnectedDoor.ThisTile.pos + new Vector2Int(0,1);
        }
        else if (door.Dir == Direction.EAST)
        {
            temp = door.ConnectedDoor.ThisTile.pos + new Vector2Int(1, 0);
            //isFlipped = true;
        }
        else if (door.Dir == Direction.WEST)
        {
            temp = door.ConnectedDoor.ThisTile.pos + new Vector2Int(-1, 0);
            //isFlipped = true;
        }
        else//_room == currentRoom.SouthRoom
        {
            temp = door.ConnectedDoor.ThisTile.pos + new Vector2Int(0,-1);
        }

        SetRoom(door.TargetRoom,temp);

        if (isFlipped)
        {
            SetLocalScale((int)-transform.localScale.x);
        }

		GameManager.instance.SetCurrentRoom (door.TargetRoom);
		GameManager.instance.OnPlayerEnterRoom ();
    }
	protected override void OnDieCallback()
    {
        base.OnDieCallback();
        UIManager.instance.GameOver();
    }
    public override int fullHp
    {
        get
        {
            return base.fullHp;
        }

        set
        {
            base.fullHp = value;
            UIManager.instance.HpUpdate(currentHp, fullHp);
        }
    }

    public override int currentHp
    {
        set { base.currentHp = value;
            UIManager.instance.HpUpdate(currentHp, fullHp); }
    }
    public override bool MoveTo(Vector2Int _pos)
    {
        return base.MoveTo(_pos);
    }

    protected override void OnEndTurn ()
    {
		PlayerControl.instance.EndPlayerTurn ();
	}
}
