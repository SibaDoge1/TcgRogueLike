using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Str_Wall : Structure
{
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        isHitable = false;
        name = "Wall" + _pos;
    }

}
