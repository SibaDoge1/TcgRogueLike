using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShop : EventLayer
{
    public override void SomethingUpOnThis(Entity ot)
    {
        if(ot is Player)
        {
            UIManager.instance.DeckEditUIOn(true);
        }
    }
}
