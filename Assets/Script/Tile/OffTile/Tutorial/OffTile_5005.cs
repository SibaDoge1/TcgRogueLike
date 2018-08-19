using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5005 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            UIManager.instance.ShowTextUI("카드 획득 - 방 클리어 시 랜덤한 미감정 카드 1장 획득", Method1);
        }
    }
    private void Method1()
    {
        UIManager.instance.ShowTextUI("미감정 카드 - 감정 후 덱 편성 전까지 사용 불가", Method2);
    }
    private void Method2()
    {
        UIManager.instance.ShowTextUI("덱 확인 - 좌상단 아이콘 클릭으로 덱과 획득 카드 확인", null);
    }
}
