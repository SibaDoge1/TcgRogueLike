using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour {

    public static PlayerControll instance;
    public void Awake()
    {
        instance = this;
    }
    public bool InputOk = false;
    // Use this for initialization
    void Start () {
        Input.multiTouchEnabled = false;
        InputOk = true;
	}
	
    public void PlayerMove(TheTile pos)
    { 
        if(InputOk)
        StartCoroutine(PlayerMoveRoutine(pos));    
    }
    IEnumerator PlayerMoveRoutine(TheTile pos)
    {
        InputOk = false;

        Room cur = Player.instance.currentRoom;
        int curHp = Player.instance.currentHp;

        List<TheTile> path = PathFinding.instance.GeneratePath(Player.instance, pos);
        while (path.Count>0 && path[0].onTile == null &&
            cur == Player.instance.currentRoom&& Player.instance.currentHp==curHp)
        {
            Player.instance.MoveTo(path[0].pos);
            path.RemoveAt(0);

            TurnManager.instance.MoveNextTurn();
            yield return new WaitForSeconds(0.1f);
        }

        InputOk = true;
    }
}
