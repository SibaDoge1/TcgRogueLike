using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character {

    public Attribute Atr;

    protected Animator enemyAnimator;
    protected virtual void Start()
    {
        CurrentTurn = Cost;
        currentRoom.enemyList.Add(this);
		enemyAnimator = transform.Find ("Renderer").GetComponent<Animator> ();
        characterUI.SetAtt(Atr);
    }
    protected override void OnDieCallback (){

		currentRoom.OnEnemyDead (this);
		base.OnDieCallback ();
	}

    public void AtrCheck(Attribute atr)
    {
        if(Atr == atr)
        {
            characterUI.AttIconOn();
        }else
        {
            characterUI.AttIconFlash();
        }
    }
    public int Cost;
    private int currentTurn;
    protected int CurrentTurn
    {
        get { return currentTurn; }
        set
        {
            currentTurn = value;
            if (currentTurn == 0)
            {
                characterUI.ActionIconOn(true);
            }else
            {
                characterUI.ActionIconOn(false);
            }
        }
        }
    public virtual bool DoAct()
    {
        if (FirstIgnore())
        {
            return false;
        }
        if(TurnWaiter())
        {
            return false;
        }

        return true;
    }
    protected virtual bool TurnWaiter()
    {
        CurrentTurn--;
        if (CurrentTurn >=0)
        {
            return true;
        }
        CurrentTurn = Cost;
        return false;
    }
    protected bool firstIgnore = true;
    protected virtual bool FirstIgnore()
    {
        if(firstIgnore)
        {
            firstIgnore = false;
            return true;
        }
        return false;
    }
    protected override void OnEndTurn()
    {
        base.OnEndTurn();
        EnemyControl.instance.EnemyEndCallBack();
    }


    protected virtual void PlayAttackMotion(){
		enemyAnimator.Play ("Attack");
	}
}
