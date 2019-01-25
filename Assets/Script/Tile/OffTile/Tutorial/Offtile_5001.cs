using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offtile_5001 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            string[] vs = new string[3];
            vs[0] = "기본조작 - 키보드 & 마우스";
            vs[1] = "이동 - WASD";
            vs[2] = "활성화된 포탈 - 연결된 방으로 이동";
            UIManager.instance.ShowTextUI(vs, null);              
        }
    }      

}
