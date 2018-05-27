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
	}

	IEnumerator TileSelectRoutine(){
		while (true) {
			//TODO : ANDROID TOUCH
			if (isPlayerTurn) {
				if (Input.GetMouseButtonDown (0) &&
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
			}
			yield return null;
		}
	}
}
