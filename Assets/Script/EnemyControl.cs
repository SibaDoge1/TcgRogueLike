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
        if (enemies == null || enemies.Count == 0)
        {
            GameManager.instance.OnEndEnemyTurn();
        }
        else
        {
            foreach (Enemy e in enemies)
            {
                e.AIRoutine();
            }
        }

    }
    int count;
    public void EnemyEndCallBack()
    {
        count++;
        if (count>=enemies.Count)
        {
            count = 0;
            GameManager.instance.OnEndEnemyTurn();
        }
    }
}
