using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
using UnityEngine.EventSystems;

public class InputModule : MonoBehaviour {


	void Start(){
		StartCoroutine (TileSelectRoutine ());
        StartCoroutine(KeyBoardInputs());
    }
    IEnumerator KeyBoardInputs()
    {
        while(true)
        {
              if(Input.GetKeyDown(KeyCode.F5))
            {
                GameManager.instance.ReGame();
            }

              if(Input.GetKeyDown(KeyCode.Space))
            {
                PlayerControl.instance.ToggleHand();
            }
            yield return null;
        }


     }
        IEnumerator TileSelectRoutine(){
		while (true) {
            if (GameManager.instance.CurrentTurn==Turn.PLAYER && GameManager.instance.IsInputOk)        
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    PlayerControl.instance.MoveUP();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    PlayerControl.instance.MoveRight();
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    PlayerControl.instance.MoveDown();
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    PlayerControl.instance.MoveLeft();
                }
                else if(Input.GetKeyDown(KeyCode.E))
                {
                    PlayerControl.instance.EndTurnButton();
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    PlayerControl.instance.StationField();
                }
            }
            yield return null;
        }
    }
}
//TODO : ANDROID TOUCH
/* if (isPlayerTurn) {
    if (Input.GetMouseButtonDown (1) &&
        !EventSystem.current.IsPointerOverGameObject () &&
        !Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), new Vector3 (0, 0, 1), 10f)
    ){
        Tile t = GameManager.instance.GetCurrentRoom ().WorldToTile (
                    Camera.main.ScreenToWorldPoint (Input.mousePosition)
                );
        if(t!=null && t.OnTileObj == null)
        {

            if (PlayerControl.instance.PlayerMoveCommand(t))
            {
                InputModule.IsPlayerTurn = false;
            }

        }

    }
}*/
