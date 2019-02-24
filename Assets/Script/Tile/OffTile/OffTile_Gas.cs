using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_Gas : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            PlayerControl.playerBuff.UpdateBuff(BUFF.MOVE,2);
        }
    }
}
