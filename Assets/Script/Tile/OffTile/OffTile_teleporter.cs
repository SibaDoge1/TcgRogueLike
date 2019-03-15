using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_teleporter : OffTile
{
    public Vector2Int center;
    public override void SomethingUpOnThis(Entity ot)
    {
        if(ot is Player)
        {
            if(PlayerControl.player.currentRoom.IsEnemyAlive())
            {
                PlayerControl.instance.AkashaGage += 3;
                PlayerControl.player.Teleport(center);
            }
        }
    }
}
