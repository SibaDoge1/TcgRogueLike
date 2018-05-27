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
		
	}

	public void EnemyTurn(){
		
	}

	IEnumerator EnemyTurnRoutine(){
		
	}
}
