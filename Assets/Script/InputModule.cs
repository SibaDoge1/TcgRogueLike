using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModule : MonoBehaviour {

	public void Init(){
		Input.multiTouchEnabled = false;
	}

	IEnumerator KeyHitRoutine(){
		while (true) {
			if(Input.GetKeyDown(KeyCode.W))
			{
				Player.instance.MoveUp();
				GameManager.instance.OnEndPlayerTurn();
			}
			else if(Input.GetKeyDown(KeyCode.D))
			{
				Player.instance.MoveRight();
				GameManager.instance.OnEndPlayerTurn();
			}
			else if(Input.GetKeyDown(KeyCode.S))
			{
				Player.instance.MoveDown();
				GameManager.instance.OnEndPlayerTurn();
			}
			else if(Input.GetKeyDown(KeyCode.A))
			{
				Player.instance.MoveLeft();
				GameManager.instance.OnEndPlayerTurn();
			}
		}
	}
}
