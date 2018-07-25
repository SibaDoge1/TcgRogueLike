using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Debuffs
{
    protected int turn;
    public virtual void CountTurn()
    {
        turn--;
        if(turn <= 0)
        {
            PlayerControl.instance.EraseDebuff();
        }
    }

    public abstract void OnDestroy();

}
public class Debuff_Move : Debuffs
{
    public Debuff_Move()
    {
        PlayerControl.instance.Move = false;
        turn = 3;
        //TODO : MAKE EFFECT
    }
    public override void OnDestroy()
    {
        PlayerControl.instance.Move = true;
        //TODO : ERASE EFFECT
    }
}
public class Debuff_Draw : Debuffs
{
    public Debuff_Draw()
    {
        PlayerControl.instance.Draw = false;
        turn = 3;
        //TODO : MAKE EFFECT
    }
    public override void OnDestroy()
    {
        PlayerControl.instance.Draw = true;
        //TODO : ERASE EFFECT
    }
}