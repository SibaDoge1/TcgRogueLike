using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class Door : OffTile
{
    SpriteRenderer sprite;
    public Sprite opened;
    public Sprite closed;
    public Sprite broken;

    bool isOpen;

    private Room targetRoom;
    protected void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        isEvent = true;
    }
    public Room TargetRoom
    {
        get{ return targetRoom;}
        set { targetRoom = value;}
    }

    public Direction Dir;

    private Door connectedDoor;
    public Door ConnectedDoor
    {
        get { return connectedDoor; }
        set { connectedDoor = value; }
    }
    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
			(ot as Player).EnterRoom(this);
        }
    }
    public void Open()
    {
        sprite.sprite = opened;
        isOpen = true;
    }
    /// <summary>
    /// 문뚤리면 공사
    /// </summary>
    public void Pave()
    {
        isOpen = false;
        currentTile.OnTileObj.DestroyThis();
        switch(Dir)
        {
            case Direction.NORTH:
                break;
            case Direction.SOUTH:
                break;
            case Direction.EAST:
                break;
            case Direction.WEST:
                break;
        }
    }
    public void DestroyDoor()
    {
        sprite.sprite = broken;
    }
    public override bool IsStandAble(Entity et)
    {
        if(et is Structure)
        {
            return true;
        }       
        else
        {
            Debug.Log(isOpen);
            return isOpen;
        }
    }
    
}
