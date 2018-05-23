using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public static Player instance;
    
    private void Awake()
    {
        instance = this;  
    }
    private void Start()
    {
        fullHp = 6; currentHp = 6;
    }
    public override void MoveTo(Vector2Int _pos)
    {
        base.MoveTo(_pos);
    }
    //스폰되듯이 이동
    public void SpawnToRoom(Room _room)
    {
        if (currentRoom!=null)
        currentTile.OnTileObj = null;


        CameraFollow.instance.RoomTrace(_room);
        SetRoom(_room, new Vector2Int(4, 4));
    }
    //문을 통해서 이동
    public void EnterRoom(Room _room)
    {
        Vector2Int temp;
        bool isFlipped=false;

        if (currentRoom != null)
           currentTile.OnTileObj = null;

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

    }
    public override void DestroyThis()
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

}
