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
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Player.instance.MoveUp();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Player.instance.MoveRight();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Player.instance.MoveDown();
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Player.instance.MoveLeft();
        }
	}
}
