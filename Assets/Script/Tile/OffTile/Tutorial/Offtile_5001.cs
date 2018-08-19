using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offtile_5001 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            UIManager.instance.ShowTextUI("기본조작 - 키보드 & 마우스", Method1);              
        }
    }      
    private void Method1()
    {
        UIManager.instance.ShowTextUI("이동 - WASD", Method2);
    }
    private void Method2()
    {
        UIManager.instance.ShowTextUI("활성화된 포탈 - 연결된 방으로 이동", null);
    }
}
