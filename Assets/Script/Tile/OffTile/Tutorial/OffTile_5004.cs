using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTile_5004 : OffTile {

    public override void SomethingUpOnThis(Entity ot)
    {
        if (ot is Player)
        {
            string[] vs = new string[3];
            vs[0] = "카드 사용 - 카드를 위로 드래그하여 홀드 해제";
            vs[1] = "약점 속성 - 몬스터 표시 약점과 일치하는 카드로 공격 시 피해량 2배";
            vs[2] = "포탈 활성화 - 방 내의 몬스터 전원 처치";
            UIManager.instance.ShowTextUI(vs, null);
        }
    }


}
