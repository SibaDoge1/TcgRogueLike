using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    Animator anim;
    Text text;
    bool text1On, text2On, text3On;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        text = transform.Find("text").GetComponent<Text>();
    }

    public void TextUpdate()
    {
        string s = "";
        if(!PlayerControl.playerBuff.IsMoveAble)
        {
            s += "이동불가! \n";
        }
        if(!PlayerControl.playerBuff.IsAkashaAble)
        {
            s += "아카샤충전불가! \n";
        }
        if(!PlayerControl.playerBuff.IsCardAble)
        {
            s += "카드사용불가! \n";
        }
        if(!PlayerControl.playerBuff.IsHitAble)
        {
            s += "피해면역! \n";
        }
        text.text = s;
    }

    public void ShowAnim()
    {        
        anim.Play("errorPopUp",-1,0);
    }
}
