using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Input.multiTouchEnabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Player.instance.MoveUp();
            TurnManager.instance.MoveNextTurn();
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            Player.instance.MoveRight();
            TurnManager.instance.MoveNextTurn();
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            Player.instance.MoveDown();
            TurnManager.instance.MoveNextTurn();
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            Player.instance.MoveLeft();
            TurnManager.instance.MoveNextTurn();
        }
    }
}
