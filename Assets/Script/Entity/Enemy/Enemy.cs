using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character {


	protected override void OnDieCallback (){

		currentRoom.OnEnemyDead (this);

		base.OnDieCallback ();
	}
}
