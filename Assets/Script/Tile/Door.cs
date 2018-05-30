using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : OffTile {

    Room target;
    public override void SomethingUpOnThis(OnTileObject ot)
    {
        if (ot is Player)
        {
			(ot as Player).EnterRoom(target);
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
    public void SetTargetRoom(Room room)
    {
        target = room;
    }
}
