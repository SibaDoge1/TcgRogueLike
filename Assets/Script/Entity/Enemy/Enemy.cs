using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character {

    protected virtual void Start()
    {
        currentRoom.enemyList.Add(this);
    }
    protected override void OnDieCallback (){

		currentRoom.OnEnemyDead (this);

		base.OnDieCallback ();
	}
    public abstract void DoAct();
    protected override void OnEndTurn()
    {
        base.OnEndTurn();
        EnemyControl.instance.EnemyEndCallBack();
    }
    protected int damage;
}
