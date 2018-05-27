using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
			Debug.LogError ("SingleTone Error");
		}
	}

	private void Start() {	//Start of Everything
		//DECK CONSTRUCTION
		for (int i = 0; i < 20; i++) {
			PlayerData.deck.Add (new CardData_Sword (0));
		}

		currentFloor = MapGenerator.GetNewMap(0,new Vector2Int(10,10),10);
		PlayerControl.instance.InitPlayer (currentFloor.StartRoom);
	}

    
    private Map currentFloor;
	public Room GetCurrentRoom(){
		return currentFloor.CurrentRoom;
	}
	public void SetCurrentRoom(Room room_){
		currentFloor.CurrentRoom = room_;
	}
    
	public void OnPlayerEnterNewRoom(){
		if (currentFloor.CurrentRoom.IsCleares == false) {
			EnemyControl.instance.InitEnemy (currentFloor.CurrentRoom);
			//Close the door
		}
	}

    public void OnEndPlayerTurn()
    {
		EnemyControl.instance.EnemyTurn ();
    }

    public void OnEndEnemyTurn()
    {
		if (PlayerControl.instance.GetRemainAction () <= 0) {
			InputModule.InputOK = true;
		} else if (PlayerControl.instance.ActionResume () == false) {
			InputModule.InputOK = true;
		}
    }
}
