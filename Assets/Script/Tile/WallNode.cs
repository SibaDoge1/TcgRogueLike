using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNode : Node
{
    public WallNode(Vector2Int _pos) :base(_pos)
    {

    }
    public override void SomethingUpOnThis(OnTileObject ot)
    {
        Debug.Log("OH.. 뭔가 올라가면 안되는뎅");
    }
    public override bool IsStandAble(OnTileObject me)
    {
        return false;
    }
}
