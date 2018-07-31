using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Str_Wall : Structure
{
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        name = "Wall" + _pos;
    }
    protected override void OnDieCallback()
    {
        //TODO : EFFECT
        base.OnDieCallback();
    }
}
