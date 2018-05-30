using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character {
	protected Animator enemyAnimator;
    protected virtual void Start()
    {
        CurrentTurn = Cost;
        currentRoom.enemyList.Add(this);
		enemyAnimator = transform.Find ("Renderer").GetComponent<Animator> ();
    }
    protected override void OnDieCallback (){

		currentRoom.OnEnemyDead (this);
		base.OnDieCallback ();
	}

    public int Cost;
    private int currentTurn;
    protected int CurrentTurn
    {
        get { return currentTurn; }
        set
        {
            currentTurn = value;
            characterUI.SetTurnText(currentTurn);
        }
        }
    protected int damage;

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
