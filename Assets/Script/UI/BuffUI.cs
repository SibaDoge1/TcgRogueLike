using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    Text text;
    bool text1On, text2On, text3On;

    private void Awake()
    {
        text = transform.Find("text").GetComponent<Text>();
    }

    public void TextUpdate()
    {
        string s = "";
        if(!PlayerControl.playerBuff.IsMoveAble)
        {
            s += "이동불가! \n"+ PlayerControl.playerBuff.MoveTurn+"턴";
        }
        if(!PlayerControl.playerBuff.IsAkashaAble)
        {
            s += "아카샤충전불가! \n" + PlayerControl.playerBuff.AkashaTurn + "턴";
        }
        if(!PlayerControl.playerBuff.IsCardAble)
        {
            s += "카드사용불가! \n" + PlayerControl.playerBuff.CardTurn + "턴";
        }
        if(!PlayerControl.playerBuff.IsHitAble)
        {
            s += "피해면역! \n" + PlayerControl.playerBuff.ImmuneTurn + "턴";
        }
        text.text = s;
    }


}
