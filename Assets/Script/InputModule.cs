using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
using UnityEngine.EventSystems;

public class InputModule : MonoBehaviour {


	void Start(){
#if UNITY_EDITOR
        StartCoroutine (TileSelectRoutine ());
#endif
    }
        IEnumerator TileSelectRoutine(){
		while (true) {
            if (GameManager.instance.CurrentTurn==Turn.PLAYER && GameManager.instance.IsInputOk)        
            {
                if(PlayerControl.instance.IsDirCardSelected)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        PlayerControl.instance.DoDirCard(Direction.NORTH);
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        PlayerControl.instance.DoDirCard(Direction.EAST);
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        PlayerControl.instance.DoDirCard(Direction.SOUTH);
                    }
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        PlayerControl.instance.DoDirCard(Direction.WEST);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        PlayerControl.instance.MoveToDirection(Direction.NORTH);
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        PlayerControl.instance.MoveToDirection(Direction.EAST);
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        PlayerControl.instance.MoveToDirection(Direction.SOUTH);
                    }
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        PlayerControl.instance.MoveToDirection(Direction.WEST);
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        PlayerControl.instance.EndTurnButton();
                    }
                }
            }
            yield return null;
        }
    }
}