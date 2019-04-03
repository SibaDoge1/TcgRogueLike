using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_Save : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if(ot is Player)
        {
            UIManager.instance.SaveUIOn(DestroyThis);
            DestroyThis();
        }
    }

}
