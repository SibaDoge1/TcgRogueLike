using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5003 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            string[] vs = new string[2];
            vs[0] = "카드 드로우 - 전투 상황에서 1칸 이동";
            vs[1] = "카드 정보, 범위 표시 - 카드 홀드";
            UIManager.instance.ShowTextUI(vs, null);
        }
    }
}
