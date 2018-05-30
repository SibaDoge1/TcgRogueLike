using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character {
	protected Animator enemyAnimator;
    protected virtual void Start()
    {
        currentRoom.enemyList.Add(this);
		enemyAnimator = transform.Find ("Renderer").GetComponent<Animator> ();
    }
    protected override void OnDieCallback (){

		currentRoom.OnEnemyDead (this);

		base.OnDieCallback ();
	}
    protected int turn = -1;
    public virtual void DoAct()
    {
        turn++;
        if (turn <= 0)
        {
            OnEndTurn();
            return;
        }
    }
    protected override void OnEndTurn()
    {
        base.OnEndTurn();
        EnemyControl.instance.EnemyEndCallBack();
    }
    protected int damage;

	protected virtual void PlayAttackMotion(){
		enemyAnimator.Play ("Attack");
	}
}
