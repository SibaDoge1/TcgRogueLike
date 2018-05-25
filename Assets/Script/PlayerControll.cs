using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour {

    public static PlayerControll instance;
    public void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        Input.multiTouchEnabled = false;
	}
	
    public void PlayerMove(TheTile pos)
    {
        StartCoroutine(PlayerMoveRoutine(pos));    
    }
    IEnumerator PlayerMoveRoutine(TheTile pos)
    {
        Room cur = Player.instance.currentRoom;
        while(pos != Player.instance.currentTile && cur == Player.instance.currentRoom)
        {
            PathFinding.instance.GeneratePathTo(Player.instance, pos);
            TurnManager.instance.MoveNextTurn();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
