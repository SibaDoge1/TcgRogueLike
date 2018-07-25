using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class Door : EventLayer
{

    private Room targetRoom;
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
}
