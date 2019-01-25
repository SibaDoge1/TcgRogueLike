using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5009 : OffTile
{
    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            string[] vs = new string[1];
            vs[0] = "<1층 - 거미 신부>";
            UIManager.instance.ShowTextUI(vs, null);
            Destroy(gameObject);
        }
    }

}
