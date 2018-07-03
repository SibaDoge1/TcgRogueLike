using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class Door : OffTile {

    private Room targetRoom;
    public Room TargetRoom
    {
        get{ return targetRoom;}
        set { targetRoom = value;}
    }

    private Direction dir;
    public Direction Dir
    {
        get {return dir;}
        set{dir = value;}
    }

    private Door connectedDoor;
    public Door ConnectedDoor
    {
        get { return connectedDoor; }
        set { connectedDoor = value; }
    }
    public override void SomethingUpOnThis(OnTileObject ot)
    {
        if (ot is Player)
        {
			(ot as Player).EnterRoom(this);
        }
    }
    public override bool IsStandAble(OnTileObject ot)
    {
        if (thisTile.OnTileObj)
        {
            return false;
        }

        if (ot is Player)
            return true;
        else
            return false;
    }
    public Tile getTile()
    {
        return thisTile;
    }

}
