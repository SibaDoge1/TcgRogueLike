using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5007 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            UIManager.instance.ShowTextUI("아카샤 카운트 - 특수능력과 고등급 카드 사용 시 소모", Method1);
        }
    }
    private void Method1()
    {
        UIManager.instance.ShowTextUI("아카샤 게이지 - 적 공격 마다 10% 증가 (약점 공격 시 2배)", Method2);
    }
    private void Method2()
    {
        UIManager.instance.ShowTextUI("아카샤 게이지 100% 충전 시 카운트 1개 증가", Method3);
    }
    private void Method3()
    {
        UIManager.instance.ShowTextUI("특수능력 - 좌하단 아이콘 클릭으로 사용. 회복, 턴 넘기기 존재	", null);
    }
}
