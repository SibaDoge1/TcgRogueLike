using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_Shop : OffTile
{
    public override void SomethingUpOnThis(Entity ot)
    {
        if(ot is Player)
        {
            SoundDelegate.instance.PlayEffectSound(SoundEffect.CONNECT, transform.position);
            UIManager.instance.DeckEditUIOn();
            ot.currentRoom.OpenDoors();
            DestroyThis();
        }
    }
}
