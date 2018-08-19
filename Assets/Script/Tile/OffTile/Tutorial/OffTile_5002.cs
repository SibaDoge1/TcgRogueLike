using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5002 : OffTile
{

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            string[] vs = new string[1];
            vs[0] = "미니맵 - 우상단 표시";
            UIManager.instance.ShowTextUI(vs, null);
        }
    }
}
