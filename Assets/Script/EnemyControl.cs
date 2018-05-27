using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {
	public static EnemyControl instance;
	void Awake(){
		instance = this;
	}

	private List<Enemy> enemies;
	public void InitEnemy(Room room){
		enemies = room.enemyList;
	}

	public void EnemyTurn(){
		//TODO Something Enemy Action

		GameManager.instance.OnEndEnemyTurn ();
	}
}
