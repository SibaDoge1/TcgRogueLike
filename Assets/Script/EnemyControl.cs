using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {
	public static EnemyControl instance;
	void Awake(){
		instance = this;
	}

    private Room currentRoom;
    private List<Enemy> currentEnemies;

    public void SetRoom(Room room){
        currentRoom = room;
    }
    public void EnemyTurn(){
        if (currentRoom.enemyList == null || currentRoom.enemyList.Count == 0)
        {
            GameManager.instance.OnEndEnemyTurn();
        }
        else
        {
            count = 0;
            currentEnemies = new List<Enemy>(currentRoom.enemyList);
            for(int i=0;i<currentEnemies.Count;i++)
            {
                if(currentEnemies[i] != null)
                StartCoroutine(currentEnemies[i].AIRoutine());
            }

        }

    }

    public int count;
    public void EnemyEndCallBack()
    {
        count++;
        if (count>=currentEnemies.Count)
        {
            Debug.Log("EnemyTurn Called");
            count = 0;
            GameManager.instance.OnEndEnemyTurn();
        }
    }
}
