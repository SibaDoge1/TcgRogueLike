using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public static Player instance;
    
    private void Awake()
    {
        instance = this;  
    }

    public override void MoveTo(Vector2Int _pos)
    {
        base.MoveTo(_pos);
        //todo: TurnEnd
    }
    //스폰되듯이 이동
    public void Spawn(Room _room)
    {
        StartCoroutine(CameraFollow.instance.RoomTrace(_room));
        Player.instance.SetRoom(_room);
        Player.instance.MoveTo(new Vector2Int(4, 4));
    }
    //문을 통해서 이동
    public void EnterRoom(Room _room)
    {
        Vector2Int temp = currentRoom.Pos - _room.Pos;
        
        StartCoroutine(CameraFollow.instance.RoomTrace(_room));
        SetRoom(_room);
    }
}
