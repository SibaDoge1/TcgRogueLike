using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShop : OffTile
{
    public override void SomethingUpOnThis(Entity ot)
    {
        if(ot is Player)
        {
            UIManager.instance.DeckEditUIOn(true);
            ot.currentRoom.OpenDoors();
            PlayerData.AkashaCount += 1;
            DestroyThis();
        }
    }
}
