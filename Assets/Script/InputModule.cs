using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class InputModule : MonoBehaviour {
	private static InputModule instance;
	void Awake(){
		instance = this;
	}
	private static bool inputOK = true;
	public static bool InputOK {
		get {
			return inputOK;
		}
		set {
			inputOK = value;
		}
	}

	void Start(){
		StartCoroutine (TileSelectRoutine ());
	}

	IEnumerator TileSelectRoutine(){
		while (true) {
			//TODO ANDROID TOUCH
			if (inputOK) {
				if (Input.GetMouseButtonDown (1)) {
					Tile t = GameManager.instance.GetCurrentRoom ().WorldToTile (
						        Camera.main.ScreenToWorldPoint (Input.mousePosition)
					        );
                    if(t!=null && t.OnTileObj == null)
                    {

                        if (PlayerControl.instance.PlayerMoveCommand(t))
                        {
                            InputModule.InputOK = false;
                        }

                    }

				}
			}
			yield return null;
		}
	}
}
