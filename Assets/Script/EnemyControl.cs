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
    public void EnemyTurn()
    {
        if (currentRoom.enemyList == null || currentRoom.enemyList.Count == 0)
        {
            count = 0;
            GameManager.instance.OnEndEnemyTurn();
        }
        else
        {
            currentEnemies = new List<Enemy>(currentRoom.enemyList);
            for (int i = 0; i < currentEnemies.Count; i++)
            {
                if (currentEnemies[i] != null)
                    StartCoroutine(currentEnemies[i].AIRoutine());
            }
        }
    }



    int count;
    public void EnemyEndCallBack()
    {
        count++;
        if (count>=currentEnemies.Count)
        {
            count = 0;
            StartCoroutine(EndTurnDelay());
        }
    }
    IEnumerator EndTurnDelay()
    {
        yield return PlayerControl.player.PlayerAnim;
        GameManager.instance.OnEndEnemyTurn();
    }
}
