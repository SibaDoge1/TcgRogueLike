using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private void Start()
    {
        fullHp = 10; currentHp = 10;
    }
	public override bool Teleport(Vector2Int _pos)
	{
		bool returnValue = base.Teleport(_pos);
		currentRoom.SetPlayerTile (currentTile);
		return returnValue;
	}
	public override bool MoveTo(Vector2Int _pos)
    {
        bool returnValue = base.MoveTo(_pos);
		currentRoom.SetPlayerTile (currentTile);
		return returnValue;
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
            temp = door.ConnectedDoor.getTile().pos + Vector2Int.up;
        }
        else if (door.Dir == Direction.EAST)
        {
            temp = door.ConnectedDoor.getTile().pos + Vector2Int.right;
            isFlipped = true;
        }
        else if (door.Dir == Direction.WEST)
        {
            temp = door.ConnectedDoor.getTile().pos + Vector2Int.left;
            isFlipped = true;
        }
        else//_room == currentRoom.SouthRoom
        {
            temp = door.ConnectedDoor.getTile().pos + Vector2Int.down;
        }

        SetRoom(door.TargetRoom,temp);

        if (isFlipped)
        {
            SetLocalScale((int)-transform.localScale.x);
        }

		GameManager.instance.SetCurrentRoom (door.TargetRoom);
		GameManager.instance.OnPlayerEnterNewRoom ();
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
        get
        {
            return base.currentHp;
        }

        set
        {
            base.currentHp = value;
            characterUI.HpUpdate(fullHp, currentHp);
			UIManager.instance.HpUpdate(currentHp, fullHp);
        }
    }


	protected override void OnEndTurn ()
    {
		PlayerControl.instance.EndPlayerTurn ();
	}
}
