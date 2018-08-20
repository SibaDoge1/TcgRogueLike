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
    public Sprite opened;
    public Sprite closed;
    public Sprite broken;
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
			(ot as Player).EnterRoom(this);
        }
    }
    public void Open()
    {
        Sprite.sprite = opened;
        isOpen = true;
    }
    /// <summary>
    /// 문뚤리면 공사
    /// </summary>
    public void Pave()
    {
        isOpen = false;
    }
    public void DestroyDoor()
    {
        isDestroyed = true;
        Sprite.sprite = broken;
        Sprite.sortingOrder += 999; ///다른 Ontile에 가려지는거 막기
    }
    public override bool IsStandAble(Entity et)
    {
        return isOpen;
    }
    
}
