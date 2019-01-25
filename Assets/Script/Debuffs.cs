using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Debuffs
{
    protected int turn;
    protected GameObject effect;
    public virtual void CountTurn()
    {
        turn--;
        if(turn <= 0)
        {
            PlayerControl.instance.EraseDebuff();
        }
    }

    public abstract void Active();
    public abstract void OnDestroy();

}
public class Debuff_Move : Debuffs
{
    public override void Active()
    {
        turn = 3;
        PlayerControl.instance.IsMoveAble = false;
        UIManager.instance.ErrorPopUpOn(this);
    }
    public override void OnDestroy()
    {
        PlayerControl.instance.IsMoveAble = true;
        UIManager.instance.ErrorPopUpOff();
    }
}
public class Debuff_Draw : Debuffs
{
    public override void Active()
    {
        PlayerControl.instance.IsDrawAble = false;
        turn = 3;
        //TODO : MAKE EFFECT
    }
    public override void OnDestroy()
    {
        PlayerControl.instance.IsDrawAble = true;
        //TODO : ERASE EFFECT
    }
}
//게이지 충전 불가