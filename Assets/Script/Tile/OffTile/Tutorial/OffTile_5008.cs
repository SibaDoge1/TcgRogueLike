using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5008 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            UIManager.instance.ShowTextUI("지역이동 - 지역 내 보스 처치 시 다음 지역으로 이동", null);
        }
    }

}
