using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5005 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            string[] vs = new string[3];
            vs[0] = "카드 획득 - 방 클리어 시 랜덤한 미감정 카드 1장 획득";
            vs[1] = "미감정 카드 -감정 후 덱 편성 전까지 사용 불가";
            vs[2] = "덱 확인 - 좌상단 아이콘 클릭으로 덱과 획득 카드 확인";
            UIManager.instance.ShowTextUI(vs, null);
        }
    }

}
