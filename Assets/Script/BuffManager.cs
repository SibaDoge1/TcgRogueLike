using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BUFF
{
    MOVE,
    CARD,
    AKASHA,
    IMMUNE
}


public class BuffManager
{
    public BuffManager()
    {
        akasha = new Debuff_Akasha();
        card = new Debuff_Card();
        move = new Debuff_Move();
        IMMUNE = new Buff_Immune();
    }
    Debuff_Akasha akasha;
    Debuff_Card card;
    Debuff_Move move;
    Buff_Immune IMMUNE;

    public bool IsHitAble
    {
        get { return IMMUNE.Turn > 0 ? false : true; }
    }
    public int ImmuneTurn
    {
        get { return IMMUNE.Turn; }
    }

    public bool IsMoveAble
    {
        get { return move.Turn>0 ? false:true ; }
    }
    public int MoveTurn
    {
        get { return move.Turn; }
    }

    public bool IsCardAble
    {
        get { return card.Turn>0 ? false:true; }
    }
    public int CardTurn
    {
        get { return card.Turn; }
    }

    public bool IsAkashaAble
    {
        get
        {
            return akasha.Turn>0 ? false:true;
        }
    }
    public int AkashaTurn
    {
        get { return akasha.Turn; }
    }

    public void OnPlayerTurn()
    {
        akasha.CountTurn();
        card.CountTurn();
        move.CountTurn();
        IMMUNE.CountTurn();
        UIManager.instance.StatusTextUpdate();
    }
    public void EraseDeBuff()
    {
        akasha.EraseBuff();
        card.EraseBuff();
        move.EraseBuff();
        UIManager.instance.StatusTextUpdate();
    }

    public void UpdateBuff(BUFF buff, int turn = 3)
    {
        switch (buff)
        {
            case BUFF.AKASHA:
                akasha.UpdateTurn(turn);
                UIManager.instance.uiAnim.ShowAnim(buff);
                SoundDelegate.instance.PlayEffectSound(SoundEffect.ERROR, PlayerControl.player.transform.position);
                break;
            case BUFF.CARD:
                card.UpdateTurn(turn);
                UIManager.instance.uiAnim.ShowAnim(buff);
                SoundDelegate.instance.PlayEffectSound(SoundEffect.ERROR, PlayerControl.player.transform.position);
                break;
            case BUFF.MOVE:
                move.UpdateTurn(turn);
                UIManager.instance.uiAnim.ShowAnim(buff);
                SoundDelegate.instance.PlayEffectSound(SoundEffect.ERROR, PlayerControl.player.transform.position);
                break;
            case BUFF.IMMUNE:
                IMMUNE.UpdateTurn(turn);
                break;
        }
        UIManager.instance.StatusTextUpdate();

    }

    #region Debuffs
    class Buff
    {
        private int turn;
        public int Turn
        {
            get { return turn; }
        }
        public void UpdateTurn(int t)
        {
            if(turn<=t)
            {
                turn = t;
            }
        }
        public void CountTurn()
        {
            if (turn > 0)
            {
                turn--;
            }
        }
        public void EraseBuff()
        {
            turn = 0;
        }
    }

     class Debuff_Move : Buff
    {

    }
     class Debuff_Card : Buff
    {

    }
     class Debuff_Akasha : Buff
    {

    }
    class Buff_Immune : Buff
    {

    }
    #endregion
}


