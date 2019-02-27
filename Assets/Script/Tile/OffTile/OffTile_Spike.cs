using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_Spike : OffTile
{

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            PlayerControl.player.GetDamage(1);
        }
    }
}
