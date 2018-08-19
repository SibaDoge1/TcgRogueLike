using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5006 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            string[] vs = new string[4];
            vs[0] = "단말기 - 1.획득한 미감정 카드의 감정 및 덱 편집";
            vs[1] = "2.아카샤 카운트 1개 충전";
            vs[2] = "덱 편집 - 카드 감정 후 감정 카드와 덱의 카드의 1대1 교체";
            vs[3] = "미편성 카드 - 덱편집에서 최종 미편성 카드는 소멸";
            UIManager.instance.ShowTextUI(vs, null);
        }
    }

}
