using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_Floor : OffTile
{
    public int targetFloor;
    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            GameManager.instance.LoadLevel(targetFloor);
        }
   }
}
