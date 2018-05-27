using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private void Start()
    {
        fullHp = 6; currentHp = 6;
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
    public void EnterRoom(Room _room)
    {
        Vector2Int temp;
        bool isFlipped=false;

        if (currentRoom != null)
           currentTile.OnTileObj = null;

		//Spawn Position Set
        if (_room == currentRoom.northRoom)
        {
            temp = new Vector2Int(_room.size.x / 2, 1);
        }
        else if (_room == currentRoom.rightRoom)
        {
            temp = new Vector2Int(1, _room.size.y / 2);
            isFlipped = true;
        }
        else if (_room == currentRoom.leftRoom)
        {
            temp = new Vector2Int(_room.size.x - 2, _room.size.y / 2);
            isFlipped = true;
        }
        else//_room == currentRoom.SouthRoom
        {
            temp = new Vector2Int(_room.size.x / 2, _room.size.y - 2);
        }

        SetRoom(_room,temp);
        CameraFollow.instance.RoomTrace(_room);

        if (isFlipped)
        {
            transform.localScale = new Vector3(-transform.localScale.x,1,1);
        }

		GameManager.instance.SetCurrentRoom (_room);
		GameManager.instance.OnPlayerEnterNewRoom ();
    }
	protected override void OnDieCallback()
    {
        Debug.Log("GameOver!");
        SceneManager.LoadScene(0);
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
            UIManager.instance.HpUpdate();
        }
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
            UIManager.instance.HpUpdate();
        }
    }

	protected override void OnEndTurn (){
		GameManager.instance.OnEndPlayerTurn ();
	}
}
