using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5003 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        GameManager.instance.IsInputOk = false;
        if (ot is Player)
        {
            UIManager.instance.ShowTextUI("카드 드로우 - 전투 상황에서 1칸 이동", Method1);
        }
    }
    private void Method1()
    {
        UIManager.instance.ShowTextUI("카드 정보, 범위 표시 - 카드 홀드", new CallBack(delegate () { GameManager.instance.IsInputOk = true; }));
    }
}
