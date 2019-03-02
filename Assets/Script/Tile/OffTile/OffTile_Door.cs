using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class OffTile_Door : OffTile
{
    SpriteRenderer sprite;
    SpriteRenderer Sprite
    {
        get {
            if (sprite == null)
                return GetComponent<SpriteRenderer>();
            else
                return sprite;              
            }
    }
    private bool isDestroyed;
    public bool IsDestroyed
    {
        get { return isDestroyed; }
    }
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

    private OffTile_Door connectedDoor;
    public OffTile_Door ConnectedDoor
    {
        get { return connectedDoor; }
        set { connectedDoor = value; }
    }
    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            bool isVisited = targetRoom.IsVisited;
            (ot as Player).EnterRoom(this);
            if(!isVisited)
            {
                MinimapTexture.DrawHallWay(transform.position,connectedDoor.transform.position, Dir);
            }           
        }
    }
    public void Open()
    {
        switch (targetRoom.roomType)
        {
            case RoomType.BOSS:
                Sprite.sprite = ArchLoader.instance.GetDoorSprite("boss");
                break;
            case RoomType.EVENT:
                Sprite.sprite = ArchLoader.instance.GetDoorSprite("event");
                break;
            default:
                Sprite.sprite = ArchLoader.instance.GetDoorSprite("normal");
                break;
        }
        isOpen = true;
    }
    public void Close()
    {
        Sprite.sprite = ArchLoader.instance.GetDoorSprite("closed");
        isOpen = false;
    }
    public void DestroyDoor()
    {
        isDestroyed = true;
        Sprite.sprite = ArchLoader.instance.GetDoorSprite("broken");
        Sprite.sortingOrder += 999; ///다른 Ontile에 가려지는거 막기
    }

    public override bool IsStandAble(Entity et)
    {
        return isOpen;
    }
    
}
