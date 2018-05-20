using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : TheTile
{
    public override void MapUpdate(int _x, int _y)
    {
        isStandAble = true;
        base.MapUpdate(_x, _y);
    }
}
