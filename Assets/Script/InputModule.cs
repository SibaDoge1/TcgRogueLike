using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
using UnityEngine.EventSystems;

public class InputModule : MonoBehaviour {
	private static InputModule instance;
	void Awake(){
		instance = this;
	}
	private static bool isPlayerTurn = true;
	public static bool IsPlayerTurn {
		get {
			return isPlayerTurn;
		}
		set {
			isPlayerTurn = value;
		}
	}

	void Start(){
		StartCoroutine (TileSelectRoutine ());
        StartCoroutine(KeyBoardInputs());
    }
    IEnumerator KeyBoardInputs()
    {
        while(true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerControl.instance.ToggleHand();
            }
        #if UNITY_EDITOR
              if(Input.GetKeyDown(KeyCode.F1))
            {
                GameManager.instance.ReGame();
            }
        #else
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.R))
            {
                GameManager.instance.ReGame();
            }
        #endif

            yield return null;
        }


     }
        IEnumerator TileSelectRoutine(){
		while (true) {

            //TODO : HERE IS TEMP!
            if (IsPlayerTurn)        
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
                else if(Input.GetKeyDown(KeyCode.Q))
                {
                    PlayerControl.instance.EndTurnButton();
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
