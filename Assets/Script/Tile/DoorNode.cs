using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNode : Node {

    Room target;
    public override void SomethingUpOnThis(OnTileObject ot)
    {
        if(ot is Player)
        {
            Player.instance.EnterRoom(target);
        }
    }
    public DoorNode(Vector2Int _pos,Room _target) :base(_pos)
    {
        target = _target;
    }
    public override bool IsStandAble(OnTileObject me)
    {
        if (me is Player)
            return true;
        else
            return false;
    }
}
